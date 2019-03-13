﻿namespace BilBakalim.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class BilBakalimContext : DbContext
    {
        public BilBakalimContext()
            : base("name=BilBakalimContext")
        {
        }

        public virtual DbSet<Dİl> Dİl { get; set; }
        public virtual DbSet<Favori> Favori { get; set; }
        public virtual DbSet<Kullanici> Kullanici { get; set; }
        public virtual DbSet<KullaniciResim> KullaniciResim { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<MenuRol> MenuRol { get; set; }
        public virtual DbSet<Rapor> Rapor { get; set; }
        public virtual DbSet<Resim> Resim { get; set; }
        public virtual DbSet<ResimKategori> ResimKategori { get; set; }
        public virtual DbSet<Rol> Rol { get; set; }
        public virtual DbSet<Sinif> Sinif { get; set; }
        public virtual DbSet<SinifKategori> SinifKategori { get; set; }
        public virtual DbSet<Sorular> Sorular { get; set; }
        public virtual DbSet<Takip> Takip { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dİl>()
                .HasMany(e => e.Sinif)
                .WithOptional(e => e.Dİl)
                .HasForeignKey(e => e.LisanID);

            modelBuilder.Entity<Kullanici>()
                .HasMany(e => e.Takip)
                .WithOptional(e => e.Kullanici)
                .HasForeignKey(e => e.TakipEdenID);

            modelBuilder.Entity<Kullanici>()
                .HasMany(e => e.Takip1)
                .WithOptional(e => e.Kullanici1)
                .HasForeignKey(e => e.TakipEdilenID);

            modelBuilder.Entity<KullaniciResim>()
                .HasMany(e => e.Kullanici)
                .WithOptional(e => e.KullaniciResim)
                .HasForeignKey(e => e.ResimID);

            modelBuilder.Entity<Menu>()
                .HasMany(e => e.Menu1)
                .WithOptional(e => e.Menu2)
                .HasForeignKey(e => e.ParentMenuID);

            modelBuilder.Entity<Sinif>()
                .HasMany(e => e.Rapor)
                .WithOptional(e => e.Sinif)
                .HasForeignKey(e => e.SınıfID);

            modelBuilder.Entity<Sorular>()
                .Property(e => e.DogruCevap)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
