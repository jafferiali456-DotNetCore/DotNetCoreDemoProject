namespace ENI_Microservice.Entity
{
    public class Menu
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public string? Description { get; set; }
        public int ParentId { get;set; }
    }
}
