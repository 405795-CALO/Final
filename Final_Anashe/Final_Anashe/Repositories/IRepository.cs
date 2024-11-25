using Final_Anashe.Modelos;

namespace Final_Anashe.Repositories
{
    public interface IRepository
    {
        Task<List<Configuracion>> GetAllConfiguraciones();
        Task<Sucursal> GetSucursalByNotBsAsAndDate();
        Task<Sucursal> GetSucursalById(Guid Id);
        Task<Sucursal> PutSucursal(Sucursal sucursal);
    }
}
