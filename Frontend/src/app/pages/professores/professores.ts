
import {
  Component,
  OnInit,
  ChangeDetectorRef,
  inject
} from '@angular/core';

import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

interface Professor {
  id: number;
  nome: string;
  cpf: string;
  email: string;
  especialidade: string;
  ativo: boolean;
}

@Component({
  selector: 'app-professores',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './professores.html',
  styleUrl: './professores.css'
})
export class Professores implements OnInit {

  private http = inject(HttpClient);
  private cd = inject(ChangeDetectorRef);

  private readonly apiUrl = 'http://localhost:5168/api/Professores';

  professores: Professor[] = [];

  professorForm: Professor = this.novoProfessor();

  professorEmEdicao: number | null = null;

  termoBusca = '';

  carregando = false;
  salvando = false;

  excluindoId: number | null = null;

  mensagemErro = '';
  mensagemSucesso = '';

  ngOnInit(): void {
    this.listarProfessores();
  }

  novoProfessor(): Professor {
    return {
      id: 0,
      nome: '',
      cpf: '',
      email: '',
      especialidade: '',
      ativo: true
    };
  }

  // ATUALIZAR A VISUALIZAÇÃO
  private atualizarTela(): void {
    this.cd.markForCheck();
  }

  // LISTAR PROFESSORES
  listarProfessores(): void {
    this.carregando = true;
    this.mensagemErro = '';

    this.atualizarTela();

    this.http.get<Professor[]>(this.apiUrl).subscribe({

      next: (resposta) => {
        this.professores = [...resposta];

        this.carregando = false;
        this.atualizarTela();
      },

      error: (erro) => {
        console.error('Erro ao carregar professores:', erro);

        this.mensagemErro =
          erro.error?.mensagem ||
          'Não foi possível carregar os professores. Verifique se a API está funcionando.';

        this.carregando = false;
        this.atualizarTela();
      }

    });
  }

  // CADASTRAR E ATUALIZAR PROFESSOR
  salvar(): void {

    this.mensagemErro = '';
    this.mensagemSucesso = '';

    if (
      !this.professorForm.nome.trim() ||
      !this.professorForm.cpf.trim() ||
      !this.professorForm.email.trim() ||
      !this.professorForm.especialidade.trim()
    ) {
      this.mensagemErro = 'Preencha todos os campos obrigatórios.';
      this.atualizarTela();
      return;
    }

    if (this.salvando) {
      return;
    }

    const dados = {
      nome: this.professorForm.nome.trim(),
      cpf: this.professorForm.cpf.trim(),
      email: this.professorForm.email.trim(),
      especialidade: this.professorForm.especialidade.trim(),
      ativo: this.professorForm.ativo
    };

    this.salvando = true;
    this.atualizarTela();

    // ATUALIZAR PROFESSOR
    if (this.professorEmEdicao !== null) {

      const id = this.professorEmEdicao;

      this.http.put(`${this.apiUrl}/${id}`, {
        id,
        ...dados
      }).subscribe({

        next: () => {

          // Atualiza imediatamente a lista.
          this.professores = this.professores.map(professor =>
            professor.id === id
              ? { id, ...dados }
              : professor
          );

          this.salvando = false;
          this.mensagemSucesso = 'Professor atualizado com sucesso!';

          this.limparFormulario();
          this.termoBusca = '';

          this.atualizarTela();

          // Sincroniza com o banco de dados.
          this.listarProfessores();
        },

        error: (erro) => {
          console.error('Erro ao atualizar professor:', erro);

          this.mensagemErro =
            erro.error?.mensagem ||
            'Não foi possível atualizar o professor.';

          this.salvando = false;
          this.atualizarTela();
        }

      });

    } else {

      // CADASTRAR PROFESSOR
      this.http.post<Professor>(this.apiUrl, dados).subscribe({

        next: (resposta) => {

          // Adiciona imediatamente à lista, se a API retornar o registro.
          if (resposta && resposta.id) {
            this.professores = [
              ...this.professores,
              resposta
            ];
          }

          this.salvando = false;
          this.mensagemSucesso = 'Professor cadastrado com sucesso!';

          this.limparFormulario();
          this.termoBusca = '';

          this.atualizarTela();

          // Sincroniza com o banco de dados.
          this.listarProfessores();
        },

        error: (erro) => {
          console.error('Erro ao cadastrar professor:', erro);

          this.mensagemErro =
            erro.error?.mensagem ||
            'Não foi possível cadastrar o professor.';

          this.salvando = false;
          this.atualizarTela();
        }

      });
    }
  }

  // EDITAR PROFESSOR
  editarProfessor(professor: Professor): void {

    this.professorEmEdicao = professor.id;
    this.professorForm = { ...professor };

    this.mensagemErro = '';
    this.mensagemSucesso = '';

    this.atualizarTela();

    window.scrollTo({
      top: 0,
      behavior: 'smooth'
    });
  }

  // EXCLUIR PROFESSOR
  excluirProfessor(professor: Professor): void {

    if (this.excluindoId !== null) {
      return;
    }

    const confirmar = window.confirm(
      `Deseja realmente excluir o professor ${professor.nome}?`
    );

    if (!confirmar) {
      return;
    }

    this.mensagemErro = '';
    this.mensagemSucesso = '';

    this.excluindoId = professor.id;
    this.atualizarTela();

    this.http.delete(`${this.apiUrl}/${professor.id}`).subscribe({

      next: () => {

        // Remove imediatamente da lista.
        this.professores = this.professores.filter(
          item => item.id !== professor.id
        );

        this.mensagemSucesso = 'Professor excluído com sucesso!';

        if (this.professorEmEdicao === professor.id) {
          this.limparFormulario();
        }

        this.excluindoId = null;
        this.atualizarTela();

        // Sincroniza novamente com a API.
        this.listarProfessores();
      },

      error: (erro) => {

        console.error('Erro ao excluir professor:', erro);

        if (erro.status === 409) {

          this.mensagemErro =
            erro.error?.mensagem ||
            'Não é possível excluir este professor porque existem turmas vinculadas a ele.';

        } else if (erro.status === 404) {

          this.mensagemErro =
            'O professor não foi encontrado. Atualize a lista e tente novamente.';

          this.listarProfessores();

        } else if (erro.status === 401 || erro.status === 403) {

          this.mensagemErro =
            'Sua sessão não está autorizada. Faça login novamente.';

        } else if (erro.status === 0) {

          this.mensagemErro =
            'Não foi possível conectar à API. Verifique se o backend está funcionando.';

        } else {

          this.mensagemErro =
            erro.error?.mensagem ||
            `Falha ao excluir o professor. Código HTTP: ${erro.status}.`;
        }

        this.excluindoId = null;
        this.atualizarTela();
      }

    });
  }

  // LIMPAR FORMULÁRIO
  limparFormulario(): void {
    this.professorForm = this.novoProfessor();
    this.professorEmEdicao = null;
  }

  // CANCELAR EDIÇÃO
  cancelarEdicao(): void {

    this.limparFormulario();

    this.mensagemErro = '';
    this.mensagemSucesso = '';

    this.atualizarTela();
  }

  // FILTRAR PROFESSORES
  get professoresFiltrados(): Professor[] {

    const termo = this.termoBusca.trim().toLowerCase();

    if (!termo) {
      return this.professores;
    }

    return this.professores.filter(professor =>
      professor.nome.toLowerCase().includes(termo) ||
      professor.email.toLowerCase().includes(termo) ||
      professor.cpf.includes(termo) ||
      professor.especialidade.toLowerCase().includes(termo)
    );
  }
}
