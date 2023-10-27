using FormQuanLy.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormQuanLy.DAO
{
    public class FoodDAO
    {
        private static FoodDAO instance;
        public static FoodDAO Instance
        {
            get { if (instance == null) instance = new FoodDAO(); return FoodDAO.instance; }
            private set => FoodDAO.instance = value;
        }

        private FoodDAO() { }

        public List<Food> GetFoodByCetegoryID(int id)
        {
            List<Food> list =  new List<Food>();

            string query = "select * from Food where idCategory = " + id;

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow row in data.Rows)
            {
                Food food = new Food(row);
                list.Add(food);
            }
            return list;
        }

        public List<Food> SearchFoodByName(string name)
        {
            List<Food> list = new List<Food>();

            string query = "select * from Food where name like N'%" + name +"%'";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow row in data.Rows)
            {
                Food food = new Food(row);
                list.Add(food);
            }
            return list;
        }

        public DataTable GetListFood()
        {
            List<Food> list = new List<Food>();

            string query = "select * from Food";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            
            return data;
        }

        public bool InsertFood(string name, int id, float price)
        {
            string query = string.Format("insert into food(name,idCategory,price) values (N'{0}',{1},{2})", name, id, price);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool UpdateFood(int idFood,string name, int id, float price)
        {
            string query = string.Format("Update food set name = N'{0}', idCategory = {1}, price = {2} where id = {3}", name, id, price,idFood);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool DeleteFood(int id)
        {
            BillInfoDAO.Instance.DeleteBillInfoByIDFood(id);
            string query = string.Format("delete food where id = {0}",id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }


    }
}
