
namespace BeCleverTest.Models;

public partial class Register
{
    public int IdRegister { get; set; }

    public int? IdEmployee { get; set; }

    public DateTime? DateTime { get; set; }

    public string? RegisterType { get; set; } = null!;

    public int? IdBusiness { get; set; }

    public virtual Business? IdBusinessNavigation { get; set; } = null!;

    public virtual Employee? IdEmployeeNavigation { get; set; } = null!;
}
