using Entidades.SQLServer;
using Microsoft.AspNetCore.Mvc;
using Negocio.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/Usuario")]
    public class UsuarioController : ControllerBase
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

        [HttpPost]
        [Route(nameof(AgregarUsuario))]
        public bool AgregarUsuario([FromBody] Usuario P_Entidad)
        {
            return _iUsuarioLN.Agregar(P_Entidad);
        }

        [HttpDelete]
        [Route(nameof(EliminarUsuario))]
        public bool EliminarUsuario([FromHeader] int pUsuario)
        {
            return _iUsuarioLN.Eliminar(new Usuario { UsuarioId = pUsuario });
        }

        [HttpPut]
        [Route(nameof(ModificarUsuario))]
        public bool ModificarUsuario([FromBody] Usuario P_Entidad)
        {
            return _iUsuarioLN.Modificar(P_Entidad);
        }

        [HttpGet]
        [Route(nameof(ConsultarUsuario))]
        public List<Usuario> ConsultarUsuario([FromHeader] int pUsuario)
        {
            return _iUsuarioLN.Consultar(new Usuario
            {
                UsuarioId = pUsuario
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
        [Route(nameof(ObtenerTodos))]
        public List<Usuario> ObtenerTodos()
        {
            return _iUsuarioLN.ObtenerTodos();
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