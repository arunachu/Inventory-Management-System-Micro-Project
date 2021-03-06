﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Inventory_Management_System
{
    public partial class Products : Form
    {
        public Products()
        {
            InitializeComponent();
        }

        private void Products_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void Add_Button_Click(object sender, EventArgs e)
        {
            New_Item n = new New_Item(this);
            n.Show();
            LoadData();

        }

        private bool IfProductExists(SqlConnection con, string productCode)
        {
            SqlDataAdapter sda = new SqlDataAdapter("Select 1 From [Products] WHERE [ProductCode] = '" + productCode +"'", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
        private bool IfProductExists2(SqlConnection con, string productCode)
        {
            SqlDataAdapter sda = new SqlDataAdapter("Select 1 From [Products] WHERE [ProductCode] LIKE '" + productCode + "%'", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
        private bool IfProductExists3(SqlConnection con, string productname)
        {
            SqlDataAdapter sda = new SqlDataAdapter("Select 1 From [Products] WHERE [ProductName] LIKE '" + productname + "%'", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
        public void LoadData()
        {
            SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=D:\Users\Admin\Documents\Users.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True;");
            SqlDataAdapter sda = new SqlDataAdapter("Select * From [Products] ORDER BY [ProductCode]", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.Rows.Clear();
            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[0].Value = item["ProductCode"];
                dataGridView1.Rows[n].Cells[1].Value = item["ProductName"];
                dataGridView1.Rows[n].Cells[2].Value = item["ProductQuantity"];
                dataGridView1.Rows[n].Cells[3].Value = item["ProductRate"];
            }
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (dataGridView1.SelectedRows[0].Cells[0].Value != null && dataGridView1.SelectedRows[0].Cells[1].Value != null && dataGridView1.SelectedRows[0].Cells[2].Value != null && dataGridView1.SelectedRows[0].Cells[3].Value != null)
            {
                ProductCode_textbox.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                ProductName_textbox.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                ProductRate_textbox.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                ProductQuantity_textbox.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            }
        }

        private void Delete_Button_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=D:\Users\Admin\Documents\Users.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True;");
            if (IfProductExists(con, ProductCode_textbox.Text))
            {
                if (MessageBox.Show("Are you sure you want to Delete this?", "Delete Product", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"DELETE [Products] WHERE [ProductCode] = '" + ProductCode_textbox.Text + "'", con);
                    cmd.ExecuteNonQuery();
                    ProductCode_textbox.Clear();
                    ProductName_textbox.Clear();
                    ProductQuantity_textbox.Clear();
                    ProductRate_textbox.Clear();
                    con.Close();
                }
            }
            else
            {
                MessageBox.Show("Please Select a Product....!","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            LoadData();
        }

        private void Search_button_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=D:\Users\Admin\Documents\Users.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True;");
            if (!string.IsNullOrWhiteSpace(this.ProductCode_textbox.Text))
            {
                ProductName_textbox.Clear();
                if (IfProductExists2(con, ProductCode_textbox.Text))
                {
                    SqlDataAdapter sda = new SqlDataAdapter("Select * From [Products] WHERE [ProductCode] LIKE '%" + ProductCode_textbox.Text + "%' ORDER BY [ProductCode]", con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    dataGridView1.Rows.Clear();
                    foreach (DataRow item in dt.Rows)
                    {
                        int n = dataGridView1.Rows.Add();
                        dataGridView1.Rows[n].Cells[0].Value = item["ProductCode"];
                        dataGridView1.Rows[n].Cells[1].Value = item["ProductName"];
                        dataGridView1.Rows[n].Cells[2].Value = item["ProductQuantity"];
                        dataGridView1.Rows[n].Cells[3].Value = item["ProductRate"];
                    }

                }
                else
                {
                    MessageBox.Show("Not Found!!!!", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else if (!string.IsNullOrWhiteSpace(this.ProductName_textbox.Text))
            {
                if (IfProductExists3(con, ProductName_textbox.Text))
                {
                    SqlDataAdapter sda = new SqlDataAdapter("Select * From [Products] WHERE [ProductName] LIKE '" + ProductName_textbox.Text + "%' ORDER BY [ProductCode]", con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    dataGridView1.Rows.Clear();
                    foreach (DataRow item in dt.Rows)
                    {
                        int n = dataGridView1.Rows.Add();
                        dataGridView1.Rows[n].Cells[0].Value = item["ProductCode"];
                        dataGridView1.Rows[n].Cells[1].Value = item["ProductName"];
                        dataGridView1.Rows[n].Cells[2].Value = item["ProductQuantity"];
                        dataGridView1.Rows[n].Cells[3].Value = item["ProductRate"];
                    }

                }
                else
                {
                    MessageBox.Show("Not Found!!!!", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void Refresh_button_Click(object sender, EventArgs e)
        {
            ProductName_textbox.Clear();
            ProductCode_textbox.Clear();
            ProductQuantity_textbox.Clear();
            ProductRate_textbox.Clear();
            LoadData();
        }

        private void Edit_button_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=D:\Users\Admin\Documents\Users.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True;");
            if(IfProductExists(con,ProductCode_textbox.Text))
            {
                SqlDataAdapter sda = new SqlDataAdapter("Select * From [Products] WHERE [ProductCode] = '" + ProductCode_textbox.Text +"'", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow item in dt.Rows)
                {
                    ProductCode_textbox.Text = item["ProductCode"].ToString();
                    ProductName_textbox.Text = item["ProductName"].ToString();
                    ProductQuantity_textbox.Text = item["ProductQuantity"].ToString();
                    ProductRate_textbox.Text = item["ProductRate"].ToString();
                }
                Edit_Item edit = new Edit_Item(this,ProductCode_textbox.Text, ProductName_textbox.Text, ProductRate_textbox.Text, ProductQuantity_textbox.Text);
                edit.Show();

                LoadData();
            }
            else
            {
                MessageBox.Show("Please Select the Product to Edit", "No Product Selected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }




    }
}