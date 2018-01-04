using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_v1
{
    public partial class Configure : Form
    {
        public Configure()
        {
            InitializeComponent();
            //string[] serverNum = new string[] { "1,2,3,4,5" };
            comboBox2.Items.Add(1);
            comboBox2.Items.Add(2);
            comboBox2.Items.Add(3);
            comboBox2.Items.Add(4);
            this.comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;

            // string[] encryptions = new string[]{"AES-128,AES-192,AES-256,3-DES"};
            comboBox1.Items.Add("AES-128 Bit");
            comboBox1.Items.Add("AES-192 Bit");
            comboBox1.Items.Add("AES-256 Bit");
            comboBox1.Items.Add("Triple-DES");
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {



        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Home hF = new Home();
            hF.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label2.Text = "Settings saved";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
