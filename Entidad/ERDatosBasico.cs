using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
   
        public class ERDatosBasico
        {
            public string Usuario { get; set; }
            public string FechaActual { get; set; }

        }

        public class ERDatosBasicoGestion
        {
            public string Usuario { get; set; }
            public string NombreEmpresa { get; set; }
            public string FechaActual { get; set; }

        }

        public class ERDatosBasicoPeriodo
        {
            public string Usuario { get; set; }
            public string NombreEmpresa { get; set; }
            public string NombreGestion { get; set; }
            public string FechaActual { get; set; }

        }
    public class ERDatosBasicoArticulo
    {
        public string Usuario { get; set; }
        public string NombreEmpresa { get; set; }
        public string FechaActual { get; set; }

    }
    public class ERDatosBasicoCuenta
    {
        public string Usuario { get; set; }
        public string NombreEmpresa { get; set; }
        public string FechaActual { get; set; }
        public string RutaImagen { get; set; }
        public string Direccion { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }

    }
    public class ERDatosBasicoMargenXMes
    {
        public string Usuario { get; set; }
        public string NombreEmpresa { get; set; }
        public string FechaActual { get; set; }
        public string RutaImagen { get; set; }
        public string Direccion { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string NombrePeriodo { get; set; }
        public string FechaInicioPeriodo { get; set; }
        public string FechaFinPeriodo { get; set; }

    }
    public class ERDatosBasicoComprobante
    {
        public string Usuario { get; set; }
        public string NombreEmpresa { get; set; }
        public string NombreGestion { get; set; }
        public string NombrePeriodo { get; set; }
        public string FechaActual { get; set; }
        public string Moneda { get; set; }
    }
    public class ERDatosBasicoNota
    {
        public string Usuario { get; set; }
        public string NombreEmpresa { get; set; }
        public string NombreGestion { get; set; }
        public string NombrePeriodo { get; set; }
        public string FechaActual { get; set; }
        public string Moneda { get; set; }
    }
    public class ERDatosBasicoSumasySaldos
    {
        public string Usuario { get; set; }
        public string NombreEmpresa { get; set; }
        public string NombreGestion { get; set; }
        public string FechaActual { get; set; }
        public string Moneda { get; set; }
    }
    public class ERDatosBasicoBalanceInicial
    {
        public string Usuario { get; set; }
        public string NombreEmpresa { get; set; }
        public string NombreGestion { get; set; }
        public string FechaInicioGestion { get; set; }
        public string FechaFinGestion { get; set; }
        public string FechaActual { get; set; }
        public string Moneda { get; set; }
    }
    public class ERDatosBasicoBalanceGeneral
    {
        public string Usuario { get; set; }
        public string NombreEmpresa { get; set; }
        public string NombreGestion { get; set; }
        public string FechaInicioGestion { get; set; }
        public string FechaFinGestion { get; set; }
        public string FechaActual { get; set; }
        public string Moneda { get; set; }
    }

    public class ETotalEstado
    {
        public double Total { get; set; }
    }
    public class ERDatosBasicoEstadoResultados
    {
        public string Usuario { get; set; }
        public string NombreEmpresa { get; set; }
        public string NombreGestion { get; set; }
        public string FechaInicioGestion { get; set; }
        public string FechaFinGestion { get; set; }
        public string FechaActual { get; set; }
        public string Moneda { get; set; }
    }
}
