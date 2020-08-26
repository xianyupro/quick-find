using System;
using System.Collections;
using System.Windows.Forms;

namespace QuickFind.Helper
{
    //文本比较器
    public class ListViewItemComparer : IComparer
    {
        public bool sort_b;
        public SortOrder order = SortOrder.Ascending;
        private int col;
        public ListViewItemComparer()
        {
            col = 0;
        }
        public ListViewItemComparer(int column, bool sort)
        {
            col = column;
            sort_b = sort;
        }
        public int Compare(object x, object y)
        {
            if (sort_b)
            {
                return String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
            }
            else
            {
                return String.Compare(((ListViewItem)y).SubItems[col].Text, ((ListViewItem)x).SubItems[col].Text);
            }
        }
    }
    //数字比较器
    public class ListViewItemComparerNum : IComparer
    {
        public bool sort_b;
        public SortOrder order = SortOrder.Ascending;
        private int col;
        public ListViewItemComparerNum()
        {
            col = 0;
        }
        public ListViewItemComparerNum(int column, bool sort)
        {
            col = column;
            sort_b = sort;
        }
        public int Compare(object x, object y)
        {
            decimal d1 = Convert.ToDecimal(((ListViewItem)x).SubItems[col].Text);
            decimal d2 = Convert.ToDecimal(((ListViewItem)y).SubItems[col].Text);
            if (sort_b)
            {
                return decimal.Compare(d1, d2);
            }
            else
            {
                return decimal.Compare(d2, d1);
            }
        }
    }
    //日期比较器
    public class ListViewItemComparerDate : IComparer
    {
        public bool sort_b;
        public SortOrder order = SortOrder.Ascending;
        private int col;
        public ListViewItemComparerDate()
        {
            col = 0;
        }
        public ListViewItemComparerDate(int column, bool sort)
        {
            col = column;
            sort_b = sort;
        }
        public int Compare(object x, object y)
        {
            DateTime d1 = Convert.ToDateTime(((ListViewItem)x).SubItems[col].Text);
            DateTime d2 = Convert.ToDateTime(((ListViewItem)y).SubItems[col].Text);
            if (sort_b)
            {
                return DateTime.Compare(d1, d2);
            }
            else
            {
                return DateTime.Compare(d2, d1);
            }
        }
    }
}
