using Microsoft.AspNetCore.Mvc;
using Negocio.Interfaces;
using Entidades.SQLServer;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/Habitacion")]
    public class HabitacionController : Controller
    {
        private readonly IHabitacionLN _habitacionLN;

        public HabitacionController(IHabitacionLN habitacionLN)
        {
            _habitacionLN = habitacionLN;
        }

        [HttpGet]
        [Route(nameof(ListarHabitaciones))]
        public List<Habitacion> ListarHabitaciones()
        {
            return _habitacionLN.ObtenerTodos();
        }

        [HttpGet]
        [Route(nameof(ObtenerPorId))]
        public Habitacion ObtenerPorId([FromHeader] int habitacionId)
        {
            return _habitacionLN.ObtenerTodos()
                .FirstOrDefault(h => h.HabitacionId == habitacionId);
        }

        [HttpPost]
        [Route(nameof(AgregarHabitacion))]
        public bool AgregarHabitacion([FromBody] Habitacion habitacion)
        {
            return _habitacionLN.Agregar(habitacion);
        }

        [HttpPut]
        [Route(nameof(ModificarHabitacion))]
        public bool ModificarHabitacion([FromHeader] int habitacionId, [FromBody] Habitacion habitacion)
        {
            habitacion.HabitacionId = habitacionId;
            return _habitacionLN.Modificar(habitacion);
        }

        [HttpDelete]
        [Route(nameof(EliminarHabitacion))]
        public bool EliminarHabitacion([FromHeader] int habitacionId)
        {
            return _habitacionLN.Eliminar(habitacionId);
        }

        [HttpPut]
        [Route(nameof(CambiarEstado))]
        public bool CambiarEstado([FromHeader] int habitacionId, [FromHeader] string estado)
        {
            var habitacion = _habitacionLN.ObtenerTodos()
                .FirstOrDefault(h => h.HabitacionId == habitacionId);

            if (habitacion != null)
            {
                habitacion.Estado = estado;
                return _habitacionLN.Modificar(habitacion);
            }
            return false;
        }

        [HttpGet]
        [Route(nameof(ListarTipos))]
        public List<TipoHabitacion> ListarTipos()
        {
            return _habitacionLN.ObtenerTiposHabitacion();
        }
    }
}