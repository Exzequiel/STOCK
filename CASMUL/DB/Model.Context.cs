﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class dbcasmulEntities : DbContext
    {
        public dbcasmulEntities()
            : base("name=dbcasmulEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<cable> cable { get; set; }
        public virtual DbSet<cable_por_entrega> cable_por_entrega { get; set; }
        public virtual DbSet<categoria> categoria { get; set; }
        public virtual DbSet<configuracion> configuracion { get; set; }
        public virtual DbSet<entrega> entrega { get; set; }
        public virtual DbSet<entrega_detalle> entrega_detalle { get; set; }
        public virtual DbSet<finca> finca { get; set; }
        public virtual DbSet<grupo> grupo { get; set; }
        public virtual DbSet<item> item { get; set; }
        public virtual DbSet<mercaderia> mercaderia { get; set; }
        public virtual DbSet<mercaderia_detalle> mercaderia_detalle { get; set; }
        public virtual DbSet<movimiento> movimiento { get; set; }
        public virtual DbSet<movimiento_detalle> movimiento_detalle { get; set; }
        public virtual DbSet<pedido> pedido { get; set; }
        public virtual DbSet<pedido_detalle> pedido_detalle { get; set; }
        public virtual DbSet<proveedor> proveedor { get; set; }
        public virtual DbSet<requisa> requisa { get; set; }
        public virtual DbSet<requisa_detalle> requisa_detalle { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<tipo_usuario> tipo_usuario { get; set; }
        public virtual DbSet<unidad_medida> unidad_medida { get; set; }
        public virtual DbSet<usuario> usuario { get; set; }
    }
}