using HotelFE.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

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

        #region Metodo Privado

        private void EstablecerParametrosBase()
        {
            ConexionApi.BaseAddress = new Uri("http://localhost:5266");
            ConexionApi.DefaultRequestHeaders.Accept.Clear();
            ConexionApi.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        #endregion

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
            ConexionApi.DefaultRequestHeaders.Add("pID", P_Entidad.ID);
            HttpResponseMessage resultado = await ConexionApi.PutAsJsonAsync(rutaApi, P_Entidad);
            return resultado.IsSuccessStatusCode;
        }

        public async Task<bool> Eliminar(BitacoraModel P_Entidad)
        {
            string rutaApi = @"api/Bitacora/EliminarBitacora";
            ConexionApi.DefaultRequestHeaders.Add("pID", P_Entidad.ID);
            HttpResponseMessage resultado = await ConexionApi.DeleteAsync(rutaApi);
            return resultado.IsSuccessStatusCode;
        }

        public async Task<List<BitacoraModel>> ConsultaBitacora(BitacoraModel P_Entidad)
        {
            List<BitacoraModel> lstresultados = new List<BitacoraModel>();
            string rutaApi = @"api/Bitacora/ConsultarBitacora";
            ConexionApi.DefaultRequestHeaders.Add("pID", P_Entidad.ID);
            ConexionApi.DefaultRequestHeaders.Add("pUsuario", P_Entidad.UsuarioRegistro.Count() == 0 ? @"''" : P_Entidad.UsuarioRegistro);
            ConexionApi.DefaultRequestHeaders.Add("pAccesionRealizada", P_Entidad.AccesionRealizada.Count() == 0 ? @"''" : P_Entidad.AccesionRealizada);

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
            ConexionApi.DefaultRequestHeaders.Clear();
            ConexionApi.DefaultRequestHeaders.Accept.Clear();
            ConexionApi.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage resultado = await ConexionApi.PutAsJsonAsync(rutaApi, new
            {
                UsuarioId = P_Entidad.UsuarioId,
                NombreUsuario = P_Entidad.NombreUsuario,
                Clave = P_Entidad.Clave ?? string.Empty,
                CorreoRegistro = P_Entidad.CorreoRegistro,
                FechaRegistro = P_Entidad.FechaRegistro,
                Estado = P_Entidad.Estado
            });

            return resultado.IsSuccessStatusCode;
        }

        public async Task<bool> Eliminar(UsuarioModel P_Entidad)
        {
            string rutaApi = @"api/Usuario/EliminarUsuario";
            ConexionApi.DefaultRequestHeaders.Add("pUsuario", P_Entidad.UsuarioId.ToString());
            HttpResponseMessage resultado = await ConexionApi.DeleteAsync(rutaApi);
            return resultado.IsSuccessStatusCode;
        }

        public async Task<List<UsuarioModel>> ConsultarUsuario(UsuarioModel P_Entidad)
        {
            List<UsuarioModel> lstresultados = new List<UsuarioModel>();
            string rutaApi = @"api/Usuario/ConsultarUsuario";
            ConexionApi.DefaultRequestHeaders.Add("pUsuario", P_Entidad.UsuarioId.ToString());

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

        public async Task<List<UsuarioModel>> ObtenerUsuarios()
        {
            List<UsuarioModel> lstresultados = new List<UsuarioModel>();
            string rutaApi = @"api/Usuario/ObtenerTodos";

            HttpResponseMessage resultado = await ConexionApi.GetAsync(rutaApi);
            if (resultado.IsSuccessStatusCode)
            {
                string jsonstring = await resultado.Content.ReadAsStringAsync();
                lstresultados = JsonConvert.DeserializeObject<List<UsuarioModel>>(jsonstring);
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


        #region Habitacion

        public async Task<List<HabitacionModel>> ObtenerTodasHabitaciones()
        {
            List<HabitacionModel> lstresultados = new List<HabitacionModel>();
            string rutaApi = @"api/Habitacion/ListarHabitaciones";

            HttpResponseMessage resultado = await ConexionApi.GetAsync(rutaApi);
            if (resultado.IsSuccessStatusCode)
            {
                string jsonstring = await resultado.Content.ReadAsStringAsync();
                lstresultados = JsonConvert.DeserializeObject<List<HabitacionModel>>(jsonstring);
            }

            return lstresultados;
        }

        public async Task<HabitacionModel> ObtenerHabitacion(int id)
        {
            string rutaApi = @"api/Habitacion/ObtenerPorId";
            ConexionApi.DefaultRequestHeaders.Add("pHabitacionId", id.ToString());

            HttpResponseMessage resultado = await ConexionApi.GetAsync(rutaApi);

            var habitacion = await resultado.Content.ReadFromJsonAsync<HabitacionModel>();
            return habitacion;
        }

        public async Task<bool> AgregarHabitacion(HabitacionModel P_Entidad)
        {
            string rutaApi = @"api/Habitacion/AgregarHabitacion";
            HttpResponseMessage resultado = await ConexionApi.PostAsJsonAsync(rutaApi, P_Entidad);
            return resultado.IsSuccessStatusCode;
        }

        public async Task<bool> ModificarHabitacion(HabitacionModel P_Entidad)
        {
            string rutaApi = @"api/Habitacion/ModificarHabitacion";
            HttpResponseMessage resultado = await ConexionApi.PutAsJsonAsync(rutaApi, P_Entidad);
            return resultado.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarHabitacion(HabitacionModel P_Entidad)
        {
            string rutaApi = @"api/Habitacion/EliminarHabitacion";
            ConexionApi.DefaultRequestHeaders.Add("pHabitacionId", P_Entidad.HabitacionId.ToString());
            HttpResponseMessage resultado = await ConexionApi.DeleteAsync(rutaApi);
            return resultado.IsSuccessStatusCode;
        }

        public async Task<List<HabitacionModel>> ConsultarHabitacion(HabitacionModel P_Entidad)
        {
            List<HabitacionModel> lstresultados = new List<HabitacionModel>();
            string rutaApi = @"api/Habitacion/ConsultarHabitacion";
            ConexionApi.DefaultRequestHeaders.Add("pHabitacionId", P_Entidad.HabitacionId.ToString());

            HttpResponseMessage resultado = await ConexionApi.GetAsync(rutaApi);
            if (resultado.IsSuccessStatusCode)
            {
                string jsonstring = await resultado.Content.ReadAsStringAsync();
                lstresultados = JsonConvert.DeserializeObject<List<HabitacionModel>>(jsonstring);
            }

            return lstresultados;
        }

        public async Task<List<HabitacionModel>> ObtenerHabitacionesDisponibles(DateTime fechaInicio, DateTime fechaFin)
        {
            List<HabitacionModel> lstresultados = new List<HabitacionModel>();
            string rutaApi = @"api/Habitacion/ObtenerDisponibles";
            ConexionApi.DefaultRequestHeaders.Add("pFechaInicio", fechaInicio.ToString("yyyy-MM-dd"));
            ConexionApi.DefaultRequestHeaders.Add("pFechaFin", fechaFin.ToString("yyyy-MM-dd"));

            HttpResponseMessage resultado = await ConexionApi.GetAsync(rutaApi);
            if (resultado.IsSuccessStatusCode)
            {
                string jsonstring = await resultado.Content.ReadAsStringAsync();
                lstresultados = JsonConvert.DeserializeObject<List<HabitacionModel>>(jsonstring);
            }

            return lstresultados;
        }

        public async Task<bool> CambiarEstadoHabitacion(int habitacionId, string nuevoEstado)
        {
            string rutaApi = @"api/Habitacion/CambiarEstado";
            ConexionApi.DefaultRequestHeaders.Add("pHabitacionId", habitacionId.ToString());
            ConexionApi.DefaultRequestHeaders.Add("pEstado", nuevoEstado);
            HttpResponseMessage resultado = await ConexionApi.PutAsync(rutaApi, null);
            return resultado.IsSuccessStatusCode;
        }

        public async Task<List<TipoHabitacionModel>> ObtenerTiposHabitacion()
        {
            List<TipoHabitacionModel> lstresultados = new List<TipoHabitacionModel>();
            string rutaApi = @"api/Habitacion/ListarTipos";

            HttpResponseMessage resultado = await ConexionApi.GetAsync(rutaApi);
            if (resultado.IsSuccessStatusCode)
            {
                string jsonstring = await resultado.Content.ReadAsStringAsync();
                lstresultados = JsonConvert.DeserializeObject<List<TipoHabitacionModel>>(jsonstring);
            }

            return lstresultados;
        }

        #endregion


    }
}