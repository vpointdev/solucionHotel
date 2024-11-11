using Entidades.SQLServer;
using Microsoft.AspNetCore.Mvc;
using Negocio.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        [Route(nameof(Crear))]
        public Habitacion Crear([FromBody] Habitacion habitacion)
        {
            return _iHabitacionLN.Crear(habitacion);
        }

        [HttpGet]
        [Route(nameof(Obtener))]
        public List<Habitacion> Obtener(
            [FromQuery] int? habitacionId = null,
            [FromQuery] string? numeroHabitacion = null,
            [FromQuery] int? tipoHabitacionId = null,
            [FromQuery] string? estado = null)
        {
            return _iHabitacionLN.Obtener(habitacionId, numeroHabitacion, tipoHabitacionId, estado);
        }

        [HttpGet]
        [Route(nameof(ObtenerTodos))]
        public List<Habitacion> ObtenerTodos()
        {
            return _iHabitacionLN.Obtener();
        }

        [HttpPut]
        [Route(nameof(Actualizar))]
        public Habitacion Actualizar([FromBody] Habitacion habitacion)
        {
            return _iHabitacionLN.Actualizar(habitacion);
        }

        [HttpDelete]
        [Route("Eliminar/{habitacionId}")]
        public bool Eliminar(int habitacionId)
        {
            return _iHabitacionLN.Eliminar(habitacionId);
        }
    }
}