using Final_Anashe.Modelos;
using Microsoft.EntityFrameworkCore;

namespace Final_Anashe.Datos
{
    public class ContextDb : DbContext
    {
        public ContextDb(DbContextOptions<ContextDb> options) : base(options)
        {

        }
        public DbSet<Configuracion> Configuraciones { get; set; }
        public DbSet<Provincia> Provincias{ get; set; }
        public DbSet<Sucursal> Sucursales { get; set; }
        public DbSet<Tipo> Tipos { get; set; }
    }
}
