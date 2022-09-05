namespace Api.DTO
{
    public class TodoItem
    {
        public long Id { get; set; } //The Id property functions as the unique key in a relational database.
        public string? Name { get; set; }
        public bool IsComplete { get; set; }
    }
}