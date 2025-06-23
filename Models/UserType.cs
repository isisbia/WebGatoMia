using System.ComponentModel.DataAnnotations.Schema;
using WebGatoMia.Models;

[Table("tb_UserType")]
public class UserType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<User> Users { get; set; } = new HashSet<User>();

    public UserType()
    {
        // Opcional, pode manter ou remover se inicializou acima
        // Users = new HashSet<User>();
    }
}
