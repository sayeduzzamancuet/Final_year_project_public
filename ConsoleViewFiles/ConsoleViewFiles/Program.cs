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

namespace ConsoleViewFiles
{
    class Program
    {
        static async Task NMain(int p, string q)
        {

            List<string> serverList = new List<string>();
            serverList.Add("mongodb://localhost:27017");
            serverList.Add("mongodb://e");
            serverList.Add("mongodb://e");
            string tmpPath = @"C://Users//eliaszaman//Desktop//test//install//view.tmp";
            int i = p;
            List<string> fileList = new List<string>();
            IMongoClient client = new MongoClient(serverList[i]);
            IMongoDatabase database = client.GetDatabase("mymongodb");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("store");
            string UserID = q;
            BsonDocument filter = new BsonDocument();
            filter.Add("UserID", UserID);

            using (var cursor = await collection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    foreach (BsonDocument document in batch)
                    {
                        string baseFileName = document.GetElement("BaseFileName").ToString();
                        var theString = baseFileName;
                        var aStringBuilder = new StringBuilder(theString);
                        aStringBuilder.Remove(0, 13);
                        baseFileName = aStringBuilder.ToString();
                        Console.WriteLine(baseFileName);
                        fileList.Add(baseFileName);
                    }
                }
                System.IO.File.WriteAllLines(tmpPath, fileList.ToList());
            }

        }
        static void Main(string[] args)
        {
            int serverNo = 0;
            string userName;
            serverNo = Convert.ToInt32(args[0]);
            userName = args[1];

            NMain(serverNo, userName).Wait();


        }
    }
}