using GestionEventosModelo.Entidades;
using Microsoft.EntityFrameworkCore;

namespace GestionEventosBack.Data
{
    public class DataContext:DbContext
    {
        // Constructor que recibe las opciones de configuración
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        // Cada DbSet representa una tabla de la base de datos
        // A través de estos DbSet podemos hacer consultas a cada tabla
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Ponente> Ponentes { get; set; }
        public DbSet<Actividad> Actividades { get; set; }
        public DbSet<Inscripcion> Inscripciones { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<InscripcionActividad> InscripcionActividades { get; set; }
        public DbSet<Asistencia> Asistencias { get; set; }
        public DbSet<Material> Materiales { get; set; }
        public DbSet<Certificado> Certificados { get; set; }

        // OnModelCreating es donde configuramos las relaciones entre tablas
        // y le decimos a Entity Framework cómo mapear las clases a las tablas
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ---- TABLA USUARIOS ----
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(u => u.Id_Usuario);
                // HasKey le dice a EF cuál es la clave primaria
                entity.Property(u => u.Email).IsRequired();
                entity.HasIndex(u => u.Email).IsUnique();
                // HasIndex con IsUnique garantiza que no haya emails repetidos
            }
            );

            // ---- TABLA EVENTOS ----
            modelBuilder.Entity<Evento>(entity =>
            {
                entity.HasKey(e => e.Id_Evento);
                // Un evento pertenece a un organizador (Usuario)
                entity.HasOne(e => e.Organizador)
                    .WithMany(u => u.Eventos)
                    .HasForeignKey(e => e.Id_Organizador);
            }
            );

            // ---- TABLA PONENTES ----
            modelBuilder.Entity<Ponente>(entity =>
            {
                entity.HasKey(p => p.Id_Ponente);
            }
            );

            // ---- TABLA ACTIVIDADES ----
            modelBuilder.Entity<Actividad>(entity =>
            {
                entity.HasKey(a => a.Id_Actividad);
                // Una actividad pertenece a un evento
                entity.HasOne(a => a.Evento)
                    .WithMany(e => e.Actividades)
                    .HasForeignKey(a => a.Id_Evento);
                // Una actividad puede tener un ponente (opcional)
                entity.HasOne(a => a.Ponente)
                      .WithMany(p => p.Actividades)
                      .HasForeignKey(a => a.Id_Ponente)
                      .IsRequired(false);
                // IsRequired(false) porque el ponente puede ser nulo
            });

            // ---- TABLA INSCRIPCIONES ----
            modelBuilder.Entity<Inscripcion>(entity =>
            {
                entity.HasKey(i => i.Id_Inscripcion);
                // Una inscripción pertenece a un usuario
                entity.HasOne(i => i.Usuario)
                    .WithMany(u => u.Inscripciones)
                    .HasForeignKey(i => i.Id_Usuario);
                // Una inscripción pertenece a un evento
                entity.HasOne(i => i.Evento)
                    .WithMany(e => e.Inscripciones)
                    .HasForeignKey(i => i.Id_Evento);
            });

            // ---- TABLA PAGOS ----
            modelBuilder.Entity<Pago>(entity =>
            {
                entity.HasKey(p => p.Id_Pago);
                // Un pago pertenece a una inscripción
                entity.HasOne(p => p.Inscripcion)
                    .WithOne(i => i.Pago)
                    .HasForeignKey<Pago>(p => p.Id_Inscripcion);
                // HasOne/WithOne porque es relación uno a uno
            });

            // ---- TABLA INSCRIPCION_ACTIVIDADES ----
            modelBuilder.Entity<InscripcionActividad>(entity =>
            {
                entity.HasKey(ia => ia.Id);
                // Pertenece a una inscripción
                entity.HasOne(ia => ia.Inscripcion)
                    .WithMany(i => i.InscripcionActividades)
                    .HasForeignKey(ia => ia.Id_Inscripcion);
                // Pertenece a una actividad
                entity.HasOne(ia => ia.Actividad)
                    .WithMany(a => a.InscripcionActividades)
                    .HasPrincipalKey(ia => ia.Id_Actividad);
            });

            // ---- TABLA ASISTENCIAS ----
            modelBuilder.Entity<Asistencia>(entity =>
            {
                entity.HasKey(a => a.Id_Asistencia);
                // Pertenece a una inscripción
                entity.HasOne(a => a.Inscripcion)
                    .WithMany(i => i.Asistencias)
                    .HasForeignKey(a => a.Id_Inscripcion);
                // Pertenece a una actividad
                entity.HasOne(a => a.Actividad)
                    .WithMany(a => a.Asistencias)
                    .HasForeignKey(a => a.Id_Actividad);
            });

            // ---- TABLA MATERIALES ----
            modelBuilder.Entity<Material>(entity =>
            {
                entity.HasKey(m => m.Id_Material);
                // Un material pertenece a un evento
                entity.HasOne(m => m.Evento)
                    .WithMany(e => e.Materiales)
                    .HasForeignKey(m => m.Id_Evento);
            });

            // ---- TABLA CERTIFICADOS ----
            modelBuilder.Entity<Certificado>(entity =>
            {
                entity.HasKey(c => c.Id_Certificado);
                entity.HasIndex(c => c.CodigoValidacion).IsUnique();
                // Un certificado pertenece a una inscripción (uno a uno)
                entity.HasOne(c => c.Inscripcion)
                    .WithOne(i => i.Certificado)
                    .HasForeignKey<Certificado>(c => c.Id_Inscripcion);
            });
        }
    }
}
