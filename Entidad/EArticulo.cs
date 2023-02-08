using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class EArticulo
    {
        public List<EArticuloJSON> Articulo { get; set; }
        public List<ECategoriasJSON> Categoria { get; set; }
       

    }
    public class EArticuloJSON
    {

        public int IdArticulo { get; set; }
        public string CodigoArticulo { get; set; }
        public string NombreArticulo { get; set; }
        public string NombreCategoria { get; set; }
        public string Descripcion { get; set; }
        public double PrecioVenta { get; set; }
        public double PrecioCompra { get; set; }
        public double TotalxVenta { get; set; }
        public double TotalxCompra { get; set; }
        public int Cantidad { get; set; }
        public string Marca { get; set; }
        public string Origen { get; set; }
        public int IdCategoria { get; set; }
        public string RutaImagen { get; set; }
        public byte[] Imagen { get; set; }

    }
    public class ECategoriasJSON
    {

        public int IdCategoria { get; set; }
        public string NombreCategoria { get; set; }
        public string Descripcion { get; set; }
        public int IdPadre { get; set; }
    }

}
