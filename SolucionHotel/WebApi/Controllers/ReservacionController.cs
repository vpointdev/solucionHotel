using Entidades.SQLServer;
using Microsoft.AspNetCore.Mvc;
using Negocio.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/Reservacion")]
    [ApiController]
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

        #region Vista
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region Métodos Públicos
        [HttpPost]
        [Route("Crear")]
        public IActionResult Crear([FromBody] Reservacion entidad)
        {
            try
            {
                var resultado = _iReservacionLN.Crear(entidad);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("ProcesarPago")]
        public IActionResult ProcesarPago([FromBody] Reservacion entidad)
        {
            try
            {
                var resultado = _iReservacionLN.ProcesarPago(entidad.ReservacionId, entidad.UsuarioId);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Cancelar")]
        public IActionResult Cancelar([FromBody] Reservacion entidad)
        {
            try
            {
                var resultado = _iReservacionLN.Cancelar(entidad.ReservacionId, entidad.UsuarioId);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("ObtenerPorUsuario/{usuarioId}")]
        public IActionResult ObtenerPorUsuario(int usuarioId)
        {
            try
            {
                var resultado = _iReservacionLN.ObtenerPorUsuario(usuarioId);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("ObtenerDisponibles")]
        public IActionResult ObtenerDisponibles([FromQuery] DateTime fechaEntrada, [FromQuery] DateTime fechaSalida)
        {
            try
            {
                var resultado = _iReservacionLN.ObtenerDisponibles(fechaEntrada, fechaSalida);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}