//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CASMUL.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class pedido
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public pedido()
        {
            this.mercaderia = new HashSet<mercaderia>();
            this.pedido_detalle = new HashSet<pedido_detalle>();
        }
    
        public int id_pedido { get; set; }
        public string nro_pedido { get; set; }
        public int id_finca { get; set; }
        public int id_proveedor { get; set; }
        public System.DateTime fecha_transaccion { get; set; }
        public int semana { get; set; }
        public int periodo { get; set; }
        public bool activo { get; set; }
    
        public virtual finca finca { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<mercaderia> mercaderia { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pedido_detalle> pedido_detalle { get; set; }
        public virtual proveedor proveedor { get; set; }
    }
}
