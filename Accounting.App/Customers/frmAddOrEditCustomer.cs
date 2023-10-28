using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ValidationComponents;
using Accounting.Datalayer;
using Accounting.Datalayer.Context;

namespace Accounting.App
{
    public partial class frmAddOrEditCustomer : Form
    {
        public int customerId = 0;
        UnitOfWork db = new UnitOfWork();
        public frmAddOrEditCustomer()
        {
            InitializeComponent();
        }

        private void btnSelectPhoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            if(openfile.ShowDialog() == DialogResult.OK)
            {
                pcCustomer.ImageLocation = openfile.FileName;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
           if (BaseValidator.IsFormValid(this.components))
            {
                string imageName = Guid.NewGuid().ToString() + Path.GetExtension(pcCustomer.ImageLocation);
                string path = Application.StartupPath + "/Images/";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                pcCustomer.Image.Save(path + imageName);
                Customers customers = new Customers()
                {
                    Address = txtAddressCustomer.Text,
                    Email = txtEmailCustomer.Text,
                    FullName = txtNameCustomer.Text,
                    Mobile = txtMobilCustomer.Text,
                    CustomerImage = imageName
                };
                if (customerId == 0)
                {
                    db.CustomerRepository.InsertCustomer(customers);
                }
                else
                {
                    customers.CustomerID = customerId;
                    db.CustomerRepository.UpdateCustomer(customers);
                }
                db.Save();
                DialogResult = DialogResult.OK;

            }
        }

        private void frmAddOrEditCustomer_Load(object sender, EventArgs e)
        {
            if (customerId != null)
            {
                this.Text = "Edit";
                btnSave.Text = "Edit";
                var customer = db.CustomerRepository.GetCustomerById(customerId);
                txtAddressCustomer.Text = customer.Address;
                txtEmailCustomer.Text = customer.Email;
                txtMobilCustomer.Text = customer.Mobile;
                txtNameCustomer.Text = customer.FullName;
                pcCustomer.ImageLocation = Application.StartupPath + "Images" + customer.CustomerImage;

            }
        }
    }
}
