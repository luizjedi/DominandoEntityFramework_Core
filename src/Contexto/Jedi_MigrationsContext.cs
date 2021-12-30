﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Meu.NameSpace;

#nullable disable

namespace Meu.NameSpace.Contexto
{
    public partial class Jedi_MigrationsContext : DbContext
    {
        public Jedi_MigrationsContext()
        {
        }

        public Jedi_MigrationsContext(DbContextOptions<Jedi_MigrationsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Pessoa> Pessoas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=GUIDE-LUIZJEDI;Initial Catalog=Jedi_Migrations;Integrated Security=true;pooling=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
