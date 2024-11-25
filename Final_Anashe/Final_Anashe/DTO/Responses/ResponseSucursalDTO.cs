namespace Final_Anashe.DTO.Responses
{
    public class ResponseSucursalDTO
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string IdCiudad { get; set; }
        public Guid IdTipo { get; set; }
        public Guid IdProvincia { get; set; }
        public string Telefono { get; set; }
        public string NombreTitular { get; set; }
        public string ApellidoTitular { get; set; }
        public DateTime FechaAlta { get; set; }
    }
}
