using AutoMapper;
using Final_Anashe.ApiResponse;
using Final_Anashe.DTO.Queries;
using Final_Anashe.DTO.Responses;
using Final_Anashe.MappingProfile;
using Final_Anashe.Modelos;
using Final_Anashe.Repositories;
using Final_Anashe.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.InteropServices;

namespace Final_Anashe
{
    public class Class
    {

    }
}

// ----BACKEND----

// Instalar las 4 dependencias EntityFramework y Automapper

// 1° Modelos:
[Table("sucursales")] //En todas las clases, con atributos públicos
public class Sucursal
{
    public Guid Id { get; set; }
    public string Nombre { get; set; }
    public string IdCiudad { get; set; }
    public Guid IdTipo { get; set; }
    [ForeignKey("IdTipo")] public Tipo Tipo { get; set; }
    public Guid IdProvincia { get; set; }
    [ForeignKey("IdProvincia")] public Provincia Provincia { get; set; }
    public string Telefono { get; set; }
    public string NombreTitular { get; set; }
    public string ApellidoTitular { get; set; }
    public DateTime FechaAlta { get; set; }

}

// 2° Agregar en appsettings.json la cadena de conexion:
public class stringd {
"ConnectionStrings": {
    "DefaultConnection": "Data Source=DESKTOP-UQFFVI6\\SQLNASHE;Initial Catalog=FinalAnashe;Integrated Security=True;Trust Server Certificate=True"
  },
}

// 3° Datos - ContextDb:
public class ContextDb : DbContext
{
    public ContextDb(DbContextOptions<ContextDb> options) : base(options)
    {

    }
    public DbSet<Configuracion> Configuraciones { get; set; }
    public DbSet<Provincia> Provincias { get; set; }
    public DbSet<Sucursal> Sucursales { get; set; }
    public DbSet<Tipo> Tipos { get; set; }
}

/* 4° Program - Agregar el builder
 
        ////Database Context
        //builder.Services.AddDbContext<ContextDb>(options =>
        //{
        //    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        //});

        ////CORS
        //builder.Services.AddCors(options =>
        //{
        //    options.AddDefaultPolicy(policy =>
        //    {
        //        policy.AllowAnyOrigin()
        //              .AllowAnyHeader()
        //              .AllowAnyMethod();
        //    });
        //});

        ////Mapper
        //builder.Services.AddAutoMapper(typeof(MappingProfile));

        ////Repositories
        //builder.Services.AddScoped<IRepository, Repository>();

        ////Services
        //builder.Services.AddScoped<IService, Service>();

*/

/*  5° Iniciar la migración
    Abrir la consola del administrador de paquetes nuget y ejecutar los comandos:
    Add-Migration InitialCreate
    Update-Database 
*/

// 6° Interfaz Repositorio:
public interface IRepository 
{
    Task<List<Configuracion>> GetAllConfiguraciones();
    Task<Sucursal> GetSucursalByNotBsAsAndDate();
    Task<Sucursal> GetSucursalById(Guid Id);
    Task<Sucursal> PutSucursal(Sucursal sucursal);
}

// 7° Implementación Repositorio:
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
                .Include(s => s.Tipo)
                .Include(s => s.Provincia)
                .Where(s => s.Provincia.Nombre != "Buenos Aires")
                .OrderByDescending(s => s.FechaAlta)
                .FirstOrDefaultAsync();
            return sucursal;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Ocurrió un error en el repositorio", ex);
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

// 8° Interfaz Service:
public interface IService
{
    Task<ApiResponse<List<ResponseConfiguracionDTO>>> GetConfiguraciones();
    Task<ApiResponse<ResponseSucursalDTO>> GetSucursalesByNotBsAsAndDate();
    Task<ApiResponse<ResponseSucursalDTO>> GetSucursalesById(Guid id);
    Task<ApiResponse<Sucursal>> PutSucursal(Guid id, PutSucursalDTO putSucursal);
}

// 9° Implementación Service:
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

// 10° Mapping Profile:
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Sucursal, ResponseSucursalDTO>();
        CreateMap<ResponseSucursalDTO, Sucursal>();

        CreateMap<Sucursal, PutSucursalDTO>();
        CreateMap<PutSucursalDTO, Sucursal>();

        CreateMap<Configuracion, ResponseConfiguracionDTO>();
        CreateMap<ResponseConfiguracionDTO, Configuracion>();

    }
}

