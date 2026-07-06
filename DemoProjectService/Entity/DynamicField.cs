namespace DemoProjectService.Entity
{
    public class DynamicField
    {
        public int Id { get; set; }
        public string? Mode { get; set; }
        public string? FormCode { get; set; }
        public string? TableName { get; set; }
        public string? FormTableName { get; set; }
        public string? ColumnName { get; set; }
        public string? DropDownParamter { get; set; }
        public string? DataType { get; set; }
        public string? LabelName { get; set; }
        public bool IsVisible { get; set; }
        public string? JsonPayload { get; set; }
        public int RecModifiedBy { get; set; }
        public int RecAddBy { get; set; }
    }
}
