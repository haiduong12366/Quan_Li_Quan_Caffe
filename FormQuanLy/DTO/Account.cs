using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormQuanLy.DTO
{
    public class Account
    {

        private string userName; 
        private string password;
        private string displayName;
        private int type;

        public Account(string userName,string displayName,int type,string password = null)
        {
            this.userName = userName;
            this.displayName = displayName;
            this.type = type;
            this.password = password;
        }
        public Account(int type)
        {
            this.type = type;          
        }
        public Account(DataRow row)
        {
            this.userName = row["userName"].ToString();
            this.displayName = row["displayName"].ToString();
            this.type = (int)row["type"];
            this.password = row["password"].ToString(); ;
        }
        public string UserName { get => userName; set => userName = value; }
        public string Password { get => password; set => password = value; }
        public string DisplayName { get => displayName; set => displayName = value; }
        public int Type { get => type; set => type = value; }
    }
}
