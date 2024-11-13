using HotelFE.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelFE.Controllers
{
    public class AutenticacionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AccesoDenegado()
        {
            return View();
        }

        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(UsuarioModel usuario)
        {
            var conexion = new GestorConexion();

            try
            {
                if (await conexion.Autenticacion(usuario))
                {
                    conexion = new GestorConexion();
                    var perfiles = await conexion.ListarPerfilesUsuario(new UsuarioModel
                    {
                        NombreUsuario = usuario.NombreUsuario
                    });

                    if (perfiles.Count > 0)
                    {
                        var claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.Name, usuario.UsuarioId.ToString()),

                            new Claim("Usuario", usuario.NombreUsuario)
                        };

                        foreach (PerfilModel perfil in perfiles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, perfil.PerfilId.ToString()));
                        }

                        var claimIdentidad = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimIdentidad));

                        await RegistrarBitacora("Inicio de Sesión",
                            $"Inicio de sesión exitoso para el usuario: {usuario.NombreUsuario}");

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        await RegistrarBitacora("Error Inicio de Sesión",
                            $"Usuario sin perfiles asignados: {usuario.NombreUsuario}");
                        ModelState.AddModelError("", "Usuario sin perfiles asignados.");
                    }
                }
                else
                {
                    await RegistrarBitacora("Error Inicio de Sesión",
                        $"Intento fallido de inicio de sesión para usuario: {usuario.NombreUsuario}");
                    ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
                }
            }
            catch (Exception ex)
            {
                await RegistrarBitacora("Error Inicio de Sesión",
                    $"Error durante el inicio de sesión para usuario {usuario.NombreUsuario}: {ex.Message}");
                ModelState.AddModelError("", "Ocurrió un error durante el inicio de sesión.");
            }

            return View("Index", usuario);
        }

        public async Task<IActionResult> CerrarSesion()
        {
            var usuario = User.FindFirst(ClaimTypes.Name)?.Value;
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!string.IsNullOrEmpty(usuario))
            {
                await RegistrarBitacora("Cierre de Sesión",
                    $"Cierre de sesión exitoso para el usuario: {usuario}");
            }

            return RedirectToAction("Index", "Autenticacion");
        }

        [HttpPost]
        public async Task<IActionResult> Registro(UsuarioModel usuario)
        {
            var conexion = new GestorConexion();

            try
            {
                usuario.FechaRegistro = DateTime.Now;
                usuario.Estado = true;

                bool resultado = await conexion.Agregar(usuario);

                if (resultado)
                {
                    await RegistrarBitacora("Registro Usuario",
                        $"Registro exitoso de nuevo usuario: {usuario.NombreUsuario}");
                    return RedirectToAction("Index");
                }
                else
                {
                    await RegistrarBitacora("Error Registro Usuario",
                        $"Intento fallido de registro para usuario: {usuario.NombreUsuario}");
                    ModelState.AddModelError("", "No se pudo completar el registro. Por favor intente nuevamente.");
                }
            }
            catch (Exception ex)
            {
                await RegistrarBitacora("Error Registro Usuario",
                    $"Error durante el registro del usuario {usuario.NombreUsuario}: {ex.Message}");
                ModelState.AddModelError("", "Ocurrió un error durante el registro. Por favor intente nuevamente.");
            }

            return View(usuario);
        }

        private async Task RegistrarBitacora(string accion, string descripcion)
        {
            try
            {
                var conexion = new GestorConexion();
                await conexion.Agregar(new BitacoraModel
                {
                    FechaRegistro = DateTime.Now,
                    UsuarioRegistro = User.FindFirst(ClaimTypes.Name)?.Value ?? "Sistema",
                    AccesionRealizada = accion,
                    ModuloSistema = "Autenticación",
                    Descripcion = descripcion
                });
            }
            catch (Exception ex)
            {
                // Log locally if bitácora fails
                System.Diagnostics.Debug.WriteLine($"Error al registrar en bitácora: {ex.Message}");
            }
        }
    }
}