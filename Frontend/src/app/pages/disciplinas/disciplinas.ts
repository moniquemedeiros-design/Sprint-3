
import {
  Component,
  OnInit,
  ChangeDetectorRef,
  inject
} from '@angular/core';

import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

interface Disciplina {
  id: number;
  nome: string;
  descricao: string;
  cargaHoraria: number;
  ativa: boolean;
}

@Component({
  selector: 'app-disciplinas',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './disciplinas.html',
  styleUrl: './disciplinas.css'
})
export class Disciplinas implements OnInit {

  private http = inject(HttpClient);
  private cd = inject(ChangeDetectorRef);

  private readonly apiUrl = 'http://localhost:5168/api/Disciplinas';

  disciplinas: Disciplina[] = [];

  disciplinaForm: Disciplina = this.novaDisciplina();

  disciplinaEmEdicao: number | null = null;

  termoBusca = '';

  carregando = false;
  salvando = false;
  excluindoId: number | null = null;

  mensagemErro = '';
  mensagemSucesso = '';

  ngOnInit(): void {
    this.listarDisciplinas();
  }

  novaDisciplina(): Disciplina {
    return {
      id: 0,
      nome: '',
      descricao: '',
      cargaHoraria: 1,
      ativa: true
    };
  }

  // ATUALIZAR A VISUALIZAÇÃO
  private atualizarTela(): void {
    this.cd.markForCheck();
  }

  // LISTAR DISCIPLINAS
  listarDisciplinas(): void {
    this.carregando = true;
    this.mensagemErro = '';

    this.atualizarTela();

    this.http.get<Disciplina[]>(this.apiUrl).subscribe({

      next: (resposta) => {
        this.disciplinas = [...resposta];
        this.carregando = false;
        this.atualizarTela();
      },

      error: (erro) => {
        console.error('Erro ao carregar disciplinas:', erro);

        this.mensagemErro =
          erro.error?.mensagem ||
          'Não foi possível carregar as disciplinas. Verifique se a API está funcionando.';

        this.carregando = false;
        this.atualizarTela();
      }

    });
  }

  // CADASTRAR E ATUALIZAR DISCIPLINA
  salvar(): void {

    this.mensagemErro = '';
    this.mensagemSucesso = '';

    if (!this.disciplinaForm.nome.trim()) {
      this.mensagemErro = 'Informe o nome da disciplina.';
      this.atualizarTela();
      return;
    }

    if (
      !Number.isInteger(this.disciplinaForm.cargaHoraria) ||
      this.disciplinaForm.cargaHoraria < 1 ||
      this.disciplinaForm.cargaHoraria > 1000
    ) {
      this.mensagemErro = 'A carga horária deve estar entre 1 e 1000 horas.';
      this.atualizarTela();
      return;
    }

    if (this.salvando) {
      return;
    }

    const dados = {
      nome: this.disciplinaForm.nome.trim(),
      descricao: this.disciplinaForm.descricao.trim(),
      cargaHoraria: this.disciplinaForm.cargaHoraria,
      ativa: this.disciplinaForm.ativa
    };

    this.salvando = true;
    this.atualizarTela();

    // ATUALIZAR DISCIPLINA
    if (this.disciplinaEmEdicao !== null) {

      const id = this.disciplinaEmEdicao;

      this.http.put(`${this.apiUrl}/${id}`, {
        id,
        ...dados
      }).subscribe({

        next: () => {

          // Atualiza a lista imediatamente.
          this.disciplinas = this.disciplinas.map(disciplina =>
            disciplina.id === id
              ? { id, ...dados }
              : disciplina
          );

          this.salvando = false;
          this.mensagemSucesso = 'Disciplina atualizada com sucesso!';

          this.limparFormulario();
          this.termoBusca = '';

          this.atualizarTela();

          // Sincroniza com a API.
          this.listarDisciplinas();
        },

        error: (erro) => {
          console.error('Erro ao atualizar disciplina:', erro);

          this.mensagemErro =
            erro.error?.mensagem ||
            'Não foi possível atualizar a disciplina.';

          this.salvando = false;
          this.atualizarTela();
        }

      });

    } else {

      // CADASTRAR DISCIPLINA
      this.http.post<Disciplina>(this.apiUrl, dados).subscribe({

        next: (resposta) => {

          // Adiciona imediatamente se a API retornar o registro.
          if (resposta && resposta.id) {
            this.disciplinas = [
              ...this.disciplinas,
              resposta
            ];
          }

          this.salvando = false;
          this.mensagemSucesso = 'Disciplina cadastrada com sucesso!';

          this.limparFormulario();
          this.termoBusca = '';

          this.atualizarTela();

          // Sincroniza com a API.
          this.listarDisciplinas();
        },

        error: (erro) => {
          console.error('Erro ao cadastrar disciplina:', erro);

          this.mensagemErro =
            erro.error?.mensagem ||
            'Não foi possível cadastrar a disciplina.';

          this.salvando = false;
          this.atualizarTela();
        }

      });
    }
  }

