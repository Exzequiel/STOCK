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
    
    public partial class movimiento_detalle
    {
        public int id_detalle_movimiento { get; set; }
        public int id_movimiento { get; set; }
        public int id_item { get; set; }
        public int cant_enviada { get; set; }
        public bool activo { get; set; }
    
        public virtual item item { get; set; }
        public virtual movimiento movimiento { get; set; }
    }
}
