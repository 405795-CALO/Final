using Final_Anashe.Datos;
using Final_Anashe.Modelos;
using Microsoft.EntityFrameworkCore;

namespace Final_Anashe.Repositories
{
    public class Repository : IRepository
    {
        private readonly ContextDb contextDb;

        public Repository(ContextDb contextDb)
        {
            this.contextDb = contextDb;
        }
        public async Task<List<Configuracion>> GetAllConfiguraciones()
        {
            try
            {
                return await contextDb.Configuraciones.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Hubo un error al buscar las configuraciones en el repositorio", ex);
            }
        }

        public async Task<Sucursal> GetSucursalById(Guid Id)
        {
            try
            {
                return await contextDb.Sucursales.FirstOrDefaultAsync(s => s.Id == Id);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("No se encuentra sucursal con ese Id", ex);
            }
        }

        public async Task<Sucursal> GetSucursalByNotBsAsAndDate()
        {
            try
            {
                var sucursal = await contextDb.Sucursales
                    .Include(s=> s.Tipo)
                    .Include(s=> s.Provincia)
                    .Where(s=> s.Provincia.Nombre != "Buenos Aires")
                    .OrderByDescending(s=> s.FechaAlta)
                    .FirstOrDefaultAsync();
                return sucursal;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ocurrió un error en el repositorio",ex);
            }
        }

        public async Task<Sucursal> PutSucursal(Sucursal sucursal)
        {
            try
            {
                contextDb.Sucursales.Update(sucursal);
                await contextDb.SaveChangesAsync();
                return sucursal;
            }
            catch (Exception ex) 
            {
                throw new InvalidOperationException("Error al guardar sucursal en el repositorio", ex);
            }
        }
    }
}
