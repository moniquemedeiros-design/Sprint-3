using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sprint3.Migrations
{
    /// <inheritdoc />
    public partial class AjustesRelacionamentosEValidacoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matriculas_Alunos_AlunoId",
                table: "Matriculas");

            migrationBuilder.DropForeignKey(
                name: "FK_Matriculas_Turmas_TurmaId",
                table: "Matriculas");

            migrationBuilder.DropForeignKey(
                name: "FK_Notas_Matriculas_MatriculaId",
                table: "Notas");

            migrationBuilder.DropForeignKey(
                name: "FK_Turmas_Disciplinas_DisciplinaId",
                table: "Turmas");

            migrationBuilder.DropForeignKey(
                name: "FK_Turmas_Professores_ProfessorId",
                table: "Turmas");

            migrationBuilder.AlterColumn<decimal>(
                name: "Valor",
                table: "Notas",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Matriculas_Alunos_AlunoId",
                table: "Matriculas",
                column: "AlunoId",
                principalTable: "Alunos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matriculas_Turmas_TurmaId",
                table: "Matriculas",
                column: "TurmaId",
                principalTable: "Turmas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notas_Matriculas_MatriculaId",
                table: "Notas",
                column: "MatriculaId",
                principalTable: "Matriculas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Turmas_Disciplinas_DisciplinaId",
                table: "Turmas",
                column: "DisciplinaId",
                principalTable: "Disciplinas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Turmas_Professores_ProfessorId",
                table: "Turmas",
                column: "ProfessorId",
                principalTable: "Professores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matriculas_Alunos_AlunoId",
                table: "Matriculas");

            migrationBuilder.DropForeignKey(
                name: "FK_Matriculas_Turmas_TurmaId",
                table: "Matriculas");

            migrationBuilder.DropForeignKey(
                name: "FK_Notas_Matriculas_MatriculaId",
                table: "Notas");

            migrationBuilder.DropForeignKey(
                name: "FK_Turmas_Disciplinas_DisciplinaId",
                table: "Turmas");

            migrationBuilder.DropForeignKey(
                name: "FK_Turmas_Professores_ProfessorId",
                table: "Turmas");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios");

            migrationBuilder.AlterColumn<decimal>(
                name: "Valor",
                table: "Notas",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldPrecision: 5,
                oldScale: 2);

            migrationBuilder.AddForeignKey(
                name: "FK_Matriculas_Alunos_AlunoId",
                table: "Matriculas",
                column: "AlunoId",
                principalTable: "Alunos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matriculas_Turmas_TurmaId",
                table: "Matriculas",
                column: "TurmaId",
                principalTable: "Turmas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notas_Matriculas_MatriculaId",
                table: "Notas",
                column: "MatriculaId",
                principalTable: "Matriculas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Turmas_Disciplinas_DisciplinaId",
                table: "Turmas",
                column: "DisciplinaId",
                principalTable: "Disciplinas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Turmas_Professores_ProfessorId",
                table: "Turmas",
                column: "ProfessorId",
                principalTable: "Professores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
