using Entidades.SQLServer;
using Microsoft.AspNetCore.Mvc;
using Negocio.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/Reservacion")]
    public class ReservacionController : Controller
    {
        private readonly IReservacionLN _reservacionLN;

        public ReservacionController(IReservacionLN reservacionLN)
        {
            _reservacionLN = reservacionLN;
        }

        [HttpGet]
        [Route(nameof(ListarReservaciones))]
        public List<Reservacion> ListarReservaciones()
        {
            return _reservacionLN.ObtenerTodos();
        }

        [HttpGet]
        [Route(nameof(ObtenerPorUsuario))]
        public List<Reservacion> ObtenerPorUsuario([FromHeader] int pUsuarioId)
        {
            return _reservacionLN.ObtenerTodos()
                .Where(r => r.UsuarioId == pUsuarioId).ToList();
        }

        [HttpGet]
        [Route(nameof(ObtenerPorCodigo))]
        public List<Reservacion> ObtenerPorCodigo([FromHeader] string pCodigoReservacion)
        {
            return _reservacionLN.ObtenerTodos()
                .Where(r => r.CodigoReservacion == pCodigoReservacion).ToList();
        }

        [HttpGet]
        [Route(nameof(ConsultarReservacion))]
        public List<Reservacion> ConsultarReservacion(
            [FromHeader] string pCodigoReservacion = "",
            [FromHeader] int? pUsuarioId = null)
        {
            var reservaciones = _reservacionLN.ObtenerTodos();

            if (!string.IsNullOrEmpty(pCodigoReservacion))
            {
                reservaciones = reservaciones
                    .Where(r => r.CodigoReservacion == pCodigoReservacion).ToList();
            }

            if (pUsuarioId.HasValue)
            {
                reservaciones = reservaciones
                    .Where(r => r.UsuarioId == pUsuarioId.Value).ToList();
            }

            return reservaciones;
        }

        [HttpPost]
        [Route(nameof(AgregarReservacion))]
        public bool AgregarReservacion([FromBody] Reservacion pReservacion)
        {
            return _reservacionLN.Agregar(pReservacion);
        }

        [HttpPut]
        [Route(nameof(ModificarReservacion))]
        public bool ModificarReservacion([FromBody] Reservacion pReservacion)
        {
            return _reservacionLN.Modificar(pReservacion);
        }

        [HttpDelete]
        [Route(nameof(EliminarReservacion))]
        public bool EliminarReservacion([FromHeader] int pReservacionId)
        {
            return _reservacionLN.Eliminar(pReservacionId);
        }

        [HttpPost]
        [Route(nameof(CancelarReservacion))]
        public bool CancelarReservacion([FromHeader] int pReservacionId)
        {
            return _reservacionLN.CancelarReservacion(pReservacionId);
        }
    }
}