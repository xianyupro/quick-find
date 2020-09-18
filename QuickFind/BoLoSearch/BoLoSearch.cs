using QuickFind;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace BoLoSearch
{
    public class BoLoSearch
    {
        private string DBpath = @"C:\Program Files\菠萝工具箱\boloSearch.db";
        private bool DB_Ready = false;
        private List<string> UpdataString1 = new List<string>();
        private List<string> UpdataString2 = new List<string>();
        private bool addflag = true;
        public BoLoSearch()
        {
            if (File.Exists(DBpath))
            {
                File.Delete(DBpath);
            }
            NewDbFile(DBpath, "FileTable");  //"Data Source = boloSearch.db"
            Thread initDB_thread = new Thread(() => { QueryEngine.Engine.GetAllFilesAndDirectories(@"Data Source = " + DBpath, DBpath); DB_Ready = true; });
            initDB_thread.IsBackground = true;
            initDB_thread.Start();

            IEnumerable<DriveInfo> fixedNtfsDrives = QueryEngine.Engine.GetAllFixedNtfsDrives();
            FileSystemWatcher[] wathcerArr = new FileSystemWatcher[fixedNtfsDrives.Count()];
            for (int i = 0; i < fixedNtfsDrives.Count(); i++)
            {
                wathcerArr[i] = new FileSystemWatcher();
                wathcerArr[i].Path = fixedNtfsDrives.ElementAt(i).Name;
                wathcerArr[i].NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                   | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                //wathcerArr[i].Changed += new FileSystemEventHandler(OnProcess);
                wathcerArr[i].Created += new FileSystemEventHandler(OnProcess);
                wathcerArr[i].Deleted += new FileSystemEventHandler(OnProcess);
                wathcerArr[i].Renamed += new RenamedEventHandler(OnFileRenamed);

                // Begin watching.
                wathcerArr[i].EnableRaisingEvents = true;
                wathcerArr[i].IncludeSubdirectories = true;
            }
            Thread Updata_Thread = new Thread(() => { AutoUpdataDB(); });
            Updata_Thread.IsBackground = true;
            Updata_Thread.Start();

        }

        private void OnFileRenamed(object sender, RenamedEventArgs e)
        {
            var file = e.FullPath;
            if (file.Substring(file.Length - 3, 3) == "lnk") return;
            if (addflag)
            {
                UpdataString1.Add("Rename," + e.FullPath + "," + e.OldFullPath + "," + e.Name + "," + e.OldName);
            }
            else
            {
                UpdataString2.Add("Rename," + e.FullPath + "," + e.OldFullPath + "," + e.Name + "," + e.OldName);
            }
        }

        private void OnProcess(object sender, FileSystemEventArgs e)
        {
            var file = e.FullPath;
            if (file.Substring(file.Length - 3, 3) == "lnk") return;
            if (addflag)
            {
                UpdataString1.Add(e.ChangeType + "," + e.Name + "," + e.FullPath);
            }
            else
            {
                UpdataString2.Add(e.ChangeType + "," + e.Name + "," + e.FullPath);
            }
            //Console.WriteLine(e.FullPath + "  " + e.ChangeType);
        }

        public static Boolean NewDbFile(string m_FilePath, string tableName)
        {
            try
            {
                SQLiteConnection.CreateFile(m_FilePath);

                SQLiteConnection sqliteConn = new SQLiteConnection("data source=" + m_FilePath);
                if (sqliteConn.State != System.Data.ConnectionState.Open)
                {
                    sqliteConn.Open();
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.Connection = sqliteConn;

                    cmd.CommandText = "CREATE TABLE " + tableName + "(FileName Text,FullFileName Text, isFolder boolean)";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "CREATE INDEX FilesIndex ON " + tableName + "(FileName)";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "CREATE INDEX FullFileNameIndex ON " + tableName + "(FullFileName)";
                    cmd.ExecuteNonQuery();
                }
                sqliteConn.Close();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("新建数据库文件" + m_FilePath + "失败：" + ex.Message);
            }


        }


        private void AutoUpdataDB()
        {
            while (true)
            {
                if (DB_Ready)
                {
                    Thread.Sleep(50);
                    if (addflag)
                    {
                        if (UpdataString1.Count > 10000 || QuickForm.StartUpdateDB)
                        {
                            addflag = false;
                            UpdateDB(UpdataString1);
                            UpdataString1.Clear();
                            QuickForm.StartUpdateDB = false;
                        }
                    }
                    else
                    {
                        if (UpdataString2.Count > 10000 || QuickForm.StartUpdateDB)
                        {
                            addflag = true;
                            UpdateDB(UpdataString2);
                            UpdataString2.Clear();
                            QuickForm.StartUpdateDB = false;
                        }
                    }
                }
            }
        }

        private void UpdateDB(List<string> keyString)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            SQLiteConnection connection = new SQLiteConnection(@"Data Source = " + DBpath);
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "PRAGMA synchronous = OFF";
            command.ExecuteNonQuery();
            using (SQLiteTransaction mytransaction = connection.BeginTransaction())
            {

                SQLiteCommand mycommandInsert = new SQLiteCommand(connection);
                SQLiteParameter[] myparam = new SQLiteParameter[3];
                myparam[0] = new SQLiteParameter();
                myparam[1] = new SQLiteParameter();
                myparam[2] = new SQLiteParameter();
                mycommandInsert.CommandText = "INSERT INTO [FileTable] ([FileName],[FullFileName],[isFolder]) VALUES(?,?,?)";
                mycommandInsert.Parameters.AddRange(myparam);

                //DELETE FROM COMPANY WHERE ID = 7
                SQLiteCommand mycommandDelete = new SQLiteCommand(connection);
                SQLiteParameter myparamDelete = new SQLiteParameter();
                mycommandDelete.CommandText = "DELETE FROM [FileTable] WHERE [FullFileName] = ?";
                mycommandDelete.Parameters.Add(myparamDelete);

                //UPDATE table_name SET column1 = value1, column2 = value2...., columnN = valueN WHERE[condition];
                SQLiteCommand mycommandRename = new SQLiteCommand(connection);
                SQLiteParameter[] myparamRename = new SQLiteParameter[3];
                myparamRename[0] = new SQLiteParameter();
                myparamRename[1] = new SQLiteParameter();
                myparamRename[2] = new SQLiteParameter();
                mycommandRename.CommandText = "UPDATE [FileTable] SET [FullFileName]=?,[FileName]=?  WHERE [FullFileName] = ?";
                mycommandRename.Parameters.AddRange(myparamRename);

                for (int i = 0; i < keyString.Count; i++)
                {
                    string[] arr = keyString[i].Split(',');
                    if (arr[0] == "Created")
                    {
                        myparam[0].Value = Directory.Exists(arr[2]) ? new DirectoryInfo(arr[2]).Name : new FileInfo(arr[2]).Name;
                        myparam[1].Value = arr[2];
                        myparam[2].Value = Directory.Exists(arr[2]);
                        mycommandInsert.ExecuteNonQuery();
                    }
                    if (arr[0] == "Deleted")
                    {
                        myparamDelete.Value = arr[2];
                        mycommandDelete.ExecuteNonQuery();
                    }
                    if (arr[0] == "Rename")
                    {
                        myparamRename[0].Value = arr[1];
                        myparamRename[1].Value = Directory.Exists(arr[1]) ? new DirectoryInfo(arr[1]).Name : new FileInfo(arr[1]).Name;
                        myparamRename[2].Value = arr[2];
                        mycommandRename.ExecuteNonQuery();
                    }
                }
                mycommandInsert.Dispose();
                mycommandDelete.Dispose();
                mycommandRename.Dispose();
                mytransaction.Commit();
            }
            connection.Close();
            stopWatch.Stop();
            Console.WriteLine(".................................." + stopWatch.ElapsedMilliseconds);

        }

        public (List<string[]>, List<string[]>) getRet(string keyWords)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<string[]> Dirs = new List<string[]>();
            List<string[]> Flists = new List<string[]>();
            SQLiteConnection connection = new SQLiteConnection(@"Data Source = " + DBpath);
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "PRAGMA synchronous = OFF";
            command.ExecuteNonQuery();
            using (SQLiteTransaction mytransaction = connection.BeginTransaction())
            {
                //command.CommandText = @"SELECT * FROM FileTable where instr([FileName],'" + keyWords + @"')>0 limit 400";
                command.CommandText = @"SELECT * FROM FileTable where [FileName] LIKE '%" + keyWords + @"%' limit 400";
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if ((bool)reader[2])
                    {
                        Dirs.Add(new string[2] { (string)reader[0], (string)reader[1] });
                    }
                    else
                    {
                        Flists.Add(new string[2] { (string)reader[0], (string)reader[1] });
                    }

                }
                reader.Close();
                mytransaction.Commit();
            }
            connection.Close();
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
            return (Dirs.OrderByDescending(item => new DirectoryInfo(item[1]).LastWriteTime).ToList(), Flists);
        }

    }
}
