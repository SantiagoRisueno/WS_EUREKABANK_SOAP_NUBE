using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS_EurekaBank_Server.Models
{
    internal class Usuarios
    {
        public int UsuarioID { get; set; }
        public string NombreUsuario { get; set; }
        public byte[] ContrasenaHash { get; set; }
    }
}
