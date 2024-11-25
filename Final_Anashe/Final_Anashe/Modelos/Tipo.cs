using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Anashe.Modelos
{
    [Table("tipos")]
    public class Tipo
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
    }
}
