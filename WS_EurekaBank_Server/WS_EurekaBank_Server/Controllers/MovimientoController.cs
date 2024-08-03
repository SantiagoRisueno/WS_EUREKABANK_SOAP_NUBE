using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WS_EurekaBank_Server.Models;
using WS_EurekaBank_Server.Service;

namespace WS_EurekaBank_Server.Controllers
{
    public class MovimientoController
    {
        private MovimientoService movimientoService;

        public MovimientoController()
        {
            movimientoService = new MovimientoService();
        }

        public List<Movimiento> obtenerMovimientos(String numCuenta)
        {
            return movimientoService.traerMovimientos(numCuenta);
        }

        public bool registrarDeposito(Movimiento movimiento)
        {
            return movimientoService.registrarDeposito(movimiento);
        }
    }
}