/* 11° DTO's:
    Son clases idénticas a los modelos, quitando los datos que no querés que devuelvan, como los ID y las ForeingKey
*/
// Queries
public class PutSucursalDTO
{
    public string Nombre { get; set; }
    public string IdCiudad { get; set; }
    public Guid IdTipo { get; set; }
    public Guid IdProvincia { get; set; }
    public string Telefono { get; set; }
    public string NombreTitular { get; set; }
    public string ApellidoTitular { get; set; }
    public DateTime FechaAlta { get; set; }
}

// Responses
public class ResponseConfiguracionDTO
{
    public Guid Id { get; set; }
    public string Nombre { get; set; }
    public string Valor { get; set; }
}
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

// 12° Api Response:
public class ApiResponse<T>
{
    public T Data { get; set; }
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
    public HttpStatusCode StatusCode { get; set; }

    public ApiResponse()
    {
        Success = true;
        ErrorMessage = "";
        StatusCode = HttpStatusCode.OK;
    }

    public void SetError(string mensaje, HttpStatusCode statusCode)
    {
        Success = false;
        ErrorMessage = mensaje;
        StatusCode = statusCode;
    }
}

// 13° Controller:
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

// ----FRONTEND----
// HTML
public class Front { 
< !DOCTYPE html >
< html lang = "en" >
< head >
    < meta charset = "UTF-8" >
    < meta name = "viewport" content = "width=device-width, initial-scale=1.0" >
    < title > Document</ title>
    < link href = "https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel = "stylesheet" integrity = "sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" crossorigin = "anonymous" >
    < script src = "https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js" integrity = "sha384-YvpcrYf0tY3lHB60NNkmXc5s9fDVZLESaAA55NDzOxhy9GkcIdslK1eN7N6jIeHz" crossorigin = "anonymous" ></ script >
    < script src = "https://ajax.googleapis.com/ajax/libs/jquery/3.7.1/jquery.min.js" ></ script >
    < script src = "https://cdn.jsdelivr.net/npm/jquery-validation@1.19.5/dist/jquery.validate.min.js" ></ script >
    < script src = "/index.js" ></ script >

</ head >
< body >
    < div class= "container" >
        < div class= "row" >
            < h1 class= "bg-primary text-white text-center mt-3 p-1" > Final Prog 3</h1>
        </div>
        <div class= "form-group" >
            < form id = "sucursalForm" >
                    < div class= "form-group" >                                                < !--borrar esto-- >
                        < label for= "nombre" > Guid</ label>                                    < !--borrar esto-- >
                        < input type = "text" class= "form-control" id = "guid" required disabled><!--borrar esto-->
                    </div>                                                                  <!--borrar esto-->  
                    <div class= "form-group" >
                        < label for= "nombre" > Nombre</ label>
                        < input type = "text" class= "form-control" id = "nombre" required >
                    </ div >
                    < div class= "form-group" >
                        < label for= "nombreCiudad" > Nombre de la Ciudad</ label >
                        < input type = "text" class= "form-control" id = "nombreCiudad" required >
                    </ div >
                    < div class= "form-group" >
                        < label for= "tipo" > Tipo</ label>
                        < select class= "form-control" id = "tipo" required >
                            < option value = "" disabled selected>-- Selecciona una Tipo --</option>
                            <option value = "e0660d87-3e12-4837-882b-d8927a265ac7" > Pequeña </ option >
                            < option value="776ff474-fa61-434c-941c-3a5426fbde1d">Grande</option>
                        </select>
                    </div>
                    <div class= "form-group" >
                        < label for= "provincia" > Provincia</ label>
                        < select class= "form-control" id = "provincia" required >
                            < option value = "" disabled selected>-- Selecciona una Provincia --</option>
                            <option value = "5deb1099-8449-4f26-b945-4ef1d8faa617" > Buenos Aires</option>
                            <option value = "3ce90c95-b9bc-4756-a86c-c36e0081f76d" > Córdoba </ option >
                            < option value="d7a74855-8a78-4db3-aadd-57513aae53b8">Salta</option>
                        </select>
                    </div>
                    <div class= "form-group" >
                        < label for= "telefono" > Teléfono</ label>
                        < input type = "text" class= "form-control" id = "telefono" required >
                    </ div >
                    < div class= "form-group" >
                        < label for= "nombreTitular" > Nombre Titular</ label >
                        < input type = "text" class= "form-control" id = "nombreTitular" required >
                    </ div >
                    < div class= "form-group" >
                        < label for= "apellidoTitular" > Apellido Titular</ label >
                        < input type = "text" class= "form-control" id = "apellidoTitular" required >
                    </ div >
                    < div class= "form-group" >
                        < label for= "fechaAlta" > Fecha de Alta</ label>
                        < input class= "form-control" id = "fechaAlta" type = "datetime" readonly disabled>
                    </ div >
                    < button type = "submit" class= "btn btn-primary mt-3" > Actualizar</ button>
                </ form >


