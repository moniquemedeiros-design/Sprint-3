
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

interface Turma {
  id: number;
  nome: string;
  ano: number;
  semestre: number;
  professorId: number;
  disciplinaId: number;
}

@Component({
  selector: 'app-turmas',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './turmas.html',
  styleUrl: './turmas.css'
})
export class Turmas {

  turmas: Turma[] = [];

  turmaForm: Turma = this.novaTurma();

  turmaEmEdicao: number | null = null;

  termoBusca = '';

  mensagemErro = '';
  mensagemSucesso = '';

  novaTurma(): Turma {
    return {
      id: 0,
      nome: '',
      ano: new Date().getFullYear(),
      semestre: 1,
      professorId: 0,
      disciplinaId: 0
    };
  }

  salvar(): void {
    this.mensagemErro = '';
    this.mensagemSucesso = '';

    if (!this.turmaForm.nome.trim()) {
      this.mensagemErro = 'Informe o nome da turma.';
      return;
    }

    if (
      this.turmaForm.ano < 2000 ||
      this.turmaForm.ano > 2100
    ) {
      this.mensagemErro = 'Informe um ano entre 2000 e 2100.';
      return;
    }

    if (
      this.turmaForm.semestre !== 1 &&
      this.turmaForm.semestre !== 2
    ) {
      this.mensagemErro = 'Selecione o semestre 1 ou 2.';
      return;
    }

    if (
      !Number.isInteger(this.turmaForm.professorId) ||
      this.turmaForm.professorId <= 0
    ) {
      this.mensagemErro = 'Informe um ID de professor válido.';
      return;
    }

    if (
      !Number.isInteger(this.turmaForm.disciplinaId) ||
      this.turmaForm.disciplinaId <= 0
    ) {
      this.mensagemErro = 'Informe um ID de disciplina válido.';
      return;
    }

    const dados: Turma = {
      id: this.turmaEmEdicao ?? this.proximoId(),
      nome: this.turmaForm.nome.trim(),
      ano: this.turmaForm.ano,
      semestre: this.turmaForm.semestre,
      professorId: this.turmaForm.professorId,
      disciplinaId: this.turmaForm.disciplinaId
    };

    if (this.turmaEmEdicao !== null) {
      this.turmas = this.turmas.map(turma =>
        turma.id === this.turmaEmEdicao ? dados : turma
      );

      this.mensagemSucesso = 'Turma atualizada com sucesso!';
    } else {
      this.turmas = [...this.turmas, dados];

      this.mensagemSucesso = 'Turma cadastrada com sucesso!';
    }

    this.limparFormulario();
  }

  proximoId(): number {
    if (this.turmas.length === 0) return 1;

    return Math.max(...this.turmas.map(turma => turma.id)) + 1;
  }

  editarTurma(turma: Turma): void {
    this.turmaEmEdicao = turma.id;
    this.turmaForm = { ...turma };

    this.mensagemErro = '';
    this.mensagemSucesso = '';

    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  excluirTurma(turma: Turma): void {
    const confirmar = window.confirm(
      `Deseja realmente excluir a turma ${turma.nome}?`
    );

    if (!confirmar) return;

    this.turmas = this.turmas.filter(
      item => item.id !== turma.id
    );

    if (this.turmaEmEdicao === turma.id) {
      this.limparFormulario();
    }

    this.mensagemSucesso = 'Turma excluída com sucesso!';
    this.mensagemErro = '';
  }

  limparFormulario(): void {
    this.turmaForm = this.novaTurma();
    this.turmaEmEdicao = null;
  }

  cancelarEdicao(): void {
    this.limparFormulario();
    this.mensagemErro = '';
    this.mensagemSucesso = '';
  }

  get turmasFiltradas(): Turma[] {
    const termo = this.termoBusca.trim().toLowerCase();

    if (!termo) return this.turmas;

    return this.turmas.filter(turma =>
      turma.nome.toLowerCase().includes(termo) ||
      String(turma.ano).includes(termo) ||
      String(turma.semestre).includes(termo) ||
      String(turma.professorId).includes(termo) ||
      String(turma.disciplinaId).includes(termo)
    );
  }
}
