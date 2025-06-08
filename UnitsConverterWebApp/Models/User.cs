namespace UnitsConverterWebApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;  

        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;

        public ICollection<HistoryEntry> HistoryEntries { get; set; } = new List<HistoryEntry>();

    }

}
