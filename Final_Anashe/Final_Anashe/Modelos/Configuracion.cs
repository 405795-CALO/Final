using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Anashe.Modelos
{
    [Table("configuraciones")]
    public class Configuracion
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Valor { get; set; }
    }
}
