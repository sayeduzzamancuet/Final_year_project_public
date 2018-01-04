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
using System.Security;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Project_v1
{
    public partial class FileView : Form
    {
        public FileView()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var watch = System.Diagnostics.Stopwatch.StartNew();
            // the code that you want to measure comes here
            int i = 0;
            for (i = 0; i < 6; i++)
            {
                string myConsoleLocation = @"C:\Users\eliaszaman\Documents\Visual Studio 2012\Projects\LICT\ConsoleApplication2\ConsoleApplication2\bin\Debug\ConsoleApplication2.exe";
                string str = textBox1.Text;//The filename required to download
                string usr = File.ReadAllText(@"C:\Users\eliaszaman\Desktop\test\install\user.x");
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = myConsoleLocation;
                startInfo.Arguments = i + " " + usr + " " + str;
                process.StartInfo = startInfo;
                process.Start();
            }
            var elapsedDown = watch.ElapsedMilliseconds;
           // elapsedDown = elapsedDown * 10;
            label7.Text = elapsedDown.ToString();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            string username = @"C:\Users\eliaszaman\Desktop\test\install\user.x";
            
            string usr = File.ReadAllText(username);
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = @"C:\Users\eliaszaman\Documents\Visual Studio 2012\Projects\LICT\ConsoleViewFiles\ConsoleViewFiles\bin\Debug\ConsoleViewFiles.exe";
            startInfo.Arguments = 0 + " " + usr;
            process.StartInfo = startInfo;
            process.Start();
            string filePath = @"C:\Users\eliaszaman\Desktop\test\install\view.tmp";
            List<string> files = File.ReadAllLines(filePath).ToList();
            foreach (string i in files)
            {
                listBox1.Items.Add(i);
            }
            //File.Delete(@"C:\Users\eliaszaman\Desktop\test\install\view.tmp");
        }

        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int lCounter = 1;
            string outPath = "C://Users//eliaszaman//Desktop//test//"; // Substitute this with your Input Folder 
            string[] tmpFiles = Directory.GetFiles(outPath, "*txt");
            FileStream outputFile = null;
            string prevFileName = "";

            foreach (string tempFile in tmpFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(tempFile);
                string baseFileName = textBox1.Text;
                string extension = Path.GetExtension(fileName);

                if (!prevFileName.Equals(baseFileName))
                {
                    if (outputFile != null)
                    {
                        outputFile.Flush();
                        outputFile.Close();
                    }
                    outputFile = new FileStream(outPath + baseFileName + extension, FileMode.OpenOrCreate, FileAccess.Write);
                }

                int bytesRead = 0;


                //From here each file part is accessed

                byte[] buffer = new byte[1024];
                FileStream inputTempFile = new FileStream(tempFile, FileMode.OpenOrCreate, FileAccess.Read);

                while ((bytesRead = inputTempFile.Read(buffer, 0, 1024)) > 0)
                    outputFile.Write(buffer, 0, bytesRead);

                inputTempFile.Close();
                File.Delete(tempFile);
                prevFileName = baseFileName;
            }
            lCounter++;
            outputFile.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Home hF = new Home();
            hF.Show();
        }


        //Decryption method 

        public byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Padding = PaddingMode.PKCS7;
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 5000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }
            return decryptedBytes;
        }



        public void DecryptFile()
        {
            string fExtension = File.ReadAllText(@"C:\Users\eliaszaman\Desktop\test\install\extension.x");
            string outPath = @"C:\Users\eliaszaman\Desktop\test\";
            string filepath = outPath + textBox1.Text + ".txt";
            string encoded = File.ReadAllText(filepath);
            var utfEncoded = Convert.FromBase64String(encoded);
            File.WriteAllBytes(filepath, utfEncoded);

            string fileEncrypted = filepath;
            string password = textBox3.Text;

            byte[] bytesToBeDecrypted = File.ReadAllBytes(fileEncrypted);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytes);
            string file = filepath;
            File.WriteAllBytes(file, bytesDecrypted);
            FileInfo rF = new FileInfo(file);// changing the file type from txt to original file extension
            rF.MoveTo(Path.ChangeExtension(file, fExtension));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            DecryptFile();
            watch.Stop();
            var elapsedMs2 = watch.ElapsedMilliseconds;
            label5.Text = elapsedMs2.ToString();
        }
        string privateKey;
        private void button6_Click(object sender, EventArgs e)
        {

            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string privateKeyPath = dlg.FileName.ToString();
                privateKey = File.ReadAllText(privateKeyPath);

            }


            string decrypted = AsymmetricEncryption.DecryptText(textBox2.Text, 1024, privateKey);
            textBox3.Text = decrypted;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox2.Text = File.ReadAllText(@"C:\Users\eliaszaman\Desktop\test\key\encr.txt");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

        }
    }
}
