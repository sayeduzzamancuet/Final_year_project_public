using System;
using System.Security;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Project_v1
{

    public partial class Add_File : Form
    {

        public Add_File()
        {
            InitializeComponent();
        }
        string filepath, pathFolder, extension;
        int sNumber;

        List<string> dirList = new List<string>();
        private void button1_Click(object sender, EventArgs e)
        {

            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {


                filepath = dlg.FileName.ToString();
                pathFolder = System.IO.Path.GetDirectoryName(dlg.FileName);
                extension = Path.GetExtension(filepath);
                FileInfo f = new FileInfo(filepath);
                f.MoveTo(Path.ChangeExtension(filepath, ".txt"));
                textBox1.Text = filepath;
                filepath = Path.ChangeExtension(filepath, "txt");


            }

            string msg = null;
            try
            {
                using (StreamReader streamReader = new StreamReader(filepath, Encoding.UTF8))
                {
                    msg = streamReader.ReadToEnd();
                }
                textBox3.Text = msg;
            }
            catch (Exception ex)
            {
                msg = null;
                //If no file in selected than it will not give error message
            }
        }



        //AES algorithm encryption function 



        public byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

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

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }


        //File encryption performed here 


        public void EncryptFile()
        {
            string file = filepath;
            string password = textBox2.Text;

            byte[] bytesToBeEncrypted = File.ReadAllBytes(file);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

            string fileEncrypted = filepath;

            File.WriteAllBytes(fileEncrypted, bytesEncrypted);
            textBox3.Text = null;
            byte[] encryptedText = File.ReadAllBytes(file);
            string ms = Convert.ToBase64String(encryptedText);
            textBox3.Text = ms;


            File.WriteAllText(fileEncrypted, ms);

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

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
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
            string encoded = File.ReadAllText(filepath);
            var utfEncoded = Convert.FromBase64String(encoded);
            File.WriteAllBytes(filepath, utfEncoded);

            string fileEncrypted = filepath;
            string password = textBox2.Text;

            byte[] bytesToBeDecrypted = File.ReadAllBytes(fileEncrypted);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

            string file = filepath;
            File.WriteAllBytes(file, bytesDecrypted);
            textBox3.Text = null;
            byte[] decryptedText = File.ReadAllBytes(file);
            string msg = null;
            textBox3.Text = null;
            try
            {
                using (StreamReader streamReader = new StreamReader(filepath, Encoding.UTF8))
                {
                    msg = streamReader.ReadToEnd();
                }
                textBox3.Text = msg;
            }
            catch (Exception ex)
            {
                msg = null;
                MessageBox.Show(ex.ToString());
            }

        }

        private void timer1_Tick(object sender, EventArgs e)//Unused
        {

        }
        private void button2_Click(object sender, EventArgs e)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();//Counting the encryption time
            EncryptFile();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            label3.Text = elapsedMs.ToString();
        }

        private void label1_Click(object sender, EventArgs e)//unused
        {

        }





        private void button4_Click(object sender, EventArgs e)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();//Counting decryption time
            DecryptFile();
            watch.Stop();
            var elapsedMss = watch.ElapsedMilliseconds;
            label3.Text = elapsedMss.ToString();
        }




        //Upload Button functionality

        string baseFileName, chunkName;

        private void button3_Click(object sender, EventArgs e)
        {
            var watchV = System.Diagnostics.Stopwatch.StartNew();
            List<string> serverList = new List<string>();
            serverList.Add("mongodb://localhost:27017");
            serverList.Add("mongodb://eliaszamanm:a22334455.@ds131511.mlab.com:31511/mymongodb");
            serverList.Add("mongodb://eliaszamanm:a22334455@ds119395.mlab.com:19395/mymongodb");
            serverList.Add("mongodb://eliaszamanm:a22334455@ds151355.mlab.com:51355/mymongodb");
            serverList.Add("mongodb://eliaszamanm:a22334455@ds255455.mlab.com:55455/mymongodb");
            serverList.Add("mongodb://eliaszamanm:a22334455@ds159845.mlab.com:59845/mymongodb");

            int count = 0;
            var fileCollection = serverList[0];
            int CN = 0;


            foreach (string server in serverList)//Run the loop for each server to insert data in a sequence
            {
                CN++;
                if (CN > sNumber)
                    break;
                else
                {
                    MongoClient client = new MongoClient(server);
                    var DB1 = client.GetDatabase("mymongodb");
                    var collection = DB1.GetCollection<BOStore>("store");
                    int x1 = count;
                    int x2 = sNumber;
                    if (dirList[count] == null)
                        break;
                    else
                    {
                        string Address = dirList[count]; //This value is for File Tracking server

                        string partAddress = Address;

                        chunkName = Path.GetFileName(partAddress);
                        if (count < sNumber)
                        {
                            count = count + 1;
                        }
                        else
                            break;
                        string fileID = chunkName;



                        MongoClient fileClient = new MongoClient(serverList[0]);
                        var fDB = fileClient.GetDatabase("FileTracker");
                        var fileC = fDB.GetCollection<FileTrack>("FileDetails");
                        var FileInfo = new FileTrack
                        {
                            UserID = File.ReadAllText(@"C:\Users\eliaszaman\Desktop\test\install\user.x"),
                            FileID = fileID,
                            ServerID = server,
                            FileKey = File.ReadAllText(@"C:\Users\eliaszaman\Desktop\test\key\encr.txt")
                        };


                        string fileData = File.ReadAllText(partAddress, Encoding.UTF8);
                        textBox3.Text = null;
                        textBox3.Text = fileData;
                        var document = new BOStore
                        {
                            UserID = File.ReadAllText(@"C:\Users\eliaszaman\Desktop\test\install\user.x"),
                            BaseFileName = baseFileName,
                            FileID = fileID,
                            FileData = fileData,
                            FileKey = File.ReadAllText(@"C:\Users\eliaszaman\Desktop\test\key\encr.txt"),
                            FileExtension = extension
                        };


                        collection.InsertManyAsync(new[] { document });
                        fileC.InsertManyAsync(new[] { FileInfo });
                        File.Delete(partAddress);//Delete the Chunk after upload finished
                    }
                }
            }

            watchV.Stop();
            var elapsedUpload = watchV.ElapsedMilliseconds;
            label5.Text = elapsedUpload.ToString();
            MessageBox.Show("Successfully uploaded to cloud");
            listBox1.Items.Clear();//Clear the list box that contains the chunk list
        }




        //Split method starts here

        private void button5_Click(object sender, EventArgs e)
        {
            string inputFile = filepath; // Substitute this with your Input File 
            FileStream fs = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
            int numberOfFiles = Convert.ToInt32(sNumber);
            int sizeOfEachFile = (int)Math.Ceiling((double)fs.Length / numberOfFiles);

            for (int i = 1; i <= numberOfFiles; i++)
            {
                baseFileName = Path.GetFileNameWithoutExtension(inputFile);
                string extension = Path.GetExtension(inputFile);
                FileStream outputFile = new FileStream(Path.GetDirectoryName(inputFile) + "\\" + baseFileName + "." + i.ToString().PadLeft(5, Convert.ToChar("0")) + extension + "." + "txt", FileMode.Create, FileAccess.Write);
                int bytesRead = 0;
                byte[] buffer = new byte[sizeOfEachFile];


                if ((bytesRead = fs.Read(buffer, 0, sizeOfEachFile)) > 0)
                {
                    outputFile.Write(buffer, 0, bytesRead);
                }
                outputFile.Close();
            }

            fs.Close();
            System.IO.File.Delete(filepath);
            listBox1.Items.Clear();
            dirList.Clear();
            string[] files = Directory.GetFiles(pathFolder);
            string[] dirs = Directory.GetDirectories(pathFolder);
            foreach (string file in files)
            {
                string dir = Path.GetFullPath(file);
                dirList.Add(dir);
                string f = Path.GetFileNameWithoutExtension(file);
                listBox1.Items.Add(Path.GetFileNameWithoutExtension(f));
                chunkName = Path.GetFileName(file);
            }
        }


        //Merge method starts here

        int lCounter = 1;
        public void button6_Click(object sender, EventArgs e)
        {
            string outPath = "C://Users//eliaszaman//Desktop//test//"; // Substitute this with your Input Folder 
            string[] tmpFiles = Directory.GetFiles(outPath, "*txt");
            FileStream outputFile = null;
            string prevFileName = "";

            foreach (string tempFile in tmpFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(tempFile);
                baseFileName = fileName.Substring(0, fileName.IndexOf(Convert.ToChar(".")));
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
            listBox1.Items.Clear();

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            Home home = new Home();
            home.Show();
        }


        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }


        string KeyPath;
        string PubKey;

        private void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg2 = new OpenFileDialog();
            if (dlg2.ShowDialog() == DialogResult.OK)
            {
                KeyPath = dlg2.FileName.ToString();
                PubKey = File.ReadAllText(KeyPath);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string enPath = @"C:\Users\eliaszaman\Desktop\test\key\encr.txt";
            string encrypted = AsymmetricEncryption.EncryptText(textBox2.Text, 1024, PubKey);
            MessageBox.Show("Encrypted");
            File.WriteAllText(enPath, encrypted);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string sPath = @"C:\Users\eliaszaman\Desktop\test\install\sN.x";
            string selected = this.comboBox1.GetItemText(this.comboBox1.SelectedItem);
            sNumber = Convert.ToInt32(selected);
            
        }


    }
}