namespace BlazorGoldenZebra.Models.Common
{
    public class ColumnDefinition
    {
        public string PropertyName { get; set; }
        public string Title { get; set; }
        public bool IsVisible { get; set; } = true;
        public bool IsSortable { get; set; } = true;

        public string Format { get; set; }
    }
}
