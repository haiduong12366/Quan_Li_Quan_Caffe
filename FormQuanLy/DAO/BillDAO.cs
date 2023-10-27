using FormQuanLy.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormQuanLy.DAO
{
    public class BillDAO
    {
        private static BillDAO instance;
        public static BillDAO Instance
        {
            get { if (instance == null) instance = new BillDAO(); return BillDAO.instance; }
            private set => BillDAO.instance = value;
        }

        private BillDAO() { }

        public int GetUncheckBillIDByTableID(int id)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("select * from bill where idTable = " + id + " and status = 0");
            if (data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.ID;
            }
            return -1;
        }
        public void checkOut(int id,int discount, float totalPrice)
        {
            string query = "Update bill set totalPrice = " + totalPrice + " , datecheckout = getdate(), status = 1, discount = " + discount + " where id = " + id;
            DataProvider.Instance.ExecuteQuery(query);  
        }
        public void InsertBill(int id)
        {
            DataProvider.Instance.ExecuteNonQuery("USP_InsertBill @idTable", new object[] { id });
        }
        public DataTable GetBillListByDate(DateTime checkIn,DateTime checkOut)
        {
            return DataProvider.Instance.ExecuteQuery("USP_GetListBillByDate @checkIn , @checkOut", new object[] { checkIn, checkOut });
        }
        public DataTable GetBillListByDateAndPage(DateTime checkIn, DateTime checkOut,int page)
        {
            return DataProvider.Instance.ExecuteQuery("USP_GetListBillByDateAndPage @checkIn , @checkOut , @page ", new object[] { checkIn, checkOut,page });
        }

        public int GetNumBillByDateAndPage(DateTime checkIn, DateTime checkOut)
        {
            return (int)DataProvider.Instance.ExecuteScalar("USP_GetNumBillByDate @checkIn , @checkOut ", new object[] { checkIn, checkOut});
        }
        public int GetMaxIDBill()
        {
            try
            {
                return (int)DataProvider.Instance.ExecuteScalar("select max(id) from bill");
            }
            catch (Exception ex)
            {
                return 1;
            }
        }
    }
}
