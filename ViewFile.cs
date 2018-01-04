using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Project_v1
{
    

    
        class ViewFile
        {
            static async Task _Main()
            {

                IMongoClient client = new MongoClient("mongodb://localhost:27017/");
                IMongoDatabase database = client.GetDatabase("mymongodb");
                IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("store");

                BsonDocument filter = new BsonDocument();
                filter.Add("UserID", "eliaszaman");

                using (var cursor = await collection.FindAsync(filter))
                {
                    while (await cursor.MoveNextAsync())
                    {
                        var batch = cursor.Current;
                        foreach (BsonDocument document in batch)
                        {
                            //Console.WriteLine(document.GetElement("_id"));
                            Console.WriteLine(document.GetElement("FileID"));
                        }
                        Console.Read();
                    }

                }

            }

          //public  static void Main(string[] args)
          //  {
          //      _Main().Wait();

          //  }
        }
    }