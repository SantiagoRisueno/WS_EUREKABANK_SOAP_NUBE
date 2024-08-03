using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WS_EurekaBank_Server.Models
{
    public class Movimiento
    {
        private String numeroCuenta;
        private String idTransaccion;
        private String fecha;
        private String codigoMovimiento;
        private String tipoMovimiento;
        private double importe;

        // Getters and Setters

        public String getNumeroCuenta()
        {
            return numeroCuenta;
        }

        public void setNumeroCuenta(String numeroCuenta)
        {
            this.numeroCuenta = numeroCuenta;
        }

        public String getIdTransaccion()
        {
            return idTransaccion;
        }

        public void setIdTransaccion(String idTransaccion)
        {
            this.idTransaccion = idTransaccion;
        }

        public String getFecha()
        {
            return fecha;
        }

        public void setFecha(String fecha)
        {
            this.fecha = fecha;
        }

        public String getCodigoMovimiento()
        {
            return codigoMovimiento;
        }

        public void setCodigoMovimiento(String codigoMovimiento)
        {
            this.codigoMovimiento = codigoMovimiento;
        }

        public String getTipoMovimiento()
        {
            return tipoMovimiento;
        }

        public void setTipoMovimiento(String tipoMovimiento)
        {
            this.tipoMovimiento = tipoMovimiento;
        }

        public double getImporte()
        {
            return importe;
        }

        public void setImporte(double importe)
        {
            this.importe = importe;
        }
    }
}