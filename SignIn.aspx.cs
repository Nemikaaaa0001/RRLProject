﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;



namespace E_Cart_Online_Shopping
{
    public partial class SignIn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if(Request.Cookies["Username"]!=null && Request.Cookies["Upwd"] != null)
                {
                    txtUsername.Text = Request.Cookies["Username"].Value;
                    txtPass.Text = Request.Cookies["Upwd"].Value;
                    CheckBox1.Checked = true;

                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ECartOnlineShoppingDB"].ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("Select * from Users where Username=@username and Password=@pwd", con);
                cmd.Parameters.AddWithValue("@username",txtUsername.Text);
                cmd.Parameters.AddWithValue("@pwd", txtPass.Text);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if(dt.Rows.Count !=0)
                {
                    if(CheckBox1.Checked)
                    {
                        Response.Cookies["Username"].Value = txtUsername.Text;
                        Response.Cookies["Upwd"].Value = txtPass.Text;

                        Response.Cookies["Username"].Expires = DateTime.Now.AddDays(30);
                        Response.Cookies["Upwd"].Expires = DateTime.Now.AddDays(30);

                    }

                    else
                    {
                        Response.Cookies["Username"].Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies["Upwd"].Expires = DateTime.Now.AddDays(-1);

                    }

                    string Utype;
                    Utype = dt.Rows[0][5].ToString().Trim();

                    if(Utype=="User")
                    {
                        Session["Username"] = txtUsername.Text;
                        Response.Redirect("~/UserHome.aspx");

                    }
                    if (Utype == "Admin")
                    {
                        Session["Username"] = txtUsername.Text;
                        Response.Redirect("~/AdminHome.aspx");

                    }


                }

                // Response.Write("<script> alert('Login Sucessfully Done'); </script>");

                else
                {
                    lblError.Text = "Invalid Username and Password";
                }

                clr();
                con.Close();
                //lblMsg.Text = "Registration Succesfully Done";
               // lblMsg.ForeColor = System.Drawing.Color.Green;

            }

        }

        private void clr()
        {
            txtPass.Text = string.Empty;
            txtUsername.Text = string.Empty;
            txtUsername.Focus();

        }

        protected void LinkbtnForgotPass_Click(object sender, EventArgs e)
        {

        }
    }
}