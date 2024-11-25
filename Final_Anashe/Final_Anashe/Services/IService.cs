using Final_Anashe.ApiResponse;
using Final_Anashe.DTO.Queries;
using Final_Anashe.DTO.Responses;
using Final_Anashe.Modelos;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Final_Anashe.Services
{
    public interface IService
    {
        Task<ApiResponse<List<ResponseConfiguracionDTO>>> GetConfiguraciones();
        Task<ApiResponse<ResponseSucursalDTO>> GetSucursalesByNotBsAsAndDate();
        Task<ApiResponse<ResponseSucursalDTO>> GetSucursalesById(Guid id);
        Task<ApiResponse<Sucursal>> PutSucursal(Guid id, PutSucursalDTO putSucursal);
    }
}
