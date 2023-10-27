using FormQuanLy.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormQuanLy.DAO
{
    internal class BillInfoDAO
    {
        private static BillInfoDAO instance;
        public static BillInfoDAO Instance
        {
            get { if (instance == null) instance = new BillInfoDAO(); return BillInfoDAO.instance; }
            private set => BillInfoDAO.instance = value;
        }

        private BillInfoDAO() { }

        public List<BillInfo> GetListBillInfo(int id)
        {
            List<BillInfo> listBillInfo = new List<BillInfo>();
            DataTable data = DataProvider.Instance.ExecuteQuery("select * from billinfo where idbill = " + id);

            foreach (DataRow row in data.Rows)
            {
                BillInfo info = new BillInfo(row);
                listBillInfo.Add(info);
            }
            return listBillInfo;
        }
        public void InsertBillInfo(int idBill, int idFood, int count)
        {
            DataProvider.Instance.ExecuteNonQuery("USP_InsertBillInfo @idbill , @idFood , @count", new object[] { idBill, idFood, count });
        }
        public void DeleteBillInfoByIDFood(int idFood)
        {
            DataProvider.Instance.ExecuteNonQuery("delete billinfo where idfood = " + idFood);
        }
    }
}
