using Entidades.SQLServer;
using Microsoft.AspNetCore.Mvc;
using Negocio.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/TipoHabitacion")]
    public class TipoHabitacionController : Controller
    {
        #region Atributos
        private readonly ITipoHabitacionLN _iTipoHabitacionLN;
        #endregion

        #region Constructor
        public TipoHabitacionController(ITipoHabitacionLN iTipoHabitacionLN)
        {
            _iTipoHabitacionLN = iTipoHabitacionLN;
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }

        #region Métodos Públicos
        [HttpPost]
        [Route(nameof(Crear))]
        public TipoHabitacion Crear([FromBody] TipoHabitacion entidad)
        {
            return _iTipoHabitacionLN.Crear(entidad);
        }

        [HttpGet]
        [Route(nameof(Obtener))]
        public List<TipoHabitacion> Obtener([FromHeader] int? tipoHabitacionId = null)
        {
            return _iTipoHabitacionLN.Obtener(tipoHabitacionId);
        }

        [HttpPut]
        [Route(nameof(Actualizar))]
        public TipoHabitacion Actualizar([FromBody] TipoHabitacion entidad)
        {
            return _iTipoHabitacionLN.Actualizar(entidad);
        }

        [HttpDelete]
        [Route(nameof(Eliminar))]
        public bool Eliminar([FromHeader] int tipoHabitacionId)
        {
            return _iTipoHabitacionLN.Eliminar(tipoHabitacionId);
        }
        #endregion
    }
}