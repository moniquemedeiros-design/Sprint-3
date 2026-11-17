
import { Component, OnInit, ChangeDetectorRef, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

interface Aluno {
  id: number;
  nome: string;
  cpf: string;
  email: string;
  matricula: string;
  dataNascimento: string;
  ativo: boolean;
}

@Component({
  selector: 'app-alunos',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './alunos.html',
  styleUrl: './alunos.css'
})
export class Alunos implements OnInit {

  private http = inject(HttpClient);
  private cd = inject(ChangeDetectorRef);

  private readonly apiUrl = 'http://localhost:5168/api/Alunos';

  alunos: Aluno[] = [];

  alunoForm: Aluno = this.novoAluno();

  alunoEmEdicao: number | null = null;

  termoBusca = '';

  carregando = false;
  salvando = false;

  excluindoId: number | null = null;

  mensagemErro = '';
  mensagemSucesso = '';

  ngOnInit(): void {
    this.listarAlunos();
  }

  novoAluno(): Aluno {
    return {
      id: 0,
      nome: '',
      cpf: '',
      email: '',
      matricula: '',
      dataNascimento: '',
      ativo: true
    };
  }

  // ATUALIZAR A VISUALIZAÇÃO
  private atualizarTela(): void {
    this.cd.markForCheck();
  }

  // LISTAR ALUNOS
  listarAlunos(): void {
    this.carregando = true;
    this.mensagemErro = '';
    this.atualizarTela();

    this.http.get<Aluno[]>(this.apiUrl).subscribe({

      next: (resposta) => {
        this.alunos = [...resposta];
        this.carregando = false;
        this.atualizarTela();
      },

      error: (erro) => {
        console.error('Erro ao carregar alunos:', erro);

        this.mensagemErro =
          erro.error?.mensagem ||
          'Não foi possível carregar os alunos. Verifique se a API está funcionando.';

        this.carregando = false;
        this.atualizarTela();
      }

    });
  }

  // CADASTRAR E ATUALIZAR ALUNO
  salvar(): void {
    this.mensagemErro = '';
    this.mensagemSucesso = '';

    if (
      !this.alunoForm.nome.trim() ||
      !this.alunoForm.cpf.trim() ||
      !this.alunoForm.email.trim() ||
      !this.alunoForm.matricula.trim() ||
      !this.alunoForm.dataNascimento
    ) {
      this.mensagemErro = 'Preencha todos os campos obrigatórios.';
      this.atualizarTela();
      return;
    }

    if (this.salvando) {
      return;
    }

    const dados = {
      nome: this.alunoForm.nome.trim(),
      cpf: this.alunoForm.cpf.trim(),
      email: this.alunoForm.email.trim(),
      matricula: this.alunoForm.matricula.trim(),
      dataNascimento: `${this.alunoForm.dataNascimento.substring(0, 10)}T00:00:00`,
      ativo: this.alunoForm.ativo
    };

    this.salvando = true;
    this.atualizarTela();

    // ATUALIZAR ALUNO
    if (this.alunoEmEdicao !== null) {

      const id = this.alunoEmEdicao;

      this.http.put(`${this.apiUrl}/${id}`, {
        id,
        ...dados
      }).subscribe({

        next: () => {
          // Atualiza a lista imediatamente.
          this.alunos = this.alunos.map(aluno =>
            aluno.id === id
              ? { id, ...dados }
              : aluno
          );

          this.salvando = false;
          this.mensagemSucesso = 'Aluno atualizado com sucesso!';

          this.limparFormulario();
          this.termoBusca = '';

          this.atualizarTela();

          // Consulta novamente a API para sincronizar os dados.
          this.listarAlunos();
        },

        error: (erro) => {
          console.error('Erro ao atualizar aluno:', erro);

          this.mensagemErro =
            erro.error?.mensagem ||
            'Não foi possível atualizar o aluno.';

          this.salvando = false;
          this.atualizarTela();
        }

      });

    } else {

      // CADASTRAR ALUNO
      this.http.post<Aluno>(this.apiUrl, dados).subscribe({

        next: (resposta) => {
          // Insere o aluno na lista imediatamente.
          if (resposta && resposta.id) {
            this.alunos = [
              ...this.alunos,
              resposta
            ];
          }

          this.salvando = false;
          this.mensagemSucesso = 'Aluno cadastrado com sucesso!';

          this.limparFormulario();
          this.termoBusca = '';

          this.atualizarTela();

          // Consulta novamente a API para sincronizar os dados.
          this.listarAlunos();
        },

        error: (erro) => {
          console.error('Erro ao cadastrar aluno:', erro);

          this.mensagemErro =
            erro.error?.mensagem ||
            'Não foi possível cadastrar o aluno.';

          this.salvando = false;
          this.atualizarTela();
        }

      });
    }
  }

  // EDITAR ALUNO
  editarAluno(aluno: Aluno): void {
    this.alunoEmEdicao = aluno.id;

    this.alunoForm = {
      ...aluno,
      dataNascimento: aluno.dataNascimento
        ? aluno.dataNascimento.substring(0, 10)
        : ''
    };

    this.mensagemErro = '';
    this.mensagemSucesso = '';

    this.atualizarTela();

    window.scrollTo({
      top: 0,
      behavior: 'smooth'
    });
  }

  // EXCLUIR ALUNO
  excluirAluno(aluno: Aluno): void {

    if (this.excluindoId !== null) {
      return;
    }

    const confirmar = window.confirm(
      `Deseja realmente excluir o aluno ${aluno.nome}?`
    );

    if (!confirmar) {
      return;
    }

    this.mensagemErro = '';
    this.mensagemSucesso = '';

    this.excluindoId = aluno.id;
    this.atualizarTela();

    this.http.delete(`${this.apiUrl}/${aluno.id}`).subscribe({

      next: () => {
        // Remove o aluno imediatamente da tela.
        this.alunos = this.alunos.filter(
          item => item.id !== aluno.id
        );

        this.mensagemSucesso = 'Aluno excluído com sucesso!';

        if (this.alunoEmEdicao === aluno.id) {
          this.limparFormulario();
        }

        this.excluindoId = null;
        this.atualizarTela();

        // Sincroniza a lista com o banco de dados.
        this.listarAlunos();
      },

      error: (erro) => {
        console.error('Erro ao excluir aluno:', erro);

        if (erro.status === 409) {
          this.mensagemErro =
            erro.error?.mensagem ||
            'Este aluno possui matrículas ou outros registros vinculados.';

        } else if (erro.status === 404) {
          this.mensagemErro =
            'O aluno não foi encontrado. Atualize a lista e tente novamente.';

          this.listarAlunos();

        } else if (erro.status === 401 || erro.status === 403) {
          this.mensagemErro =
            'Sua sessão não está autorizada. Faça login novamente.';

        } else if (erro.status === 0) {
          this.mensagemErro =
            'Não foi possível conectar à API. Verifique se o backend está funcionando.';

        } else {
          this.mensagemErro =
            erro.error?.mensagem ||
            `Falha ao excluir o aluno. Código HTTP: ${erro.status}.`;
        }

        this.excluindoId = null;
        this.atualizarTela();
      }

    });
  }

  // LIMPAR FORMULÁRIO
  limparFormulario(): void {
    this.alunoForm = this.novoAluno();
    this.alunoEmEdicao = null;
  }

  // CANCELAR EDIÇÃO
  cancelarEdicao(): void {
    this.limparFormulario();

    this.mensagemErro = '';
    this.mensagemSucesso = '';

    this.atualizarTela();
  }

  // FORMATAR DATA
  formatarData(data: string): string {
    if (!data) {
      return '—';
    }

    const partes = data.substring(0, 10).split('-');

    if (partes.length !== 3) {
      return '—';
    }

    return `${partes[2]}/${partes[1]}/${partes[0]}`;
  }

  // FILTRAR ALUNOS
  get alunosFiltrados(): Aluno[] {
    const termo = this.termoBusca.trim().toLowerCase();

    if (!termo) {
      return this.alunos;
    }

    return this.alunos.filter(aluno =>
      aluno.nome.toLowerCase().includes(termo) ||
      aluno.email.toLowerCase().includes(termo) ||
      aluno.cpf.includes(termo) ||
      aluno.matricula.toLowerCase().includes(termo)
    );
  }
}
