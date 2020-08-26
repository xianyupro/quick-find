using QuickFind;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using UsnOperation;

namespace QueryEngine
{
    public class Engine
    {
        /// <summary>
        /// When its values is 1407374883553285(0x5000000000005L), it means this file/folder is under drive root
        /// </summary>
        protected const UInt64 ROOT_FILE_REFERENCE_NUMBER = 0x5000000000005L;

        protected static readonly string excludeFolders = string.Join("|",
            new string[]
            {
                "$RECYCLE.BIN",
                "System Volume Information",
                "$AttrDef",
                "$BadClus",
                "$BitMap",
                "$Boot",
                "$LogFile",
                "$Mft",
                "$MftMirr",
                "$Secure",
                "$TxfLog",
                "$UpCase",
                "$Volume",
                "$Extend"
            }).ToUpper();

        public static IEnumerable<DriveInfo> GetAllFixedNtfsDrives()
        {
            return DriveInfo.GetDrives()
                .Where(d => d.DriveType == DriveType.Fixed && d.DriveFormat.ToUpper() == "NTFS");
        }

        public static void GetAllFilesAndDirectories(string constring, string DBpath)
        {
            List<FileAndDirectoryEntry> result = new List<FileAndDirectoryEntry>();

            IEnumerable<DriveInfo> fixedNtfsDrives = GetAllFixedNtfsDrives();

            foreach (var drive in fixedNtfsDrives)
            {
                var usnOperator = new UsnOperator(drive);
                var usnEntries = usnOperator.GetEntries().Where(e => !excludeFolders.Contains(e.FileName.ToUpper()));
                var folders = usnEntries.Where(e => e.IsFolder).ToArray();
                List<FrnFilePath> paths = GetFolderPath(folders, drive);

                result.AddRange(usnEntries.Join(
                    paths,
                    usn => usn.ParentFileReferenceNumber,
                    path => path.FileReferenceNumber,
                    (usn, path) => new FileAndDirectoryEntry(usn, path.Path)));
                paths.Clear();
            }
            SQLiteConnection connection = new SQLiteConnection("Data Source = " + DBpath);
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(connection);

            command.CommandText = "PRAGMA synchronous = OFF";
            command.ExecuteNonQuery();
            using (SQLiteTransaction mytransaction = connection.BeginTransaction())
            {
                using (SQLiteCommand mycommand = new SQLiteCommand(connection))
                {
                    SQLiteParameter[] myparam = new SQLiteParameter[3];
                    myparam[0] = new SQLiteParameter();
                    myparam[1] = new SQLiteParameter();
                    myparam[2] = new SQLiteParameter();
                    int n;

                    mycommand.CommandText = "INSERT INTO [FileTable] ([FileName],[FullFileName],[isFolder]) VALUES(?,?,?)";
                    mycommand.Parameters.AddRange(myparam);

                    for (n = 0; n < result.Count; n++)
                    {
                        myparam[0].Value = result[n].FileName;
                        myparam[1].Value = result[n].FullFileName;
                        myparam[2].Value = result[n].IsFolder;
                        mycommand.ExecuteNonQuery();
                    }
                }
                mytransaction.Commit();
            }
            result.Clear();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            connection.Close();
            //MessageBox.Show("搜索功能初始化完成。");
            QuickForm.FinishInitSearch = true;
            return;
        }

        private static List<FrnFilePath> GetFolderPath(UsnEntry[] folders, DriveInfo drive)
        {
            Dictionary<UInt64, FrnFilePath> pathDic = new Dictionary<ulong, FrnFilePath>();
            pathDic.Add(ROOT_FILE_REFERENCE_NUMBER,
                new FrnFilePath(ROOT_FILE_REFERENCE_NUMBER, null, string.Empty, drive.Name.TrimEnd('\\')));

            foreach (var folder in folders)
            {
                pathDic.Add(folder.FileReferenceNumber,
                    new FrnFilePath(folder.FileReferenceNumber, folder.ParentFileReferenceNumber, folder.FileName));
            }

            Stack<UInt64> treeWalkStack = new Stack<ulong>();

            foreach (var key in pathDic.Keys)
            {
                treeWalkStack.Clear();

                FrnFilePath currentValue = pathDic[key];

                if (string.IsNullOrWhiteSpace(currentValue.Path)
                    && currentValue.ParentFileReferenceNumber.HasValue
                    && pathDic.ContainsKey(currentValue.ParentFileReferenceNumber.Value))
                {
                    FrnFilePath parentValue = pathDic[currentValue.ParentFileReferenceNumber.Value];

                    while (string.IsNullOrWhiteSpace(parentValue.Path)
                        && parentValue.ParentFileReferenceNumber.HasValue
                        && pathDic.ContainsKey(parentValue.ParentFileReferenceNumber.Value))
                    {
                        currentValue = parentValue;

                        if (currentValue.ParentFileReferenceNumber.HasValue
                            && pathDic.ContainsKey(currentValue.ParentFileReferenceNumber.Value))
                        {
                            treeWalkStack.Push(key);
                            parentValue = pathDic[currentValue.ParentFileReferenceNumber.Value];
                        }
                        else
                        {
                            parentValue = null;
                            break;
                        }
                    }

                    if (parentValue != null)
                    {
                        currentValue.Path = BuildPath(currentValue, parentValue);

                        while (treeWalkStack.Count() > 0)
                        {
                            UInt64 walkedKey = treeWalkStack.Pop();

                            FrnFilePath walkedNode = pathDic[walkedKey];
                            FrnFilePath parentNode = pathDic[walkedNode.ParentFileReferenceNumber.Value];

                            walkedNode.Path = BuildPath(walkedNode, parentNode);
                        }
                    }
                }
            }

            var result = pathDic.Values.Where(p => !string.IsNullOrWhiteSpace(p.Path) && p.Path.StartsWith(drive.Name)).ToList();

            return result;
        }

        private static string BuildPath(FrnFilePath currentNode, FrnFilePath parentNode)
        {
            return string.Concat(new string[] { parentNode.Path, "\\", currentNode.FileName });
        }
    }
}
