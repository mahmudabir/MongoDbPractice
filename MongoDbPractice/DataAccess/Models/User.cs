using MongoDB.Bson;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbPractice.DataAccess.Models
{
    internal class User
    {
        public ObjectId Id { get; set; }

        [Column("UserName")]
        public string Name { get; set; }

        public Customer Customer { get; set; }
    }
}
