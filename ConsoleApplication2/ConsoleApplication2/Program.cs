using System;
using System.Security;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Diagnostics;
namespace ConsoleApplication2
{
    class Program
    {
        static async Task NMain(int p,string q,string r)
        {

            List<string> serverList = new List<string>();
            serverList.Add("mongodb://localhost:27017");
            serverList.Add("mongodb://e");
            serverList.Add("mongodb://e");
            serverList.Add("mongodb://e");
            serverList.Add("mongodb://e");
            serverList.Add("mongodb://e");
            {
                Stopwatch stW = new Stopwatch();
                stW.Start();
               // var watch = System.Diagnostics.Stopwatch.StartNew();//Counting decryption time
              //  var elapsedMss = watch.ElapsedMilliseconds;
               
                int i = p;
                IMongoClient client = new MongoClient(serverList[i]);
                IMongoDatabase database = client.GetDatabase("mymongodb");
                IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("store");
                //string UserID = Console.ReadLine();
                //string BaseFileName = Console.ReadLine();
                string UserID = q;
                string BaseFileName = r;
                BsonDocument filter = new BsonDocument();
                filter.Add("UserID", UserID);
                filter.Add("BaseFileName", BaseFileName);

                using (var cursor = await collection.FindAsync(filter))
                {
                    while (await cursor.MoveNextAsync())
                    {
                        var batch = cursor.Current;
                        foreach (BsonDocument document in batch)
                        {

                            String FileID = document.GetElement("FileID").ToString();
                            var theString = FileID;
                            var aStringBuilder = new StringBuilder(theString);
                            aStringBuilder.Remove(0, 7);
                            FileID = aStringBuilder.ToString();
                            
                            
                            String FileData = document.GetElement("FileData").ToString();
                            var theString2 = FileData;
                            var aStringBuilder2 = new StringBuilder(theString2);
                            aStringBuilder2.Remove(0, 9);
                            FileData = aStringBuilder2.ToString();
                            
                            TextWriter tw = File.CreateText(@"C:\Users\eliaszaman\Desktop\test\"+FileID);
                            tw.Write(FileData,Encoding.UTF8);
                            tw.Close();


                            String FileKey = document.GetElement("FileKey").ToString();
                            var theString3 = FileKey;
                            var aStringBuilder3 = new StringBuilder(theString3);
                            aStringBuilder3.Remove(0, 8);
                            FileKey = aStringBuilder3.ToString();
                           
                            TextWriter tw2 = File.CreateText(@"C:\Users\eliaszaman\Desktop\test\key\encr.txt");
                            tw2.Write(FileKey, Encoding.UTF8);
                            tw2.Close();


                            String FileExtension = document.GetElement("FileExtension").ToString();
                            var theString4 = FileExtension;
                            var aStringBuilder4 = new StringBuilder(theString4);
                            aStringBuilder4.Remove(0, 14);
                            FileExtension = aStringBuilder4.ToString();
                            
                            TextWriter tw3 = File.CreateText(@"C:\Users\eliaszaman\Desktop\test\install\extension.x");
                            tw3.Write(FileExtension, Encoding.UTF8);
                            tw3.Close();
                        }
                    }
     

                }
                //watch.Stop();
               // string pTime = elapsedMss.ToString();
                stW.Stop();
           
                File.AppendAllText(@"C:\Users\eliaszaman\Desktop\test\install\time.txt", stW.ElapsedMilliseconds.ToString()+Environment.NewLine);
                stW.Reset();
            }
             
               
        }

        static void Main(string[] args)
        {
            
            int serverNo;
            string userName;
            string fileName;
            try
            {
               
                if (args.Length > 0)
                {
                    
                    serverNo = Convert.ToInt32(args[0]);
                    userName = args[1];
                    fileName = args[2];
                    NMain(serverNo, userName, fileName).Wait();
                   
                }
            }
            catch (AggregateException e) { 
            }
        }
    }
}
