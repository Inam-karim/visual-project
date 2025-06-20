using System;

namespace LibraryManagementSystem.Models
{
    public class LoanViewModel
    {
        public string Id { get; set; }
        public string BookTitle { get; set; }
        public string MemberName { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
} 