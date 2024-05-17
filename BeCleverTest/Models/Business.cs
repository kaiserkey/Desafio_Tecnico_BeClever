using BeCleverTest.Models;

using System.Text.Json.Serialization;

namespace BeCleverTest;

public partial class Business
{
    public int IdBusiness { get; set; }

    public string LocationName { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Register>? Registers { get; set; } = new List<Register>();
}
