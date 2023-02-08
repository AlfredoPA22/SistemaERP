//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Datos
{
    using System;
    using System.Collections.Generic;
    
    public partial class Nota
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Nota()
        {
            this.Lote = new HashSet<Lote>();
            this.ArticuloSerie = new HashSet<ArticuloSerie>();
        }
    
        public int IdNota { get; set; }
        public int NroNota { get; set; }
        public System.DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
        public double Total { get; set; }
        public int Tipo { get; set; }
        public int IdEmpresa { get; set; }
        public int IdUsuario { get; set; }
        public Nullable<int> IdComprobante { get; set; }
        public int Estado { get; set; }
        public Nullable<int> IdVendedor { get; set; }
        public Nullable<int> IdCliente { get; set; }
        public Nullable<int> IdProveedor { get; set; }
    
        public virtual Comprobante Comprobante { get; set; }
        public virtual Empresa Empresa { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Lote> Lote { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual Proveedor Proveedor { get; set; }
        public virtual Vendedor Vendedor { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ArticuloSerie> ArticuloSerie { get; set; }
    }
}