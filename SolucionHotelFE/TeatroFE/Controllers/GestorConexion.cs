using System.Net.Http.Headers;
using HotelFE.Models;
using Newtonsoft.Json;

namespace HotelFE.Controllers
{
    public class GestorConexion
    {
        #region Propiedad

        public HttpClient ConexionApi { get; set; }

        #endregion

        #region Constructor

        public GestorConexion()
        {
            ConexionApi = new HttpClient();
            EstablecerParametrosBase();
        }

        #endregion

        #region Metodo

        #region Privado

        private void EstablecerParametrosBase()
        {
            ConexionApi.BaseAddress = new Uri("http://localhost:5266");
            ConexionApi.DefaultRequestHeaders.Accept.Clear();
            ConexionApi.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        #endregion

        #region Publico

        #region Bitacora

        public async Task<List<BitacoraModel>> ListarBitacora()
        {
            List<BitacoraModel> lstresultados = new List<BitacoraModel>();
            string rutaApi = @"api/Bitacora/ListarBitacora";

            HttpResponseMessage resultado = await ConexionApi.GetAsync(rutaApi);
            if (resultado.IsSuccessStatusCode)
            {
                string jsonstring = await resultado.Content.ReadAsStringAsync();
                lstresultados = JsonConvert.DeserializeObject<List<BitacoraModel>>(jsonstring);
            }

            return lstresultados;
        }

        public async Task<bool> Agregar(BitacoraModel P_Entidad)
        {
            string rutaApi = @"api/Bitacora/AgregarBitacora";
            HttpResponseMessage resultado = await ConexionApi.PostAsJsonAsync(rutaApi, P_Entidad);
            return resultado.IsSuccessStatusCode;
        }

        public async Task<bool> Modificar(BitacoraModel P_Entidad)
        {
            string rutaApi = @"api/Bitacora/ModificarBitacora";
            ConexionApi.DefaultRequestHeaders.Add("pID", P_Entidad.ID.ToString());
            HttpResponseMessage resultado = await ConexionApi.PutAsJsonAsync(rutaApi, P_Entidad);
            return resultado.IsSuccessStatusCode;
        }

        public async Task<bool> Eliminar(BitacoraModel P_Entidad)
        {
            string rutaApi = @"api/Bitacora/EliminarBitacora";
            ConexionApi.DefaultRequestHeaders.Add("pID", P_Entidad.ID.ToString());
            HttpResponseMessage resultado = await ConexionApi.DeleteAsync(rutaApi);
            return resultado.IsSuccessStatusCode;
        }

        public async Task<List<BitacoraModel>> ConsultaBitacora(BitacoraModel P_Entidad)
        {
            List<BitacoraModel> lstresultados = new List<BitacoraModel>();
            string rutaApi = @"api/Bitacora/ConsultarBitacora";
            ConexionApi.DefaultRequestHeaders.Add("pID", P_Entidad.ID.ToString());
            ConexionApi.DefaultRequestHeaders.Add("pUsuario", P_Entidad.UsuarioRegistro);
            ConexionApi.DefaultRequestHeaders.Add("pAccesionRealizada", P_Entidad.AccesionRealizada);
            HttpResponseMessage resultado = await ConexionApi.GetAsync(rutaApi);
            if (resultado.IsSuccessStatusCode)
            {
                string jsonstring = await resultado.Content.ReadAsStringAsync();
                lstresultados = JsonConvert.DeserializeObject<List<BitacoraModel>>(jsonstring);
            }

            return lstresultados;
        }

        #endregion

        #region Usuario

        public async Task<bool> Agregar(UsuarioModel P_Entidad)
        {
            string rutaApi = @"api/Usuario/AgregarUsuario";
            HttpResponseMessage resultado = await ConexionApi.PostAsJsonAsync(rutaApi, P_Entidad);
            return resultado.IsSuccessStatusCode;
        }

        public async Task<bool> Modificar(UsuarioModel P_Entidad)
        {
            string rutaApi = @"api/Usuario/ModificarUsuario";
            ConexionApi.DefaultRequestHeaders.Add("pUsuario", P_Entidad.NombreUsuario);
            HttpResponseMessage resultado = await ConexionApi.PutAsJsonAsync(rutaApi, P_Entidad);
            return resultado.IsSuccessStatusCode;
        }

        public async Task<bool> Eliminar(UsuarioModel P_Entidad)
        {
            string rutaApi = @"api/Usuario/EliminarUsuario";
            ConexionApi.DefaultRequestHeaders.Add("pUsuario", P_Entidad.NombreUsuario);
            HttpResponseMessage resultado = await ConexionApi.DeleteAsync(rutaApi);
            return resultado.IsSuccessStatusCode;
        }

        public async Task<List<UsuarioModel>> ConsultarUsuario(UsuarioModel P_Entidad)
        {
            List<UsuarioModel> lstresultados = new List<UsuarioModel>();
            string rutaApi = @"api/Usuario/ConsultarBitacora";
            ConexionApi.DefaultRequestHeaders.Add("pUsuario", P_Entidad.NombreUsuario);
            HttpResponseMessage resultado = await ConexionApi.GetAsync(rutaApi);
            if (resultado.IsSuccessStatusCode)
            {
                string jsonstring = await resultado.Content.ReadAsStringAsync();
                lstresultados = JsonConvert.DeserializeObject<List<UsuarioModel>>(jsonstring);
            }

            return lstresultados;
        }

        public async Task<List<PerfilModel>> ListarPerfilesUsuario(UsuarioModel P_Entidad)
        {
            List<PerfilModel> lstresultados = new List<PerfilModel>();
            string rutaApi = @"api/Usuario/PerfilesUsuario";

            ConexionApi.DefaultRequestHeaders.Add("pUsuario", P_Entidad.NombreUsuario);

            HttpResponseMessage resultado = await ConexionApi.GetAsync(rutaApi);
            if (resultado.IsSuccessStatusCode)
            {
                string jsonstring = await resultado.Content.ReadAsStringAsync();
                lstresultados = JsonConvert.DeserializeObject<List<PerfilModel>>(jsonstring);
            }

            return lstresultados;
        }

        public async Task<bool> Autenticacion(UsuarioModel P_Entidad)
        {
            bool resultadoApi = false;
            string rutaApi = @"api/Usuario/Autenticacion";
            ConexionApi.DefaultRequestHeaders.Add("pUsuario", P_Entidad.NombreUsuario);
            ConexionApi.DefaultRequestHeaders.Add("pPassword", P_Entidad.Clave);
            HttpResponseMessage resultado = await ConexionApi.GetAsync(rutaApi);
            if (resultado.IsSuccessStatusCode)
            {
                string jsonstring = await resultado.Content.ReadAsStringAsync();
                resultadoApi = JsonConvert.DeserializeObject<bool>(jsonstring);
            }

            return resultadoApi;
        }

        #endregion

        #endregion

        #endregion
    }
}