        </ div >
    </ div >
</ body >
</ html >
}
// JS
/* async function cargarConfiguraciones() {
   try {
       const response = await fetch("https://localhost:7033/api/configuraciones");

       if (!response.ok) {
           throw new Error("Error al obtener las configuraciones");
       }

       // Procesa la respuesta como un array
       const configuraciones = await response.json(); 

       // Aplica las configuraciones al formulario directamente
       const formulario = document.getElementById("sucursalForm");
       configuraciones.forEach(config => {
           formulario.style[config.nombre] = config.valor; // Aplica las configuraciones como estilo CSS
       });

       console.log("Configuraciones aplicadas automáticamente.");
   } catch (error) {
       console.error("Error cargando configuraciones:", error);
   }
}

// Llama esta función al cargar la página
document.addEventListener("DOMContentLoaded", cargarConfiguraciones);

document.addEventListener("DOMContentLoaded", () => {
   const apiBaseUrl = "https://localhost:7033/api"; // Cambia esto a la URL base de tu API.

   // Cargar la sucursal al cargar la página
   fetch(`${apiBaseUrl}/sucursal-not-bsas`)
       .then((response) => {
           if (!response.ok) {
               throw new Error("Error al obtener la sucursal");
           }
           return response.json();
       })
       .then((sucursal) => {
           console.log(sucursal);
           const fechaAlta = new Date(sucursal.fechaAlta);
           console.log(fechaAlta);

           // Suponiendo que `data.fechaAlta` viene en formato ISO: "2024-11-24T13:00:00.000Z"
           const fechaAltaOriginal = sucursal.fechaAlta.split('T'); // Divide la fecha y la hora
           const fecha = fechaAltaOriginal[0]; // Obtén la parte de la fecha (YYYY-MM-DD)
           const hora = fechaAltaOriginal[1].slice(0, 8); // Obtén la hora (HH:mm)

           // Rellenar los inputs con los datos de la sucursal
           document.getElementById("guid").value = sucursal.id; //borrar esto
           document.getElementById("nombre").value = sucursal.nombre;
           document.getElementById("nombreCiudad").value = sucursal.idCiudad;
           document.getElementById("tipo").value = sucursal.idTipo;
           document.getElementById("provincia").value = sucursal.idProvincia;
           document.getElementById("telefono").value = sucursal.telefono;
           document.getElementById("nombreTitular").value = sucursal.nombreTitular;
           document.getElementById("apellidoTitular").value = sucursal.apellidoTitular;
           document.getElementById("fechaAlta").value = `${fecha}T${hora}`; // Asignar fecha y hora al input
           //new Date(sucursal.fechaAlta).toISOString().slice(0,19); // Formato yyyy-mm-dd
       })
       .catch((error) => {
           console.error("Error:", error);
           alert("Hubo un problema al cargar los datos de la sucursal.");
       });
});

document.addEventListener("DOMContentLoaded", function () {
   document.getElementById("sucursalForm").addEventListener("submit", async function (event) {
       event.preventDefault(); // Evita el envío del formulario por defecto

       // Obtén los datos del formulario
       const id = document.getElementById("guid").value; // Reemplaza con el ID correspondiente si es fijo
       const sucursal = {

           nombre: document.getElementById("nombre").value,
           idCiudad: document.getElementById("nombreCiudad").value,
           idTipo: document.getElementById("tipo").value,
           idProvincia: document.getElementById("provincia").value,
           telefono: document.getElementById("telefono").value,
           nombreTitular: document.getElementById("nombreTitular").value,
           apellidoTitular: document.getElementById("apellidoTitular").value,
           fechaAlta: document.getElementById("fechaAlta").value
       };

       console.log(JSON.stringify(sucursal));

       try {
           // Enviar el PUT al backend
           const response = await fetch(`https://localhost:7033/api/sucursal/${id}`, {
               method: "PUT",
               headers: {
                   "Content-Type": "application/json",
               },
               body: JSON.stringify(sucursal), // Convierte el objeto a JSON
           });

           // Verifica si la operación fue exitosa
           if (response.ok) {
               const result = await response.json();
               if (result.success) {
                   alert("Sucursal actualizada correctamente.");
               } else {
                   alert("Error al actualizar la sucursal: " + result.errorMessage);
               }
           } else {
               alert("Error en la solicitud: " + response.status + "-" + response.statusText);
           }
       } catch (error) {
           console.error("Error:", error);
           alert("Ocurrió un error inesperado al intentar actualizar la sucursal.");
       }
   })
});
*/


