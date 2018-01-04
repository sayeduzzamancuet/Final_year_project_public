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
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 form1 = new Form1();
            form1.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Add_File addfile = new Add_File();
            addfile.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            FileView fv = new FileView();
            fv.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Configure cF = new Configure();
            cF.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            KeyGen kG = new KeyGen();
            kG.Show();
        }
    }
}
