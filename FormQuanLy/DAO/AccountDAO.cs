using FormQuanLy.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace FormQuanLy.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;
        public static AccountDAO Instance
        {
            get { if (instance == null) instance = new AccountDAO(); return AccountDAO.instance; }
            private set => AccountDAO.instance = value;
        }

        private AccountDAO() { }

        public bool UpdateAccount(string userName, string displayName, string pass, string newPass)
        {
            int data = DataProvider.Instance.ExecuteNonQuery("USP_UpdateAccount @username , @displayName , @pass , @newPass ", new object[] { userName, displayName, pass, newPass });

            return data > 0;
        }
        public DataTable GetListAccount()
        {
            return DataProvider.Instance.ExecuteQuery("select UserName, displayName, Type from Account");
        }
        public List<Account> cbAccount()
        {
            List<Account> list = new List<Account>();

            string query = "select distinct Type from Account";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow row in data.Rows)
            {
                Account account = new Account((int)row["type"]);
                list.Add(account);
            }
            return list;

        }

        public bool Login(string userName, string passWord)
        {

            byte[] temp = ASCIIEncoding.ASCII.GetBytes(passWord);
            byte[] hashData = new MD5CryptoServiceProvider().ComputeHash(temp);

            string hasPass = "";
            //var list = hashData.ToString();
            //list.Reverse();
            foreach(byte item in hashData)
            {
                hasPass += item;
            }    
            string query = "USp_Login @username , @password";

            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { userName, hasPass });

            return data.Rows.Count == 1;
        }

        public Account GetAccountByUserName(string userName)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("select * from account where username = '" + userName + "'");
            foreach (DataRow item in data.Rows)
            {
                return new Account(item);
            }
            return null;
        }

        public bool InsertAccount(string userName, string displayName, int type)
        {
            string query = string.Format("insert into Account(username , displayName, type, password) values (N'{0}',N'{1}',{2},N'1962026656160185351301320480154111117132155')", userName, displayName, type);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool UpdateAccount(string userName, string displayName, int type)
        {
            string query = string.Format("Update Account set displayName = N'{0}', type = {1} where username = N'{2}'", displayName, type, userName);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool DeleteAccount(string username)
        {
            string query = string.Format("delete account where username = N'{0}'", username);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool ResetAccount(string username)
        {
            string query = string.Format("update account set password = N'1962026656160185351301320480154111117132155' where username = N'{0}'", username);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
    }
}
