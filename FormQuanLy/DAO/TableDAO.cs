using FormQuanLy.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormQuanLy.DAO
{
    internal class TableDAO
    {
        private static TableDAO instance;
        public static TableDAO Instance
        {
            get { if (instance == null) instance = new TableDAO(); return TableDAO.instance; }
            private set => TableDAO.instance = value;
        }

        public static int TableWidth = 85;
        public static int TableHeight = 85;
        private TableDAO() { }

        public List<Table> LoadTableList()
        {
            List<Table> tableList = new List<Table>();

            DataTable data = DataProvider.Instance.ExecuteQuery("USP_GetTableList");

            foreach (DataRow row in data.Rows )
            {
                Table table = new Table(row);
                tableList.Add(table);
            }

            return tableList;
        }

        public void SwitchTable(int id1,int id2)
        {
            DataProvider.Instance.ExecuteQuery("USP_SwitchTable @idtable1 , @idtable2",new object[] {id1,id2});   
        }
        public void MergeTable(int id1, int id2)
        {
            DataProvider.Instance.ExecuteQuery("USP_MergeTable @idtable1 , @idtable2", new object[] { id1, id2 });
        }
    }
}
