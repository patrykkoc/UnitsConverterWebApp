using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace UnitsConverterWebApp.Models
{
    public class Unit
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        [ValidateNever]

        public Category Category { get; set; } = null!;
        public double MultiplierToBase { get; set; }
        public double OffsetToBase { get; set; } = 0;
        public bool IsBaseUnit { get; set; } = false;

        public double ToBase(double value) => value * MultiplierToBase + OffsetToBase;
        public double FromBase(double value) => (value - OffsetToBase) / MultiplierToBase;

    }
}
