
using Microsoft.EntityFrameworkCore;
using Sprint3.Models;

namespace Sprint3.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Professor> Professores { get; set; }
        public DbSet<Disciplina> Disciplinas { get; set; }
        public DbSet<Turma> Turmas { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }
        public DbSet<Nota> Notas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // E-mail único para cada usuário
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Turma - Professor
            modelBuilder.Entity<Turma>()
                .HasOne(t => t.Professor)
                .WithMany()
                .HasForeignKey(t => t.ProfessorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Turma - Disciplina
            modelBuilder.Entity<Turma>()
                .HasOne(t => t.Disciplina)
                .WithMany()
                .HasForeignKey(t => t.DisciplinaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Matrícula - Aluno
            modelBuilder.Entity<Matricula>()
                .HasOne(m => m.Aluno)
                .WithMany()
                .HasForeignKey(m => m.AlunoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Matrícula - Turma
            modelBuilder.Entity<Matricula>()
                .HasOne(m => m.Turma)
                .WithMany()
                .HasForeignKey(m => m.TurmaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Nota - Matrícula
            modelBuilder.Entity<Nota>()
                .HasOne(n => n.Matricula)
                .WithMany(m => m.Notas)
                .HasForeignKey(n => n.MatriculaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Precisão decimal das notas
            modelBuilder.Entity<Nota>()
                .Property(n => n.Valor)
                .HasPrecision(5, 2);
        }
    }
}
