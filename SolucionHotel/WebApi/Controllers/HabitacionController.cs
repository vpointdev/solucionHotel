using Entidades.SQLServer;
using Microsoft.AspNetCore.Mvc;
using Negocio.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/Habitacion")]
    public class HabitacionController : Controller
    {
        #region Atributos
        private readonly IHabitacionLN _iHabitacionLN;
        #endregion

        #region Constructor
        public HabitacionController(IHabitacionLN iHabitacionLN)
        {
            _iHabitacionLN = iHabitacionLN;
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route(nameof(AgregarHabitacion))]
        public bool AgregarHabitacion([FromBody] Habitacion P_Entidad)
        {
            return _iHabitacionLN.Agregar(P_Entidad);
        }

        [HttpDelete]
        [Route(nameof(EliminarHabitacion))]
        public bool EliminarHabitacion([FromHeader] string pNumeroHabitacion)
        {
            return _iHabitacionLN.Eliminar(new Habitacion { NumeroHabitacion = pNumeroHabitacion });
        }

        [HttpPut]
        [Route(nameof(ModificarHabitacion))]
        public bool ModificarHabitacion([FromHeader] string pNumeroHabitacion, [FromBody] Habitacion P_Entidad)
        {
            return _iHabitacionLN.Modificar(new Habitacion
            {
                NumeroHabitacion = pNumeroHabitacion,
                TipoHabitacionId = P_Entidad.TipoHabitacionId,
                Piso = P_Entidad.Piso,
                Estado = P_Entidad.Estado,
                Observaciones = P_Entidad.Observaciones
            });
        }

        [HttpGet]
        [Route(nameof(ConsultarHabitacion))]
        public List<Habitacion> ConsultarHabitacion([FromHeader] string pNumeroHabitacion)
        {
            return _iHabitacionLN.Consultar(new Habitacion
            {
                NumeroHabitacion = string.IsNullOrEmpty(pNumeroHabitacion.Replace("''", string.Empty)) ? string.Empty : pNumeroHabitacion
            });
        }

        [HttpGet]
        [Route(nameof(ObtenerDisponibles))]
        public List<Habitacion> ObtenerDisponibles([FromHeader] DateTime pFechaEntrada, [FromHeader] DateTime pFechaSalida)
        {
            return _iHabitacionLN.ObtenerDisponibles(pFechaEntrada, pFechaSalida);
        }

        [HttpGet]
        [Route(nameof(ObtenerTiposHabitacion))]
        public List<TipoHabitacion> ObtenerTiposHabitacion()
        {
            return _iHabitacionLN.ObtenerTiposHabitacion();
        }

        [HttpGet]
        [Route(nameof(ConsultarOcupacion))]
        public List<Habitacion> ConsultarOcupacion([FromHeader] DateTime pFechaInicio, [FromHeader] DateTime pFechaFin)
        {
            return _iHabitacionLN.ConsultarOcupacion(pFechaInicio, pFechaFin);
        }

        [HttpGet]
        [Route(nameof(ObtenerIngresos))]
        public decimal ObtenerIngresos([FromHeader] DateTime pFechaInicio, [FromHeader] DateTime pFechaFin)
        {
            return _iHabitacionLN.ObtenerIngresos(pFechaInicio, pFechaFin);
        }
    }
}