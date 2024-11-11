using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HotelFE.Models;

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
        public async Task<IActionResult> IniciarSesion(UsuarioModel P_usuario)
        {
            GestorConexion _conexion = new GestorConexion();

            if (await _conexion.Autenticacion(P_usuario))
            {
                _conexion = new GestorConexion();

                List<PerfilModel> perfiles = await _conexion.ListarPerfilesUsuario(P_usuario);

                if (perfiles.Count > 0)
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, P_usuario.NombreUsuario),
                        new Claim("Usuario", P_usuario.NombreUsuario)
                    };

                    foreach (PerfilModel perfil in perfiles)
                        claims.Add(new Claim(ClaimTypes.Role, perfil.Codigo.ToString()));

                    var claimIdentidad = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentidad));

                   return RedirectToAction("Index", "Home");
                }
            }

            return RedirectToAction("Index", "Autenticacion");
        }

        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Autenticacion");
        }

        [HttpPost]
        public async Task<IActionResult> Registro(UsuarioModel P_usuario)
        {
            GestorConexion _conexion = new GestorConexion();
            try
            {
                P_usuario.FechaRegistro = DateTime.Now;
                P_usuario.Estado = true;

                bool resultado = await _conexion.Agregar(P_usuario);

                if (resultado)
                {
                    await _conexion.Agregar(new BitacoraModel
                    {
                        FechaRegistro = DateTime.Now,
                        UsuarioRegistro = "Sistema",
                        AccesionRealizada = "Registro Usuario",
                        ModuloSistema = "Autenticación",
                        Descripcion = $"Registro exitoso de nuevo usuario: {P_usuario.NombreUsuario}"
                    });

                    return RedirectToAction("Index", "Autenticacion");
                }
                else
                {
                    // Log failedregistration attempt
                    await _conexion.Agregar(new BitacoraModel
                    {
                        FechaRegistro = DateTime.Now,
                        UsuarioRegistro = "Sistema",
                        AccesionRealizada = "Registro Usuario",
                        ModuloSistema = "Autenticación",
                        Descripcion = $"Intento fallido de registro para usuario: {P_usuario.NombreUsuario}"
                    });

                    ViewBag.Error = "No se pudo completar el registro. Por favor intente nuevamente.";
                    return View(P_usuario);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error durante el registro. Por favor intente nuevamente.";
                return View(P_usuario);
            }
        }
    }
}