  // EDITAR DISCIPLINA
  editarDisciplina(disciplina: Disciplina): void {

    this.disciplinaEmEdicao = disciplina.id;
    this.disciplinaForm = { ...disciplina };

    this.mensagemErro = '';
    this.mensagemSucesso = '';

    this.atualizarTela();

    window.scrollTo({
      top: 0,
      behavior: 'smooth'
    });
  }

  // EXCLUIR DISCIPLINA
  excluirDisciplina(disciplina: Disciplina): void {

    if (this.excluindoId !== null) {
      return;
    }

    const confirmar = window.confirm(
      `Deseja realmente excluir a disciplina ${disciplina.nome}?`
    );

    if (!confirmar) {
      return;
    }

    this.mensagemErro = '';
    this.mensagemSucesso = '';

    this.excluindoId = disciplina.id;
    this.atualizarTela();

    this.http.delete(`${this.apiUrl}/${disciplina.id}`).subscribe({

      next: () => {

        // Remove imediatamente da lista.
        this.disciplinas = this.disciplinas.filter(
          item => item.id !== disciplina.id
        );

        this.mensagemSucesso = 'Disciplina excluída com sucesso!';

        if (this.disciplinaEmEdicao === disciplina.id) {
          this.limparFormulario();
        }

        this.excluindoId = null;
        this.atualizarTela();

        // Sincroniza novamente com a API.
        this.listarDisciplinas();
      },

      error: (erro) => {

        console.error('Erro ao excluir disciplina:', erro);

        if (erro.status === 409) {

          this.mensagemErro =
            erro.error?.mensagem ||
            'Não é possível excluir esta disciplina porque existem turmas vinculadas a ela.';

        } else if (erro.status === 404) {

          this.mensagemErro =
            'A disciplina não foi encontrada. Atualize a lista e tente novamente.';

          this.listarDisciplinas();

        } else if (erro.status === 401 || erro.status === 403) {

          this.mensagemErro =
            'Sua sessão não está autorizada. Faça login novamente.';

        } else if (erro.status === 0) {

          this.mensagemErro =
            'Não foi possível conectar à API. Verifique se o backend está funcionando.';

        } else {

          this.mensagemErro =
            erro.error?.mensagem ||
            `Falha ao excluir a disciplina. Código HTTP: ${erro.status}.`;
        }

        this.excluindoId = null;
        this.atualizarTela();
      }

    });
  }

  // LIMPAR FORMULÁRIO
  limparFormulario(): void {
    this.disciplinaForm = this.novaDisciplina();
    this.disciplinaEmEdicao = null;
  }

  // CANCELAR EDIÇÃO
  cancelarEdicao(): void {

    this.limparFormulario();

    this.mensagemErro = '';
    this.mensagemSucesso = '';

    this.atualizarTela();
  }

  // FILTRAR DISCIPLINAS
  get disciplinasFiltradas(): Disciplina[] {

    const termo = this.termoBusca.trim().toLowerCase();

    if (!termo) {
      return this.disciplinas;
    }

    return this.disciplinas.filter(disciplina =>
      disciplina.nome.toLowerCase().includes(termo) ||
      disciplina.descricao.toLowerCase().includes(termo)
    );
  }
}
