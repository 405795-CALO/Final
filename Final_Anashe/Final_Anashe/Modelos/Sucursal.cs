using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Anashe.Modelos
{
    [Table("sucursales")]
    public class Sucursal
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string IdCiudad { get; set; }
        public Guid IdTipo { get; set; }
        [ForeignKey("IdTipo")]public Tipo Tipo { get; set; }
        public Guid IdProvincia { get; set; }
        [ForeignKey("IdProvincia")]public Provincia Provincia { get; set; }
        public string Telefono { get; set; }
        public string NombreTitular { get; set; }
        public string ApellidoTitular { get; set; }
        public DateTime FechaAlta { get; set; }

    }
}
