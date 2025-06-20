using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Author { get; set; }

        [Required]
        public string? ISBN { get; set; }

        [Display(Name = "Published Year")]
        [Range(1000, 3000)]
        public int PublishedYear { get; set; }

        [Display(Name = "Available Copies")]
        [Range(0, int.MaxValue)]
        public int AvailableCopies { get; set; }
    }
} 