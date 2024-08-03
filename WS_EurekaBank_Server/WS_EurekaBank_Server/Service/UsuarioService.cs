using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WS_EurekaBank_Server.Service
{
    internal class UsuarioService
    {
        private string connectionString = "Server=SANTI\\SQLEXPRESS; Initial Catalog=EUREKABANK; Integrated Security=True;";

        public async Task<bool> RegistrarUsuarioAsync(string nombreUsuario, string contrasena)
        {
            if (await UsuarioExisteAsync(nombreUsuario))
            {
                throw new Exception("El nombre de usuario ya existe.");
            }

            var contrasenaHash = HashPassword(contrasena);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Usuarios (NombreUsuario, ContrasenaHash) VALUES (@nombreUsuario, @contrasenaHash)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);
                cmd.Parameters.AddWithValue("@contrasenaHash", contrasenaHash);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }

            return true;
        }

        public async Task<bool> AutenticarUsuarioAsync(string nombreUsuario, string contrasena)
        {
            var contrasenaHash = HashPassword(contrasena);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Usuarios WHERE NombreUsuario = @nombreUsuario AND ContrasenaHash = @contrasenaHash";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);
                cmd.Parameters.AddWithValue("@contrasenaHash", contrasenaHash);

                await conn.OpenAsync();
                int count = (int)await cmd.ExecuteScalarAsync();
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

        private async Task<bool> UsuarioExisteAsync(string nombreUsuario)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Usuarios WHERE NombreUsuario = @nombreUsuario";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);

                await conn.OpenAsync();
                int count = (int)await cmd.ExecuteScalarAsync();
                return count > 0;
            }
        }
    }
}
