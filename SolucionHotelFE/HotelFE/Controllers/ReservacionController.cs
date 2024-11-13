using HotelFE.Controllers;
using HotelFE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize]
public class ReservacionController : Controller
{
    [Authorize(Roles = "1")] // Admin
    public async Task<IActionResult> Index()
    {
        try
        {
            var conexion = new GestorConexion();
            var reservaciones = await conexion.ObtenerTodasReservaciones();
            var habitaciones = await conexion.ObtenerTodasHabitaciones();
            ViewBag.Habitaciones = habitaciones;
            return View(reservaciones);
        }
        catch (Exception ex)
        {
            await RegistrarBitacora("Error Consultar Reservaciones",
                $"Error al listar reservaciones: {ex.Message}");
            return View(new List<ReservacionModel>());
        }
    }

    [Authorize] // Both Admin and Client
    public async Task<IActionResult> MisReservaciones()
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var conexion = new GestorConexion();
            var reservaciones = await conexion.ConsultarReservacion(new ReservacionModel { UsuarioId = userId });
            var habitaciones = await conexion.ObtenerTodasHabitaciones();
            ViewBag.Habitaciones = habitaciones;
            return View(reservaciones);
        }
        catch (Exception ex)
        {
            await RegistrarBitacora("Error Consultar Mis Reservaciones",
                $"Error al listar reservaciones: {ex.Message}");
            return View(new List<ReservacionModel>());
        }
    }

    [HttpGet]
    public async Task<IActionResult> Buscar(string codigoReservacion)
    {
        try
        {
            var conexion = new GestorConexion();
            var reservaciones = await conexion.ConsultarReservacion(
                new ReservacionModel { CodigoReservacion = codigoReservacion });
            var habitaciones = await conexion.ObtenerTodasHabitaciones();
            ViewBag.Habitaciones = habitaciones;
            return View("Index", reservaciones);
        }
        catch (Exception ex)
        {
            await RegistrarBitacora("Error Buscar Reservación",
                $"Error al buscar reservación: {ex.Message}");
            return View("Index", new List<ReservacionModel>());
        }
    }

    [HttpGet]
    public async Task<IActionResult> Crear()
    {
        try
        {
            var userClaims = this.User; // Access claims directly
            var conexion = new GestorConexion();
            ViewBag.TiposHabitacion = await conexion.ObtenerTiposHabitacion();
            return View(new ReservacionModel());
        }
        catch (Exception ex)
        {
            await RegistrarBitacora("Error Cargar Crear Reservación",
                $"Error al cargar formulario: {ex.Message}");
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(ReservacionModel reservacion, [FromServices] ClaimsPrincipal user)
    {
        try
        {
            var conexion = new GestorConexion();


            var resultado = await conexion.AgregarReservacion(reservacion, int.Parse(user.Identity.Name));

            if (resultado)
            {
                await RegistrarBitacora("Crear Reservación",
                    $"Reservación creada para habitación {reservacion.HabitacionId}");
                TempData["Success"] = "Reservación creada exitosamente.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "No se pudo crear la reservación.";
            ViewBag.TiposHabitacion = await conexion.ObtenerTiposHabitacion();
            return View(reservacion);
        }
        catch (Exception ex)
        {
            await RegistrarBitacora("Error Crear Reservación",
                $"Error al crear reservación: {ex.Message}");
            TempData["Error"] = "Error al crear la reservación.";
            return View(reservacion);
        }
    }

    [HttpGet]
    [Authorize(Roles = "1")] // Admin only
    public async Task<IActionResult> Modificar(int id)
    {
        try
        {
            var conexion = new GestorConexion();

            var reservacion = (await conexion.ConsultarReservacion(new ReservacionModel { ReservacionId = id })).FirstOrDefault();

            if (reservacion == null)
            {
                TempData["Error"] = "No se encontró la reservación.";
                return RedirectToAction(nameof(Index));
            }

            conexion = new GestorConexion();

            ViewBag.TiposHabitacion = await conexion.ObtenerTiposHabitacion();
            return View(reservacion);
        }
        catch (Exception ex)
        {
            await RegistrarBitacora("Error Cargar Modificar Reservación", $"Error al cargar formulario de modificación: {ex.Message}");
            TempData["Error"] = "Error al cargar la reservación.";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "1")] // Admin only
    public async Task<IActionResult> Modificar(ReservacionModel reservacion)
    {
        try
        {
            var conexion = new GestorConexion();

            var resultado = await conexion.ModificarReservacion(reservacion);

            if (resultado)
            {
                await RegistrarBitacora("Modificar Reservación", $"Reservación {reservacion.ReservacionId} modificada");
                TempData["Success"] = "Reservación modificada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            conexion = new GestorConexion();

            TempData["Error"] = "No se pudo modificar la reservación.";
            ViewBag.TiposHabitacion = await conexion.ObtenerTiposHabitacion();
            return View(reservacion);
        }
        catch (Exception ex)
        {
            await RegistrarBitacora("Error Modificar Reservación", $"Error al modificar reservación: {ex.Message}");
            TempData["Error"] = "Error al modificar la reservación.";
            return View(reservacion);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "1")] // Admin only
    public async Task<IActionResult> Eliminar(int id)
    {
        try
        {
            var conexion = new GestorConexion();
            var resultado = await conexion.EliminarReservacion(id);

            if (resultado)
            {
                await RegistrarBitacora("Eliminar Reservación",
                    $"Reservación {id} eliminada");
                TempData["Success"] = "Reservación eliminada exitosamente.";
            }
            else
            {
                TempData["Error"] = "No se pudo eliminar la reservación.";
            }

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            await RegistrarBitacora("Error Eliminar Reservación",
                $"Error al eliminar reservación {id}: {ex.Message}");
            TempData["Error"] = "Error al eliminar la reservación.";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancelar(int id)
    {
        try
        {
            var conexion = new GestorConexion();
            var resultado = await conexion.CancelarReservacion(id);

            if (resultado)
            {
                await RegistrarBitacora("Cancelar Reservación",
                    $"Reservación {id} cancelada");
                TempData["Success"] = "Reservación cancelada exitosamente.";
            }
            else
            {
                TempData["Error"] = "No se pudo cancelar la reservación.";
            }

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            await RegistrarBitacora("Error Cancelar Reservación",
                $"Error al cancelar reservación {id}: {ex.Message}");
            TempData["Error"] = "Error al cancelar la reservación.";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpGet]
    public async Task<IActionResult> HabitacionesDisponibles(DateTime fechaInicio, DateTime fechaFin)
    {
        try
        {
            var conexion = new GestorConexion();
            var habitaciones = await conexion.ObtenerHabitacionesDisponibles(fechaInicio, fechaFin);
            return PartialView("_HabitacionesDisponibles", habitaciones);
        }
        catch (Exception ex)
        {
            await RegistrarBitacora("Error",
                $"Error al obtener habitaciones disponibles: {ex.Message}");
            return Json(new { error = "Error al obtener habitaciones disponibles" });
        }
    }

    private async Task RegistrarBitacora(string accion, string descripcion)
    {
        var usuarioActual = User.FindFirst(ClaimTypes.Name)?.Value ?? "Sistema";
        var conexion = new GestorConexion();
        await conexion.Agregar(new BitacoraModel
        {
            FechaRegistro = DateTime.Now,
            UsuarioRegistro = usuarioActual,
            AccesionRealizada = accion,
            ModuloSistema = "Reservaciones",
            Descripcion = descripcion
        });
    }
}