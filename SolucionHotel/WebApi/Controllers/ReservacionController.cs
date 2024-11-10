using Entidades.SQLServer;
using Microsoft.AspNetCore.Mvc;
using Negocio.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/Reservacion")]
    public class ReservacionController : Controller
    {
        #region Atributos
        private readonly IReservacionLN _iReservacionLN;
        #endregion

        #region Constructor
        public ReservacionController(IReservacionLN iReservacionLN)
        {
            _iReservacionLN = iReservacionLN;
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route(nameof(AgregarReservacion))]
        public bool AgregarReservacion([FromBody] Reservacion P_Entidad)
        {
            return _iReservacionLN.Agregar(P_Entidad);
        }

        [HttpDelete]
        [Route(nameof(EliminarReservacion))]
        public bool EliminarReservacion([FromHeader] string pCodigoReservacion)
        {
            return _iReservacionLN.Eliminar(new Reservacion { CodigoReservacion = pCodigoReservacion });
        }

        [HttpPut]
        [Route(nameof(ModificarReservacion))]
        public bool ModificarReservacion([FromHeader] string pCodigoReservacion, [FromBody] Reservacion P_Entidad)
        {
            return _iReservacionLN.Modificar(new Reservacion
            {
                CodigoReservacion = pCodigoReservacion,
                UsuarioId = P_Entidad.UsuarioId,
                HabitacionId = P_Entidad.HabitacionId,
                FechaEntrada = P_Entidad.FechaEntrada,
                FechaSalida = P_Entidad.FechaSalida,
                EstadoReservacionId = P_Entidad.EstadoReservacionId,
                PrecioTotal = P_Entidad.PrecioTotal,
                Observaciones = P_Entidad.Observaciones,
                UsuarioCreacionId = P_Entidad.UsuarioCreacionId,
                UsuarioModificacionId = P_Entidad.UsuarioModificacionId
            });
        }

        [HttpGet]
        [Route(nameof(ConsultarReservacion))]
        public List<Reservacion> ConsultarReservacion([FromHeader] string pCodigoReservacion)
        {
            return _iReservacionLN.Consultar(new Reservacion
            {
                CodigoReservacion = string.IsNullOrEmpty(pCodigoReservacion.Replace("''", string.Empty)) ? string.Empty : pCodigoReservacion
            });
        }

        [HttpPut]
        [Route(nameof(CancelarReservacion))]
        public bool CancelarReservacion([FromHeader] string pCodigoReservacion)
        {
            return _iReservacionLN.CancelarReservacion(pCodigoReservacion);
        }

        [HttpGet]
        [Route(nameof(ConsultarReservacionesPorUsuario))]
        public List<Reservacion> ConsultarReservacionesPorUsuario([FromHeader] int pUsuarioId)
        {
            return _iReservacionLN.ConsultarPorUsuario(pUsuarioId);
        }

        [HttpGet]
        [Route(nameof(ConsultarReservacionesPorFecha))]
        public List<Reservacion> ConsultarReservacionesPorFecha([FromHeader] DateTime pFechaInicio, [FromHeader] DateTime pFechaFin)
        {
            return _iReservacionLN.ConsultarPorFecha(pFechaInicio, pFechaFin);
        }

        [HttpGet]
        [Route(nameof(CalcularCargoCancelacion))]
        public decimal CalcularCargoCancelacion([FromHeader] string pCodigoReservacion)
        {
            return _iReservacionLN.CalcularCargoCancelacion(pCodigoReservacion);
        }

        [HttpPost]
        [Route(nameof(CompletarCheckIn))]
        public bool CompletarCheckIn([FromHeader] string pCodigoReservacion)
        {
            return _iReservacionLN.CompletarCheckIn(pCodigoReservacion);
        }

        [HttpPost]
        [Route(nameof(CompletarCheckOut))]
        public bool CompletarCheckOut([FromHeader] string pCodigoReservacion)
        {
            return _iReservacionLN.CompletarCheckOut(pCodigoReservacion);
        }
    }
}