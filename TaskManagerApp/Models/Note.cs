namespace TaskManagerAppNotes.Models
{
    public class Note
    {
        public int Id { get; set; }    // Primary Key
        public string Title { get; set; } // Title of the note
        public string Content { get; set; } // The content of the note
        public DateTime CreatedAt { get; set; }
    }
}
