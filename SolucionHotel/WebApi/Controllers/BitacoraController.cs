using Entidades;
using Microsoft.AspNetCore.Mvc;
using Negocio.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/Bitacora")]
    public class BitacoraController : Controller
    {
        #region Atributos
        private readonly IBitacoraLN _iBitacoraLN;
        #endregion

        #region Constructor
        public BitacoraController(IBitacoraLN iBitacoraLN)
        {
            _iBitacoraLN = iBitacoraLN;
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route(nameof(AgregarBitacora))]
        public bool AgregarBitacora([FromBody] Bitacora P_Entidad)
        {
            return _iBitacoraLN.Agregar(P_Entidad);
        }

        [HttpGet]
        [Route(nameof(ListarBitacora))]
        public List<Bitacora> ListarBitacora()
        {
            return _iBitacoraLN.Listar();
        }

        [HttpDelete]
        [Route(nameof(EliminarBitacora))]
        public bool EliminarBitacora([FromHeader] string pID)
        {
            return _iBitacoraLN.Eliminar(new Bitacora { ID = pID });
        }

        [HttpPut]
        [Route(nameof(ModificarBitacora))]
        public bool ModificarBitacora([FromHeader] string pID, [FromBody] Bitacora P_Entidad)
        {
            return _iBitacoraLN.Modificar(new Bitacora
            {
                ID = pID,
                Descripcion = P_Entidad.Descripcion,
                FechaRegistro = P_Entidad.FechaRegistro,
                ModuloSistema = P_Entidad.ModuloSistema,
                UsuarioRegistro = P_Entidad.UsuarioRegistro
            });
        }

        [HttpGet]
        [Route(nameof(ConsultarBitacora))]
        public List<Bitacora> ConsultarBitacora([FromHeader] string pID, [FromHeader] string pUsuario, [FromHeader] string pAccesionRealizada)
        {
            return _iBitacoraLN.Consultar(new Bitacora
            {
                ID = string.IsNullOrEmpty(pID.Replace("''", string.Empty)) ? string.Empty : pID,
                UsuarioRegistro = string.IsNullOrEmpty(pUsuario.Replace("''", string.Empty)) ? string.Empty : pUsuario,
            });
        }
    }
}