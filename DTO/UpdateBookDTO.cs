﻿namespace StudentManagmentSystem.DTO
{
    public class UpdateBookDTO
    {
        public string Title { get; set; }
        public string ISBN { get; set; }
        public int PublishYear { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId {  get; set; }    
    }
}
