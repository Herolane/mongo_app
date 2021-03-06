﻿using Matrix.Core.FrameworkCore;
using Matrix.Core.MongoCore;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrix.Entities.MongoEntities
{
    [BsonIgnoreExtraElements]
    public class Client : MXMongoEntity
    {
        [BsonElement("cd")]
        [Required]
        public string Code { get; set; }

        [BsonElement("ct")]
        [RequiredDenormalizedRefernce(ErrorMessage = "Required")]
        public DenormalizedReference ClientType { get; set; }

        [BsonElement("ph")]
        [Required]
        public string PhoneNumber { get; set; }

        [BsonElement("ws")]
        public string Website { get; set; }

        [BsonElement("ad")]
        public string Address { get; set; }

        [BsonElement("cts")]
        public IList<Contact> Contacts { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Contact 
    {
        [BsonElement("id")]        
        public string Id { get; set; }

        [BsonElement("nm")]
        [Required]
        public string Name { get; set; }

        [BsonElement("ph")]
        [Required]
        public string PhoneNumber { get; set; }

        [BsonElement("em")]
        [Required]
        public string Email { get; set; }

    }
}
