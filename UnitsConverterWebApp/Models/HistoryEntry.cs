using System.ComponentModel.DataAnnotations;

namespace UnitsConverterWebApp.Models
{
    public class HistoryEntry
    {
        public int Id { get; set; }

        public int? FromUnitId { get; set; }
        public Unit? FromUnit { get; set; }

        public int? ToUnitId { get; set; }
        public Unit? ToUnit { get; set; }

        public double InputValue { get; set; }
        public double OutputValue { get; set; }
        
         
        public DateTime Time { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
 
