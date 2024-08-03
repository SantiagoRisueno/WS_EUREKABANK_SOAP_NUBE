using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.Services;
using WS_EurekaBank_Server.Models;

namespace WS_EurekaBank_Server
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class WSEureka1 : WebService
    {
        //GOOGLE CLOUD PROXY
        //private string connectionString = "Server=127.0.0.1,1433;Database=EUREKABANK;User Id=sqlserver;Password=EurekaDb2024*;TrustServerCertificate=True;";

        //SIN PROXY

        //private string connectionString = "Server=34.176.26.104;Database=EUREKABANK;User Id=sqlserver;Password=EurekaDb2024*;Encrypt=True;TrustServerCertificate=True;";

        //GOOGLE CLOUD SHELL
        private string connectionString = "Server=34.176.26.104;Database=EUREKABANK;User Id=sqlserver;Password=EurekaDb2024*;Encrypt=True;TrustServerCertificate=True;";


        //   "Server=34.176.26.104;Database=EUREKABANK;User Id=sqlserver;Password=EurekaDb2024*;Encrypt=True;TrustServerCertificate=True;";
        // = "Server=SANTI\\SQLEXPRESS; Initial Catalog=EUREKABANK; Integrated Security=True;";

        [WebMethod]
        public string RegistrarUsuario(string nombreUsuario, string contrasena)
        {
            try
            {
                if (UsuarioExiste(nombreUsuario))
                {
                    return "El nombre de usuario ya existe.";
                }

                var contrasenaHash = HashPassword(contrasena);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Usuarios (NombreUsuario, ContrasenaHash) VALUES (@nombreUsuario, @contrasenaHash)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);
                    cmd.Parameters.AddWithValue("@contrasenaHash", contrasenaHash);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                return "Usuario registrado exitosamente";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        [WebMethod]
        public bool AutenticarUsuario(string nombreUsuario, string contrasena)
        {
            var contrasenaHash = HashPassword(contrasena);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Usuarios WHERE NombreUsuario = @nombreUsuario AND ContrasenaHash = @contrasenaHash";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);
                cmd.Parameters.AddWithValue("@contrasenaHash", contrasenaHash);

                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        private byte[] HashPassword(string password)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                return sha1.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool UsuarioExiste(string nombreUsuario)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Usuarios WHERE NombreUsuario = @nombreUsuario";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);

                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        [WebMethod]
        public DataSet traerMovimiento(string num_cuenta)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM MOVIMIENTO WHERE chr_cuencodigo = @num_cuenta";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    da.SelectCommand.Parameters.AddWithValue("@num_cuenta", num_cuenta);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return ds;
                }
            }
            catch (SqlException)
            {
                return null;
            }
        }

        [WebMethod]
        public DataSet regDeposito(string num_cuenta, double importe)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    int newMovNumber;

                    // Obtener el último valor de int_movinumero para la cuenta específica
                    string getLastMovNumberQuery = @"
                        SELECT ISNULL(MAX(int_movinumero), 0)
                        FROM MOVIMIENTO
                        WHERE chr_cuencodigo = @num_cuenta";
                    SqlCommand getLastMovNumberCmd = new SqlCommand(getLastMovNumberQuery, conn);
                    getLastMovNumberCmd.Parameters.AddWithValue("@num_cuenta", num_cuenta);
                    int lastMovNumber = (int)getLastMovNumberCmd.ExecuteScalar();
                    newMovNumber = lastMovNumber + 1;

                    // Insertar el nuevo registro
                    string query = @"
                        INSERT INTO MOVIMIENTO (
                            chr_cuencodigo, int_movinumero, dtt_movifecha, chr_emplcodigo, chr_tipocodigo, dec_moviimporte)
                        VALUES (
                            @num_cuenta, @int_movinumero, @fecha, '0004', '003', @importe)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@num_cuenta", num_cuenta);
                    cmd.Parameters.AddWithValue("@int_movinumero", newMovNumber);
                    cmd.Parameters.AddWithValue("@fecha", DateTime.Now);
                    cmd.Parameters.AddWithValue("@importe", importe);

                    cmd.ExecuteNonQuery();

                    // Devolver el conjunto de datos
                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM MOVIMIENTO WHERE chr_cuencodigo = @num_cuenta", conn);
                    da.SelectCommand.Parameters.AddWithValue("@num_cuenta", num_cuenta);
                    da.Fill(ds);
                    return ds;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
