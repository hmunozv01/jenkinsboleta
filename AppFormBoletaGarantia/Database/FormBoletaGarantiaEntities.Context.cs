﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AppFormBoletaGarantia.Database
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class FormularioBoletaGarantiaEntities : DbContext
    {
        public FormularioBoletaGarantiaEntities()
            : base("name=FormularioBoletaGarantiaEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Inmobiliaria> Inmobiliaria { get; set; }
        public virtual DbSet<TipoGarantia> TipoGarantia { get; set; }
        public virtual DbSet<TipoMoneda> TipoMoneda { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<UsuarioInmobiliaria> UsuarioInmobiliaria { get; set; }
        public virtual DbSet<EstadoBoletaGarantia> EstadoBoletaGarantia { get; set; }
        public virtual DbSet<Grupo> Grupo { get; set; }
        public virtual DbSet<BoletaGarantia> BoletaGarantia { get; set; }
    
        public virtual ObjectResult<SP_GetUsuarios_Result> SP_GetUsuarios()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetUsuarios_Result>("SP_GetUsuarios");
        }
    
        public virtual ObjectResult<SP_UsuarioInmobiliarias_Result> SP_UsuarioInmobiliarias(Nullable<int> usuarioId)
        {
            var usuarioIdParameter = usuarioId.HasValue ?
                new ObjectParameter("UsuarioId", usuarioId) :
                new ObjectParameter("UsuarioId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_UsuarioInmobiliarias_Result>("SP_UsuarioInmobiliarias", usuarioIdParameter);
        }
    }
}
