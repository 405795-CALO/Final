using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Anashe.Modelos
{
    [Table("provincias")]
    public class Provincia
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
    }
}
