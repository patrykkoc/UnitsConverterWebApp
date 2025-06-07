namespace UnitsConverterWebApp.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<Unit> Units { get; set; } = new List<Unit>();

    }
}
