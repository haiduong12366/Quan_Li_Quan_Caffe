using FormQuanLy.DAO;
using FormQuanLy.DTO;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FormQuanLy
{
    public partial class fAdmin : Form
    {
        BindingSource foodList = new BindingSource();
        BindingSource accountList = new BindingSource();
        public Account loginAccount;
        public fAdmin()
        {
            InitializeComponent();
            Load();
        }

        List<Food> searchFoodByName(string name)
        {
            List<Food> listFood = FoodDAO.Instance.SearchFoodByName(name);

            return listFood;
        }
        void Load()
        {
            dtgvFood.DataSource = foodList;
            dtgvAccount.DataSource = accountList;
            //LoadAccountList();
            LoadDateTimePickerBill();
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            LoadListFood();
            LoadAccount();
            AddFoodBinding();
            LoadCategoryIntoCb();
            LoadAccountIntoCb();
            AddAccountBinding();
        }
        #region
        void LoadAccountList()
        {
            string query = "execute USP_GetAccountByUserName @username ";

            dtgvAccount.DataSource = DataProvider.Instance.ExecuteQuery(query, new object[] { "k9" });

        }
        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }
        void LoadCategoryIntoCb()
        {
            cbFoodCategory.DataSource = CategoryDAO.Instance.GetListCategory();
            cbFoodCategory.DisplayMember = "Name";
        }
        void LoadAccountIntoCb()
        {
            cbAccountType.DataSource = AccountDAO.Instance.cbAccount();
            cbAccountType.DisplayMember = "Type";
        }
        public void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDateAndPage(dtpkFromDate.Value, dtpkToDate.Value, 1);
            txbPageBill.Text = "1";
        }
        void LoadAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
        }
        
        void LoadListFood()
        {
            foodList.DataSource = FoodDAO.Instance.GetListFood();
        }

        void AddFoodBinding()
        {
            txbFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txbFoodID.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
            nmPriceFood.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }
        void AddAccountBinding()
        {
            txbUsernameAccount.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "UserName"));
            txbDisplayAccount.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "DisplayName"));
        }
        void AddAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.InsertAccount(userName, displayName, type))
            {
                MessageBox.Show("Thêm tài khoản thành công");
                LoadAccount();
            }
            else
                MessageBox.Show("Thêm tài khoản thất bại");
        }
        void UpdateAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.UpdateAccount(userName, displayName, type))
            {
                MessageBox.Show("Sửa tài khoản thành công");
                LoadAccount();
            }
            else
                MessageBox.Show("Sửa tài khoản thất bại");
        }
        void DeleteAccount(string userName)
        {
            if (loginAccount.UserName.Equals(userName))
            {
                MessageBox.Show("Vui lòng không xóa tài khoản của chính bạn");
                return;
            }
            if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa tài khoản thành công");
                LoadAccount();
            }
            else
                MessageBox.Show("Xóa tài khoản thất bại");
        }
        void ResetAccount(string userName)
        {
            if (AccountDAO.Instance.ResetAccount(userName))
            {
                MessageBox.Show("Đã tạo mk mới");
                LoadAccount();
            }
            else
                MessageBox.Show("Reset mk thất bại");
        }
        #endregion
        private void btnShowFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }

        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }

        private void txbFoodID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtgvFood.SelectedCells.Count > 0 && dtgvFood.SelectedCells[0].OwningRow.Cells["idCategory"].Value != null)
                {
                    int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["idCategory"].Value;

                    Category category = CategoryDAO.Instance.GetCategoryByID(id);


                    int index = -1;
                    int i = 0;
                    foreach (Category item in cbFoodCategory.Items)
                    {
                        if (item.ID == category.ID)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }
                    cbFoodCategory.SelectedIndex = index;
                }
            }
            catch { }
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmPriceFood.Value;
            if (FoodDAO.Instance.InsertFood(name, categoryID, price))
            {
                MessageBox.Show("Thêm món thành công");
                LoadListFood();
                if (insertFood != null)
                    insertFood(this, new EventArgs());
            }
            else
                MessageBox.Show("Lỗi thêm món thành công");
        }

        private void btnEditFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmPriceFood.Value;
            var id = Convert.ToInt32(txbFoodID.Text);
            if (FoodDAO.Instance.UpdateFood(id, name, categoryID, price))
            {
                MessageBox.Show("Sửa món thành công");
                LoadListFood();
                if (updateFood != null)
                    updateFood(this, new EventArgs());
            }
            else
                MessageBox.Show("Lỗi sửa món thành công");
        }

        private void btnDeleteFood_Click(object sender, EventArgs e)
        {

            var id = Convert.ToInt32(txbFoodID.Text);
            if (FoodDAO.Instance.DeleteFood(id))
            {
                MessageBox.Show("Xóa món thành công");
                LoadListFood();
                if (deleteFood != null)
                    deleteFood(this, new EventArgs());
            }
            else
                MessageBox.Show("Xóa sửa món thành công");
        }

        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            foodList.DataSource = searchFoodByName(txbFoodNameSearch.Text);
        }


        private void btnShowAccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }

        private void txbUsernameAccount_TextChanged(object sender, EventArgs e)
        {

            if (dtgvAccount.SelectedCells.Count > 0 && dtgvAccount.SelectedCells[0].OwningRow.Cells["Username"].Value != null)
            {
                string username = dtgvAccount.SelectedCells[0].OwningRow.Cells["Username"].Value.ToString();


                //int type = (int)dtgvAccount.CurrentRow.Cells["Type"].Value;
                int type = (int)dtgvAccount.SelectedCells[0].OwningRow.Cells["Type"].Value;
                int index = -1;
                int i = 0;
                //MessageBox.Show(string.Format("{0}", cbAccountType.Items.Count));

                foreach (Account item in cbAccountType.Items)
                {
                    if (item.Type == type)
                    {
                        index = i;
                        break;
                    }
                    i++;
                }
                cbAccountType.SelectedIndex = index;

            }


        }

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            try {
                string userName = txbUsernameAccount.Text;
                string displayName = txbDisplayAccount.Text;
                int type = Convert.ToInt32(cbAccountType.Text);
                AddAccount(userName, displayName, type);
            }
            catch
            {
                MessageBox.Show("UserName đã được sử dụng");
            }
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {

            string username = txbUsernameAccount.Text;

            DeleteAccount(username);
        }

        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUsernameAccount.Text;
            string displayName = txbDisplayAccount.Text;
            int type = Convert.ToInt32(cbAccountType.Text);
            UpdateAccount(userName, displayName, type);
        }

        private void btnResetPassWord_Click(object sender, EventArgs e)
        {
            string username = txbUsernameAccount.Text;

            ResetAccount(username);
        }

        private void btnFirstBillPage_Click(object sender, EventArgs e)
        {
            txbPageBill.Text = "1";
        }

        private void btnLatBillPage_Click(object sender, EventArgs e)
        {
            int sumRecord = BillDAO.Instance.GetNumBillByDateAndPage(dtpkFromDate.Value, dtpkToDate.Value);
            int pageLast = sumRecord / 3;
            if (sumRecord % 3 != 0)
                pageLast++;
            txbPageBill.Text = pageLast.ToString();
        }

        private void txbPageBill_TextChanged(object sender, EventArgs e)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDateAndPage(dtpkFromDate.Value, dtpkToDate.Value, Convert.ToInt32(txbPageBill.Text));
        }

        private void btnPreviousBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txbPageBill.Text);

            if (page > 1)
                page--;
            txbPageBill.Text = page.ToString();


            
        }

        private void btnNextBillPage_Click(object sender, EventArgs e)
        {
            int sumRecord = BillDAO.Instance.GetNumBillByDateAndPage(dtpkFromDate.Value, dtpkToDate.Value);
            int pageLast = sumRecord / 3;



            int page = Convert.ToInt32(txbPageBill.Text);
            if (sumRecord % 3 != 0)
                pageLast++;
            if (page < pageLast)
                page++;
            txbPageBill.Text = page.ToString();



        }

        private void fAdmin_Load(object sender, EventArgs e)
        {

        }

    }
}
