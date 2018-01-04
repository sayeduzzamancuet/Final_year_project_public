using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Project_v1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string URL = "http://localhost/project/login.php?";
            WebClient webClient = new WebClient();
            NameValueCollection formData = new NameValueCollection();
            formData["Username"] = textBox1.Text;
            formData["Password"] = textBox2.Text;
            
            byte[] responseBytes = webClient.UploadValues(URL, "POST", formData);
            string responsefromserver = Encoding.UTF8.GetString(responseBytes);
            Console.WriteLine(responsefromserver);
            webClient.Dispose();
            
            
            if (responsefromserver == "true")
            {
                string userPath = @"C:\Users\eliaszaman\Desktop\test\install\user.x";
                string user = textBox1.Text;
                File.WriteAllText(userPath, user);

                this.Hide();
                Home home = new Home();
                home.Show();
            }
            //else MessageBox.Show("Enter correct password");
            label1.Text = "You have entered wrong credentials";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
