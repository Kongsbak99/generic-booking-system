namespace Api.DTO
{
    public class BookableItem
    {
        public long Id { get; set; } //The Id property functions as the unique key in a relational database.
        public string? Name { get; set; }
        public string? Describtion { get; set; }
        public string? Location { get; set; }
    }
}