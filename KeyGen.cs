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
namespace Project_v1
{
    public partial class KeyGen : Form
    {
        public KeyGen()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int keySize = Convert.ToInt32(textBox1.Text);
            string publicAndPrivateKey;
            string publicKey;
            string PubKeyPath = @"C:\Users\eliaszaman\Desktop\test\key\publickey.txt";
            string PriKeyPath=@"C:\Users\eliaszaman\Desktop\test\key\privatekey.txt";
            AsymmetricEncryption.GenerateKeys(keySize, out publicKey, out publicAndPrivateKey);
            textBox2.Text = publicAndPrivateKey;
            File.WriteAllText(PriKeyPath, publicAndPrivateKey);
            textBox3.Text = publicKey;
            File.WriteAllText(PubKeyPath, publicKey);
            //string text = "text for encryptionkkkkkkkkkkk";
           // string encrypted = AsymmetricEncryption.EncryptText(text, keySize, publicKey);
            //string decrypted = AsymmetricEncryption.DecryptText(encrypted, keySize, publicAndPrivateKey);
            //Console.WriteLine("Encrypted: {0}", encrypted);
            //MessageBox.Show(encrypted);
            //Console.WriteLine("Decrypted: {0}", decrypted);
            //MessageBox.Show(decrypted);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Home hF = new Home();
            hF.Show();
        }
    }
}
