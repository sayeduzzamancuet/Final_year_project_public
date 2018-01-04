using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Core;
using MongoDB.Bson.IO;
using MongoDB.Driver.Linq;
using System.Diagnostics;

namespace Project_v1
{
    class View
    {
        public static async Task ViewFiles()
        {
            IMongoClient client = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase database = client.GetDatabase("mymongodb");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("UserDetails");
            BsonDocument filter = new BsonDocument();
            filter.Add("UserID","eliaszaman");
            var cursor = await collection.FindAsync(x => x.UserNam);
            while (await cursor.MoveNextAsync())
            {
                var listOfUsers = cursor.Current;
            }  
        }
    }
}
