using HotelFE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelFE.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var conexion = new GestorConexion();
                var usuarios = await conexion.ObtenerUsuarios();
                return View(usuarios);
            }
            catch (Exception ex)
            {
                await RegistrarBitacora("Error Consultar Usuarios", $"Error al listar usuarios: {ex.Message}");
                return View(new List<UsuarioModel>());
            }
        }

        [Authorize(Roles = "1")]
        public IActionResult Crear()
        {
            return View(new UsuarioModel { Estado = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Crear(UsuarioModel usuario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    usuario.FechaRegistro = DateTime.Now;
                    var conexion = new GestorConexion();
                    bool resultado = await conexion.Agregar(usuario);

                    if (resultado)
                    {
                        await RegistrarBitacora("Crear Usuario", $"Usuario creado exitosamente: {usuario.NombreUsuario}");
                        return RedirectToAction(nameof(Index));
                    }

                    await RegistrarBitacora("Error Crear Usuario", $"No se pudo crear el usuario: {usuario.NombreUsuario}");
                    ModelState.AddModelError("", "No se pudo crear el usuario.");
                }
                catch (Exception ex)
                {
                    await RegistrarBitacora("Error Crear Usuario", $"Error al crear usuario {usuario.NombreUsuario}: {ex.Message}");
                    ModelState.AddModelError("", "Error al crear el usuario.");
                }
            }
            return View(usuario);
        }


        [Authorize(Roles = "1")]
        public async Task<IActionResult> Modificar(int id)
        {
            try
            {
                var conexion = new GestorConexion();
                var usuarios = await conexion.ConsultarUsuario(new UsuarioModel { UsuarioId = id });
                var usuario = usuarios.FirstOrDefault();
                if (usuario == null)
                {
                    return NotFound();
                }
                return View(usuario);
            }
            catch (Exception ex)
            {
                await RegistrarBitacora("Error Modificar Usuario", $"Error al obtener usuario {id}: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Modificar(UsuarioModel usuario)
        {
            if (string.IsNullOrEmpty(usuario.Clave))
            {
                ModelState.Remove("Clave");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var conexion = new GestorConexion();
                    bool resultado = await conexion.Modificar(usuario);

                    if (resultado)
                    {
                        await RegistrarBitacora("Modificar Usuario", $"Usuario {usuario.NombreUsuario} modificado exitosamente");
                        return RedirectToAction(nameof(Index));
                    }

                    await RegistrarBitacora("Error Modificar Usuario", $"No se pudo modificar el usuario {usuario.NombreUsuario}");
                    ModelState.AddModelError("", "No se pudo actualizar el usuario.");
                }
                catch (Exception ex)
                {
                    await RegistrarBitacora("Error Modificar Usuario", $"Error al modificar usuario {usuario.NombreUsuario}: {ex.Message}");
                    ModelState.AddModelError("", "Error al actualizar el usuario.");
                }
            }
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Eliminar(int usuarioId)
        {
            try
            {
                var conexion = new GestorConexion();
                bool resultado = await conexion.Eliminar(new UsuarioModel { UsuarioId = usuarioId });

                if (resultado)
                {
                    await RegistrarBitacora("Eliminar Usuario", $"Usuario eliminado exitosamente: {usuarioId}");
                    return RedirectToAction(nameof(Index));
                }

                await RegistrarBitacora("Error Eliminar Usuario", $"No se pudo eliminar el usuario: {usuarioId}");
                TempData["Error"] = "No se pudo eliminar el usuario.";
            }
            catch (Exception ex)
            {
                await RegistrarBitacora("Error Eliminar Usuario", $"Error al eliminar usuario {usuarioId}: {ex.Message}");
                TempData["Error"] = "Error al eliminar el usuario.";
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "1")]
        public async Task<IActionResult> VerPerfiles(string nombreUsuario)
        {
            try
            {
                var conexion = new GestorConexion();
                var perfiles = await conexion.ListarPerfilesUsuario(new UsuarioModel { NombreUsuario = nombreUsuario });
                return View(perfiles);
            }
            catch (Exception ex)
            {
                await RegistrarBitacora("Error Listar Perfiles", $"Error al obtener los perfiles del usuario {nombreUsuario}: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Autenticacion(UsuarioModel usuario)
        {
            try
            {
                var conexion = new GestorConexion();
                bool resultado = await conexion.Autenticacion(usuario);

                if (resultado)
                {
                    await RegistrarBitacora("Autenticación", $"Usuario {usuario.NombreUsuario} autenticado exitosamente");
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Nombre de usuario o contraseña incorrectos.");
                return View(usuario);
            }
            catch (Exception ex)
            {
                await RegistrarBitacora("Error Autenticación", $"Error al autenticar usuario {usuario.NombreUsuario}: {ex.Message}");
                ModelState.AddModelError("", "Error al autenticar el usuario.");
                return View(usuario);
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
                ModuloSistema = "Usuarios",
                Descripcion = descripcion
            });
        }
    }
}