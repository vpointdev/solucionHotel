using Entidades.SQLServer;
using Microsoft.AspNetCore.Mvc;
using Negocio.Interfaces;

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
        public Habitacion ObtenerPorId([FromHeader] int pHabitacionId)
        {
            return _habitacionLN.ObtenerTodos()
                .FirstOrDefault(h => h.HabitacionId == pHabitacionId);
        }

        [HttpPost]
        [Route(nameof(AgregarHabitacion))]
        public bool AgregarHabitacion([FromBody] Habitacion pHabitacionId)
        {
            return _habitacionLN.Agregar(pHabitacionId);
        }

        [HttpPut]
        [Route(nameof(ModificarHabitacion))]
        public bool ModificarHabitacion([FromBody] Habitacion phabitacion)
        {
            return _habitacionLN.Modificar(phabitacion);
        }

        [HttpDelete]
        [Route(nameof(EliminarHabitacion))]
        public bool EliminarHabitacion([FromHeader] int pHabitacionId)
        {
            return _habitacionLN.Eliminar(pHabitacionId);
        }

        [HttpPut]
        [Route(nameof(CambiarEstado))]
        public bool CambiarEstado([FromHeader] int pHabitacionId, [FromHeader] string estado)
        {
            var habitacion = _habitacionLN.ObtenerTodos()
                .FirstOrDefault(h => h.HabitacionId == pHabitacionId);

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