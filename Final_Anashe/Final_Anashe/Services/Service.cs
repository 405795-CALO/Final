using AutoMapper;
using Final_Anashe.ApiResponse;
using Final_Anashe.DTO.Queries;
using Final_Anashe.DTO.Responses;
using Final_Anashe.Modelos;
using Final_Anashe.Repositories;
using System.Net;

namespace Final_Anashe.Services
{
    public class Service : IService
    {
        private readonly IRepository repository;
        private readonly IMapper _mapper;

        public Service(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this._mapper = mapper;
        }

        public async Task<ApiResponse<List<ResponseConfiguracionDTO>>> GetConfiguraciones()
        {
            try
            {
                var configuraciones = await repository.GetAllConfiguraciones();
                List<ResponseConfiguracionDTO> configuracionDTOs = new List<ResponseConfiguracionDTO>();
                foreach (var config in configuraciones)
                {
                    var configToAdd = _mapper.Map<ResponseConfiguracionDTO>(config);
                    configuracionDTOs.Add(configToAdd);
                }

                return new ApiResponse<List<ResponseConfiguracionDTO>>
                {
                    Data = configuracionDTOs
                };
            }
            catch (Exception ex) 
            {
                var errorResponse = new ApiResponse<List<ResponseConfiguracionDTO>>();
                errorResponse.SetError(ex.Message, HttpStatusCode.InternalServerError);
                return errorResponse;
            }
        }

        public async Task<ApiResponse<ResponseSucursalDTO>> GetSucursalesById(Guid id)
        {
            try
            {
                var sucursal = await repository.GetSucursalById(id);
                if (sucursal == null)
                {
                    var errorResponse = new ApiResponse<ResponseSucursalDTO>();
                    errorResponse.SetError("Sucursal no encontrada", HttpStatusCode.NotFound);
                    return errorResponse;
                }

                var responseSucursalDTO = _mapper.Map<ResponseSucursalDTO>(sucursal);

                return new ApiResponse<ResponseSucursalDTO>
                {
                    Data = responseSucursalDTO
                };
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<ResponseSucursalDTO>();
                errorResponse.SetError(ex.Message, HttpStatusCode.InternalServerError);
                return errorResponse;
            }
        }

        public async Task<ApiResponse<ResponseSucursalDTO>> GetSucursalesByNotBsAsAndDate()
        {
            try
            {
                var sucursal = await repository.GetSucursalByNotBsAsAndDate();
                ResponseSucursalDTO responseSucursalDTO = new ResponseSucursalDTO();
                responseSucursalDTO = _mapper.Map<ResponseSucursalDTO>(sucursal);

                return new ApiResponse<ResponseSucursalDTO>
                {
                    Data = responseSucursalDTO
                };
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<ResponseSucursalDTO>();
                errorResponse.SetError(ex.Message, HttpStatusCode.InternalServerError);
                return errorResponse;
            }
        }

        public async Task<ApiResponse<Sucursal>> PutSucursal(Guid id, PutSucursalDTO putSucursal)
        {
            try
            {
                // Buscar la sucursal existente
                var sucursal = await repository.GetSucursalById(id);
                if (sucursal == null)
                {
                    return new ApiResponse<Sucursal>
                    {
                        Success = false,
                        ErrorMessage = "Sucursal no encontrada",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                // Actualizar los campos necesarios
                _mapper.Map(putSucursal, sucursal);

                // Llamar al repositorio para guardar los cambios
                await repository.PutSucursal(sucursal);

                return new ApiResponse<Sucursal>
                {
                    Data = sucursal
                };
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<Sucursal>();
                errorResponse.SetError(ex.Message, HttpStatusCode.InternalServerError);
                return errorResponse;
            }
        }
    }
}
