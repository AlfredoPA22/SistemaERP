using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Entidad
{
    public class ERArticulo
    {
        public int IdArticulo { get; set; }
        public string NombreArticulo { get; set; }
        public string Descripcion { get; set; }
        public double PrecioVenta { get; set; }
        public int Cantidad { get; set; }

        public List<ECategoriaJSON> Categoria { get; set; }
        public string Codigo { get; set; }
        public int IdMarca { get; set; }
        public int IdOrigen { get; set; }
        public int StockMinimo { get; set; }
        public byte[] Imagen { get; set; }
        public string FileName { get; set; }
        public string Marca { get; set; }
        public string Origen { get; set; }
        public bool IntegracionSerie { get; set; }
    }
    public class ERArticuloLote
    {
        public List<ELoteJSON> Lote { get; set; }
    }
    public class ERArticuloVenta
    {
        public List<EVentasProductosJSON> Venta { get; set; }
    }
    public class ERNotasVenta
    {
        //grafico1
        public List<ENotasJSON> Nota { get; set; }
        public List<EPeriodosJSON> Periodo { get; set; }

    }
    public class ERGraficos2
    {
        //grafico1
        //grafico2
        public List<EProductosBajoStockJSON> Producto { get; set; }

    }
}
