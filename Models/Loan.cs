using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    public class Loan
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        public string BookId { get; set; }

        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        public string MemberId { get; set; }

        [Required]
        public DateTime LoanDate { get; set; }

        public DateTime? ReturnDate { get; set; }
    }
} 