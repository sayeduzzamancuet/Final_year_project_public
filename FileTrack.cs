using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
namespace Project_v1
{
    class FileTrack
    {
        public ObjectId ID { get; set; }
        public string UserID { get; set; }
        public string FileID { get; set; }
        public string ServerID { get; set; }
        public string FileKey { get; set; }
    }
}