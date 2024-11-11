using Entidades.SQLServer;
using Microsoft.AspNetCore.Mvc;
using Negocio.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/Usuario")]
    public class UsuarioController : Controller
    {
        #region Atributos
        private readonly IUsuarioLN _iUsuarioLN;
        #endregion

        #region Constructor
        public UsuarioController(IUsuarioLN iUsuarioLN)
        {
            _iUsuarioLN = iUsuarioLN;
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route(nameof(AgregarUsuario))]
        public bool AgregarUsuario([FromBody] Usuario P_Entidad)
        {
            return _iUsuarioLN.Agregar(P_Entidad);
        }

        [HttpDelete]
        [Route(nameof(EliminarUsuario))]
        public bool EliminarUsuario([FromHeader] string pUsuario)
        {
            return _iUsuarioLN.Eliminar(new Usuario { NombreUsuario = pUsuario });
        }

        [HttpPut]
        [Route(nameof(ModificarUsuario))]
        public bool ModificarUsuario([FromHeader] string pUsuario, [FromBody] Usuario P_Entidad)
        {
            return _iUsuarioLN.Modificar(new Usuario
            {
                NombreUsuario = pUsuario,
                Clave = P_Entidad.Clave,
                FechaRegistro = P_Entidad.FechaRegistro,
                CorreoRegistro = P_Entidad.CorreoRegistro,
                Estado = P_Entidad.Estado
            });
        }

        [HttpGet]
        [Route(nameof(ConsultarUsuario))]
        public List<Usuario> ConsultarUsuario([FromHeader] string pUsuario)
        {
            return _iUsuarioLN.Consultar(new Usuario
            {
                NombreUsuario = string.IsNullOrEmpty(pUsuario.Replace("''", string.Empty)) ? string.Empty : pUsuario
            });
        }

        [HttpGet]
        [Route(nameof(PerfilesUsuario))]
        public List<Perfil> PerfilesUsuario([FromHeader] string pUsuario)
        {
            return _iUsuarioLN.PerfilesUsuario(new Usuario
            {
                NombreUsuario = pUsuario
            });
        }

        [HttpGet]
        [Route(nameof(Autenticacion))]
        public bool Autenticacion([FromHeader] string pUsuario, [FromHeader] string pPassword)
        {
            return _iUsuarioLN.Autenticacion(new Usuario
            {
                NombreUsuario = pUsuario,
                Clave = pPassword
            });
        }
    }
}