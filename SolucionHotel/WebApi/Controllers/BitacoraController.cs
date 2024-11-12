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

        [HttpGet]
        [Route(nameof(ListarBitacora))]
        public List<Bitacora> ListarBitacora()
        {
            return _iBitacoraLN.Listar();
        }

        [HttpPost]
        [Route(nameof(AgregarBitacora))]
        public bool AgregarBitacora([FromBody] Bitacora P_Entidad)
        {
            return _iBitacoraLN.Agregar(new Bitacora
            {
                FechaRegistro = P_Entidad.FechaRegistro,
                UsuarioRegistro = P_Entidad.UsuarioRegistro,
                AccesionRealizada = P_Entidad.AccesionRealizada,
                ModuloSistema = P_Entidad.ModuloSistema,
                Descripcion = P_Entidad.Descripcion
            });
        }

        [HttpPut]
        [Route(nameof(ModificarBitacora))]
        public bool ModificarBitacora([FromHeader] string pID, [FromBody] Bitacora P_Entidad)
        {
            return _iBitacoraLN.Modificar(new Bitacora
            {
                ID = pID,
                FechaRegistro = P_Entidad.FechaRegistro,
                UsuarioRegistro = P_Entidad.UsuarioRegistro,
                AccesionRealizada = P_Entidad.AccesionRealizada,
                ModuloSistema = P_Entidad.ModuloSistema,
                Descripcion = P_Entidad.Descripcion
            });
        }

        [HttpDelete]
        [Route(nameof(EliminarBitacora))]
        public bool EliminarBitacora([FromHeader] string pID)
        {
            return _iBitacoraLN.Eliminar(new Bitacora { ID = pID });
        }

        [HttpGet]
        [Route(nameof(ConsultarBitacora))]
        public List<Bitacora> ConsultarBitacora(
            [FromHeader] string pID)
        {
            return _iBitacoraLN.Consultar(new Bitacora
            {
                ID = string.IsNullOrEmpty(pID.Replace("''", string.Empty)) ? string.Empty : pID
            });
        }
    }
}