
import { Component } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';

interface Nota {
  id: number;
  matriculaId: number;
  valor: number;
  descricao: string;
  dataLancamento: string;
}

@Component({
  selector: 'app-notas',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    DatePipe
  ],
  templateUrl: './notas.html',
  styleUrl: './notas.css'
})
export class Notas {

  // Lista de notas cadastradas
  notas: Nota[] = [];

  // Formulário
  notaForm: Nota = this.novaNota();

  // Controle de edição
  notaEmEdicao: number | null = null;

  // Pesquisa
  termoBusca = '';

  // Mensagens
  mensagemErro = '';
  mensagemSucesso = '';

  // Consulta de média
  matriculaConsulta: number | null = null;
  mediaCalculada: number | null = null;
  situacaoCalculada = '';

  // Cria uma nota vazia
  novaNota(): Nota {
    return {
      id: 0,
      matriculaId: 0,
      valor: 0,
      descricao: '',
      dataLancamento: new Date().toISOString().substring(0, 10)
    };
  }

  // Cadastrar ou atualizar nota
  salvar(): void {
    this.mensagemErro = '';
    this.mensagemSucesso = '';

    if (
      !Number.isInteger(this.notaForm.matriculaId) ||
      this.notaForm.matriculaId <= 0
    ) {
      this.mensagemErro = 'Informe um ID de matrícula válido.';
      return;
    }

    if (
      !Number.isFinite(this.notaForm.valor) ||
      this.notaForm.valor < 0 ||
      this.notaForm.valor > 10
    ) {
      this.mensagemErro = 'A nota deve estar entre 0 e 10.';
      return;
    }

    if (!this.notaForm.dataLancamento) {
      this.mensagemErro = 'Informe a data de lançamento.';
      return;
    }

    const dados: Nota = {
      id: this.notaEmEdicao ?? this.proximoId(),
      matriculaId: this.notaForm.matriculaId,
      valor: Number(this.notaForm.valor),
      descricao: this.notaForm.descricao.trim(),
      dataLancamento: this.notaForm.dataLancamento
    };

    if (this.notaEmEdicao !== null) {
      this.notas = this.notas.map(nota =>
        nota.id === this.notaEmEdicao ? dados : nota
      );

      this.mensagemSucesso = 'Nota atualizada com sucesso!';
    } else {
      this.notas = [...this.notas, dados];

      this.mensagemSucesso = 'Nota cadastrada com sucesso!';
    }

    this.limparFormulario();
    this.limparConsulta();
  }

  // Gera o próximo ID
  proximoId(): number {
    if (this.notas.length === 0) {
      return 1;
    }

    return Math.max(...this.notas.map(nota => nota.id)) + 1;
  }

  // Carrega nota para edição
  editarNota(nota: Nota): void {
    this.notaEmEdicao = nota.id;
    this.notaForm = { ...nota };

    this.mensagemErro = '';
    this.mensagemSucesso = '';

    window.scrollTo({
      top: 0,
      behavior: 'smooth'
    });
  }

  // Exclui uma nota
  excluirNota(nota: Nota): void {
    const confirmar = window.confirm(
      'Deseja realmente excluir esta nota?'
    );

    if (!confirmar) {
      return;
    }

    this.notas = this.notas.filter(
      item => item.id !== nota.id
    );

    if (this.notaEmEdicao === nota.id) {
      this.limparFormulario();
    }

    this.mensagemSucesso = 'Nota excluída com sucesso!';
    this.mensagemErro = '';

    this.limparConsulta();
  }

  // Limpa o formulário
  limparFormulario(): void {
    this.notaForm = this.novaNota();
    this.notaEmEdicao = null;
  }

  // Cancela edição
  cancelarEdicao(): void {
    this.limparFormulario();

    this.mensagemErro = '';
    this.mensagemSucesso = '';
  }

  // Pesquisa notas
  get notasFiltradas(): Nota[] {
    const termo = this.termoBusca.trim().toLowerCase();

    if (!termo) {
      return this.notas;
    }

    return this.notas.filter(nota =>
      String(nota.id).includes(termo) ||
      String(nota.matriculaId).includes(termo) ||
      String(nota.valor).includes(termo) ||
      nota.descricao.toLowerCase().includes(termo)
    );
  }

  // Calcula a média de uma matrícula
  calcularMedia(): void {
    this.mediaCalculada = null;
    this.situacaoCalculada = '';
    this.mensagemErro = '';
    this.mensagemSucesso = '';

    if (
      this.matriculaConsulta === null ||
      !Number.isInteger(this.matriculaConsulta) ||
      this.matriculaConsulta <= 0
    ) {
      this.mensagemErro = 'Informe um ID de matrícula válido.';
      return;
    }

    const notasDaMatricula = this.notas.filter(
      nota => nota.matriculaId === this.matriculaConsulta
    );

    if (notasDaMatricula.length === 0) {
      this.mensagemErro = 'Não existem notas para esta matrícula.';
      return;
    }

    const soma = notasDaMatricula.reduce(
      (total, nota) => total + nota.valor,
      0
    );

    this.mediaCalculada = soma / notasDaMatricula.length;

    // Regra provisória: média igual ou superior a 7
    // Depois será ajustada conforme a regra do Back-end.
    this.situacaoCalculada =
      this.mediaCalculada >= 7
        ? 'Aprovado'
        : 'Em recuperação';
  }

  // Limpa o resultado da consulta
  limparConsulta(): void {
    this.mediaCalculada = null;
    this.situacaoCalculada = '';
  }

  // Quantidade total de notas
  get totalNotas(): number {
    return this.notas.length;
  }

  // Média geral de todas as notas
  get mediaGeral(): number {
    if (this.notas.length === 0) {
      return 0;
    }

    const soma = this.notas.reduce(
      (total, nota) => total + nota.valor,
      0
    );

    return soma / this.notas.length;
  }
}
