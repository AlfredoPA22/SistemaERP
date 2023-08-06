using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class EPago
    {
        public int idPago { get; set; }
        public int idNota { get; set; }
        public string Fecha { get; set; }
        public double Monto { get; set; }
        public int Estado { get; set; }
        public int nroNota { get; set; }
        public int idcliente { get; set; }
        public string cliente { get; set; }

    }
}

