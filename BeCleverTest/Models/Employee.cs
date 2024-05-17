
using System.Text.Json.Serialization;

namespace BeCleverTest.Models;

public partial class Employee
{
    public int IdEmployee { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public int IdDepartments { get; set; }

    [JsonIgnore]
    public virtual Department? IdDepartmentsNavigation { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Register>? Registers { get; set; } = new List<Register>();
}
