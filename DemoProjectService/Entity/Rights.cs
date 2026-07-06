namespace DemoProjectService.Entity
{
    public class Rights
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public string? RoleName { get; set; }
        public string? MenuName { get; set; }
        public bool CanAdd { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        public bool CanView { get; set; }
        public bool CanSubmit { get; set; }
        public bool CanWFHistory { get; set; }
    }
}
