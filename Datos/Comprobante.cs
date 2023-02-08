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
    
    public partial class Comprobante
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Comprobante()
        {
            this.DetalleComprobante = new HashSet<DetalleComprobante>();
            this.Nota = new HashSet<Nota>();
        }
    
        public int IdComprobante { get; set; }
        public int Serie { get; set; }
        public string Glosa { get; set; }
        public System.DateTime Fecha { get; set; }
        public double TipoCambio { get; set; }
        public int Estado { get; set; }
        public int TipoComprobante { get; set; }
        public int IdEmpresa { get; set; }
        public int IdUsuario { get; set; }
        public int IdMoneda { get; set; }
    
        public virtual Empresa Empresa { get; set; }
        public virtual Moneda Moneda { get; set; }
        public virtual Usuario Usuario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetalleComprobante> DetalleComprobante { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Nota> Nota { get; set; }
    }
}
