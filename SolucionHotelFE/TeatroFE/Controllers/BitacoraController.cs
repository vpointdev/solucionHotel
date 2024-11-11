using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelFE.Models;

namespace HotelFE.Controllers
{
    public class BitacoraController : Controller
    {
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Index()
        {
            GestorConexion obj = new GestorConexion();
            List<BitacoraModel> resultado = await obj.ListarBitacora();
            return View(resultado);
        }

        [Authorize(Roles = "1")]
        public IActionResult NuevoRegistro()
        {
            return View();
        }

        [Authorize(Roles = "1")]
        public async Task<IActionResult> VerDatosParaEdicion(string pId)
        {
            BitacoraModel bitacora = new BitacoraModel { ID = pId, UsuarioRegistro = string.Empty, AccesionRealizada = string.Empty };
            GestorConexion objconexion = new GestorConexion();
            List<BitacoraModel> resultado = await objconexion.ConsultaBitacora(bitacora);
            return View(resultado.FirstOrDefault());
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GuardarRegistroNuevo(BitacoraModel P_Entidad)
        {
            GestorConexion objconexion = new GestorConexion();
            await objconexion.Agregar(P_Entidad);

            if (User.IsInRole("1"))
            {
                return RedirectToAction("Index", "Bitacora");
            }
            return Ok();
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public async Task<IActionResult> EditarRegistro(BitacoraModel P_Entidad)
        {
            GestorConexion objconexion = new GestorConexion();
            await objconexion.Modificar(P_Entidad);
            return RedirectToAction("Index", "Bitacora");
        }

        [Authorize(Roles = "1")]
        public async Task<IActionResult> BorrarRegistro(string pID)
        {
            GestorConexion objconexion = new GestorConexion();
            await objconexion.Eliminar(new BitacoraModel { ID = pID });
            return RedirectToAction("Index", "Bitacora");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RegistrarAccion(string accion, string modulo, string descripcion)
        {
            try
            {
                var bitacora = new BitacoraModel
                {
                    FechaRegistro = DateTime.Now,
                    UsuarioRegistro = User.Identity?.Name ?? "Sistema",
                    AccesionRealizada = accion,
                    ModuloSistema = modulo,
                    Descripcion = descripcion
                };

                GestorConexion objconexion = new GestorConexion();
                await objconexion.Agregar(bitacora);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error al registrar en bitácora");
            }
        }
    }
}