using Final_Anashe.DTO.Queries;
using Final_Anashe.DTO.Responses;
using Final_Anashe.Services;
using Microsoft.AspNetCore.Mvc;

namespace Final_Anashe.Controllers
{
    [Route("api")]
    [ApiController]
    public class Controller : ControllerBase
    {
        private readonly IService service;
        public Controller(IService service)
        {
            this.service = service;
        }

        [HttpGet("sucursal-not-bsas")]
        public async Task<ActionResult<ResponseSucursalDTO>> GetSucursalByNotBsAsAndDate()
        {
            var respuesta = await service.GetSucursalesByNotBsAsAndDate();

            if (respuesta.Success)
            {
                return Ok(respuesta.Data);
            }
            return NotFound(respuesta.ErrorMessage);
        }

        [HttpPut("sucursal/{id}")]
        public async Task<ActionResult<PutSucursalDTO>> PutSucursal(Guid id, [FromBody] PutSucursalDTO putSucursal)
        {
            var respuesta = await service.PutSucursal(id, putSucursal);

            if (respuesta.Success)
            {
                return Ok(respuesta);
            }
            return BadRequest(respuesta);
        }

        [HttpGet("configuraciones")]
        public async Task<ActionResult<ResponseConfiguracionDTO>> GetConfiguraciones()
        {
            var respuesta = await service.GetConfiguraciones();

            if (respuesta.Success)
            {   
                return Ok(respuesta.Data);
            }
            return BadRequest(respuesta.ErrorMessage);
        }

    }
}
