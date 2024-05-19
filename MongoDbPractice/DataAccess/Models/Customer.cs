using MongoDB.Bson;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbPractice.DataAccess.Models
{
    internal class Customer
    {
        public ObjectId Id { get; set; }

        public CustomerType Type { get; set; }



        public ObjectId UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }

    public enum CustomerType
    {
        Basic = 1,
        Special = 2,
        Banned = 3,
    }
}
