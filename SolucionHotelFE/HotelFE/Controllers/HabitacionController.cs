﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HotelFE.Models;
using System.Security.Claims;

namespace HotelFE.Controllers
{
    [Authorize]
    public class HabitacionController : Controller
    {
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var conexion = new GestorConexion();

                var habitaciones = await conexion.ObtenerTodasHabitaciones();
                var tiposHabitacion = await conexion.ObtenerTiposHabitacion();

                ViewBag.TiposHabitacion = tiposHabitacion;

                return View(habitaciones);
            }
            catch (Exception ex)
            {
                await RegistrarBitacora("Error Consultar Habitaciones", $"Error al listar habitaciones: {ex.Message}");
                return View(new List<HabitacionModel>());
            }
        }

        [HttpGet]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Crear()
        {
            try
            {
                var conexion = new GestorConexion();
                ViewBag.TiposHabitacion = await conexion.ObtenerTiposHabitacion();

                return View(new HabitacionModel
                {
                    Activo = true,
                    Estado = "Disponible"
                });
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Crear(HabitacionModel habitacion)
        {
            try
            {
                var conexion = new GestorConexion();

                ViewBag.TiposHabitacion = await conexion.ObtenerTiposHabitacion();
                                
                if (ModelState.IsValid)
                {
                    bool resultado = await conexion.AgregarHabitacion(habitacion);
                    if (resultado)
                    {
                        await RegistrarBitacora("Crear Habitación",
                            $"Habitación {habitacion.NumeroHabitacion} creada exitosamente");
                        return RedirectToAction(nameof(Index));
                    }

                    ModelState.AddModelError("", "No se pudo crear la habitación");
                }

                return View(habitacion);
            }
            catch (Exception ex)
            {
                await RegistrarBitacora("Error al obtener habitación", $"Error al crear habitación: {ex.Message}");

                ModelState.AddModelError("", "Ocurrió un error al crear la habitación");
                return View(habitacion);
            }
        }

        [Authorize(Roles = "1")]
        public async Task<IActionResult> Modificar(int id)
        {
            try
            {
                var conexion = new GestorConexion();
                var habitacion = await conexion.ObtenerHabitacion(id);

                if (habitacion == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                return View(habitacion);
            }
            catch (Exception ex)
            {
                await RegistrarBitacora("Error al obtener habitación", $"Error al obtener habitación {id}: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Modificar(int id, HabitacionModel habitacion)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var conexion = new GestorConexion();
                    habitacion.HabitacionId = id; 
                    bool resultado = await conexion.ModificarHabitacion(habitacion);

                    if (resultado)
                    {
                        await RegistrarBitacora("Modificar Habitación", $"Habitación {habitacion.NumeroHabitacion} modificada exitosamente");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        await RegistrarBitacora("Error Modificar Habitación", $"No se pudo modificar la habitación {habitacion.NumeroHabitacion}");
                        ModelState.AddModelError("", "No se pudo actualizar la habitación.");
                    }
                }
                catch (Exception ex)
                {
                    await RegistrarBitacora("Error Modificar Habitación", $"Error al modificar habitación {habitacion.NumeroHabitacion}: {ex.Message}");
                    ModelState.AddModelError("", "No se pudo actualizar la habitación.");
                }
            }

            return View(habitacion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> CambiarEstado(int id, string estado)
        {
            try
            {
                var conexion = new GestorConexion();
                var habitacion = await conexion.ObtenerHabitacion(id);

                if (habitacion != null)
                {
                    habitacion.Estado = estado;
                    bool resultado = await conexion.ModificarHabitacion(habitacion);

                    if (resultado)
                    {
                        await RegistrarBitacora("Cambiar Estado Habitación",
                            $"Estado de habitación {habitacion.NumeroHabitacion} cambiado a {estado}");
                    }
                    else
                    {
                        await RegistrarBitacora("Error Cambiar Estado",
                            $"No se pudo cambiar el estado de la habitación {habitacion.NumeroHabitacion}");
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await RegistrarBitacora("Error Cambiar Estado",
                    $"Error al cambiar estado de habitación {id}: {ex.Message}");
                return RedirectToAction(nameof(Index));
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
                ModuloSistema = "Habitaciones",
                Descripcion = descripcion
            });
        }
    }
}