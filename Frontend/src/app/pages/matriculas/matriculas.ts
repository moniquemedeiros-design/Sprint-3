
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DatePipe } from '@angular/common';

interface Matricula {
  id: number;
  alunoId: number;
  turmaId: number;
  dataMatricula: string;
  ativa: boolean;
}

@Component({
  selector: 'app-matriculas',
  standalone: true,
  imports: [FormsModule, DatePipe],
  templateUrl: './matriculas.html',
  styleUrl: './matriculas.css'
})
export class Matriculas {

  matriculas: Matricula[] = [];

  matriculaForm: Matricula = this.novaMatricula();

  matriculaEmEdicao: number | null = null;

  termoBusca = '';

  mensagemErro = '';
  mensagemSucesso = '';

  novaMatricula(): Matricula {
    return {
      id: 0,
      alunoId: 0,
      turmaId: 0,
      dataMatricula: new Date().toISOString().substring(0, 10),
      ativa: true
    };
  }

  get totalAtivas(): number {
    return this.matriculas.filter(m => m.ativa).length;
  }

  get totalInativas(): number {
    return this.matriculas.filter(m => !m.ativa).length;
  }

  salvar(): void {
    this.mensagemErro = '';
    this.mensagemSucesso = '';

    if (
      !Number.isInteger(this.matriculaForm.alunoId) ||
      this.matriculaForm.alunoId <= 0
    ) {
      this.mensagemErro = 'Informe um ID de aluno válido.';
      return;
    }

    if (
      !Number.isInteger(this.matriculaForm.turmaId) ||
      this.matriculaForm.turmaId <= 0
    ) {
      this.mensagemErro = 'Informe um ID de turma válido.';
      return;
    }

    if (!this.matriculaForm.dataMatricula) {
      this.mensagemErro = 'Informe a data da matrícula.';
      return;
    }

    const dados: Matricula = {
      ...this.matriculaForm,
      id: this.matriculaEmEdicao ?? this.proximoId()
    };

    if (this.matriculaEmEdicao !== null) {
      this.matriculas = this.matriculas.map(m =>
        m.id === this.matriculaEmEdicao ? dados : m
      );

      this.mensagemSucesso = 'Matrícula atualizada com sucesso!';
    } else {
      this.matriculas = [...this.matriculas, dados];

      this.mensagemSucesso = 'Matrícula cadastrada com sucesso!';
    }

    this.limparFormulario();
  }

  proximoId(): number {
    if (this.matriculas.length === 0) return 1;

    return Math.max(...this.matriculas.map(m => m.id)) + 1;
  }

  editarMatricula(matricula: Matricula): void {
    this.matriculaEmEdicao = matricula.id;
    this.matriculaForm = { ...matricula };

    this.mensagemErro = '';
    this.mensagemSucesso = '';

    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  excluirMatricula(matricula: Matricula): void {
    const confirmar = window.confirm(
      `Deseja realmente excluir a matrícula ${matricula.id}?`
    );

    if (!confirmar) return;

    this.matriculas = this.matriculas.filter(
      m => m.id !== matricula.id
    );

    if (this.matriculaEmEdicao === matricula.id) {
      this.limparFormulario();
    }

    this.mensagemSucesso = 'Matrícula excluída com sucesso!';
    this.mensagemErro = '';
  }

  limparFormulario(): void {
    this.matriculaForm = this.novaMatricula();
    this.matriculaEmEdicao = null;
  }

  cancelarEdicao(): void {
    this.limparFormulario();
    this.mensagemErro = '';
    this.mensagemSucesso = '';
  }

  get matriculasFiltradas(): Matricula[] {
    const termo = this.termoBusca.trim().toLowerCase();

    if (!termo) return this.matriculas;

    return this.matriculas.filter(m =>
      String(m.id).includes(termo) ||
      String(m.alunoId).includes(termo) ||
      String(m.turmaId).includes(termo) ||
      m.dataMatricula.includes(termo) ||
      (m.ativa ? 'ativa' : 'inativa').includes(termo)
    );
  }
}
