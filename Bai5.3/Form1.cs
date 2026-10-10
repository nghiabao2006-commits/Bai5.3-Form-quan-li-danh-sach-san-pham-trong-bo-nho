using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bai5._3
{
    public partial class Form1 : Form
    {
        private List<Product> products;
        private BindingSource bs;

        public Form1()
        {
            InitializeComponent();
            products = new List<Product>();
            bs = new BindingSource();
            bs.DataSource = products;
            dgvProducts.DataSource = bs;

            // Optional: preselect first category
            if (cbCategory.Items.Count > 0) cbCategory.SelectedIndex = 0;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var p = new Product
            {
                ProductId = txtProductId.Text.Trim(),
                ProductName = txtProductName.Text.Trim(),
                UnitPrice = numUnitPrice.Value,
                Quantity = (int)numQuantity.Value,
                Category = cbCategory.SelectedItem?.ToString() ?? string.Empty
            };

            if (string.IsNullOrEmpty(p.ProductId) || string.IsNullOrEmpty(p.ProductName))
            {
                MessageBox.Show("Mã SP và Tên SP không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            products.Add(p);
            bs.ResetBindings(false);
            ClearInputs();
        }

        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (bs.Current is Product p)
            {
                txtProductId.Text = p.ProductId;
                txtProductName.Text = p.ProductName;
                numUnitPrice.Value = p.UnitPrice;
                numQuantity.Value = p.Quantity;
                if (!string.IsNullOrEmpty(p.Category))
                {
                    int idx = cbCategory.Items.IndexOf(p.Category);
                    if (idx >= 0) cbCategory.SelectedIndex = idx;
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (bs.Current is Product p)
            {
                var res = MessageBox.Show($"Bạn có chắc muốn xóa sản phẩm '{p.ProductName}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    products.Remove(p);
                    bs.ResetBindings(false);
                    ClearInputs();
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (bs.Current is Product p)
            {
                p.ProductId = txtProductId.Text.Trim();
                p.ProductName = txtProductName.Text.Trim();
                p.UnitPrice = numUnitPrice.Value;
                p.Quantity = (int)numQuantity.Value;
                p.Category = cbCategory.SelectedItem?.ToString() ?? string.Empty;
                bs.ResetBindings(false);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string key = txtSearch.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(key))
            {
                bs.DataSource = products;
            }
            else
            {
                var filtered = products.Where(x => x.ProductName != null && x.ProductName.ToLower().Contains(key)).ToList();
                bs.DataSource = filtered;
            }
            bs.ResetBindings(false);
        }

        private void ClearInputs()
        {
            txtProductId.Clear();
            txtProductName.Clear();
            numUnitPrice.Value = 0;
            numQuantity.Value = 0;
            if (cbCategory.Items.Count > 0) cbCategory.SelectedIndex = 0;
        }
    }
}
