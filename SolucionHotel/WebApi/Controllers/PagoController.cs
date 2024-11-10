using Entidades.SQLServer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Negocio.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/Pago")]
    public class PagoController : Controller
    {
        #region Atributos
        private readonly IPagoLN _iPagoLN;
        #endregion

        #region Constructor
        public PagoController(IPagoLN iPagoLN)
        {
            _iPagoLN = iPagoLN;
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route(nameof(AgregarPago))]
        public bool AgregarPago([FromBody] Pago P_Entidad)
        {
            return _iPagoLN.Agregar(P_Entidad);
        }

        [HttpPut]
        [Route(nameof(ModificarPago))]
        public bool ModificarPago([FromBody] Pago P_Entidad)
        {
            return _iPagoLN.Modificar(P_Entidad);
        }

        [HttpDelete]
        [Route(nameof(EliminarPago))]
        public bool EliminarPago([FromHeader] int pPagoId)
        {
            return _iPagoLN.Eliminar(new Pago { PagoId = pPagoId });
        }

        [HttpGet]
        [Route(nameof(ConsultarPago))]
        public List<Pago> ConsultarPago([FromHeader] int pPagoId)
        {
            return _iPagoLN.Consultar(new Pago { PagoId = pPagoId });
        }

        [HttpGet]
        [Route(nameof(ObtenerPorReservacion))]
        public List<Pago> ObtenerPorReservacion([FromHeader] string pCodigoReservacion)
        {
            return _iPagoLN.ObtenerPorReservacion(pCodigoReservacion);
        }

        [HttpGet]
        [Route(nameof(ObtenerTiposPago))]
        public List<TipoPago> ObtenerTiposPago()
        {
            return _iPagoLN.ObtenerTiposPago();
        }

        [HttpGet]
        [Route(nameof(ObtenerTotalPorReservacion))]
        public decimal ObtenerTotalPorReservacion([FromHeader] string pCodigoReservacion)
        {
            return _iPagoLN.ObtenerTotalPorReservacion(pCodigoReservacion);
        }
    }
}