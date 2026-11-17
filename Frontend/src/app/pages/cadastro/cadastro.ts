
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router, RouterLink } from '@angular/router';
import { Auth } from '../../services/auth';

@Component({
  selector: 'app-cadastro',
  imports: [FormsModule, RouterLink],
  templateUrl: './cadastro.html',
  styleUrl: './cadastro.css'
})
export class Cadastro {
  private http = inject(HttpClient);
  private router = inject(Router);
  private auth = inject(Auth);

  private readonly apiUrl = 'http://localhost:5168/api/Auth';

  nome = '';
  email = '';
  senha = '';
  confirmarSenha = '';

  mostrarSenha = false;
  mostrarConfirmacao = false;

  aceiteTermos = false;
  carregando = false;

  mensagemErro = '';
  mensagemSucesso = '';

  cadastrar(): void {
    this.mensagemErro = '';
    this.mensagemSucesso = '';

    // Validar aceite dos termos
    if (!this.aceiteTermos) {
      this.mensagemErro =
        'Você precisa aceitar os Termos de Uso e confirmar a ciência do Aviso de Privacidade.';
      return;
    }

    // Validar campos
    if (!this.nome.trim() || !this.email.trim() || !this.senha) {
      this.mensagemErro = 'Preencha todos os campos.';
      return;
    }

    // Validar confirmação da senha
    if (this.senha !== this.confirmarSenha) {
      this.mensagemErro = 'As senhas não coincidem.';
      return;
    }

    this.carregando = true;

    // Realizar cadastro
    this.http.post<any>(`${this.apiUrl}/cadastrar`, {
      nome: this.nome.trim(),
      email: this.email.trim(),
      senha: this.senha
    }).subscribe({

      next: () => {

        console.log('Cadastro realizado. Iniciando login automático.');

        // Realizar login automático
        this.auth.login({
          email: this.email.trim(),
          senha: this.senha
        }).subscribe({

          next: (resposta) => {
            this.carregando = false;

            console.log('Login automático realizado.');

            // Registrar autorização de acesso
            sessionStorage.setItem('acessoSistema', 'true');

            // Liberar primeira entrada no Dashboard
            sessionStorage.setItem('entradaAposLogin', 'true');

            // Limpar autorização anterior
            sessionStorage.removeItem('navegacaoMenu');

            // Abrir Dashboard
            this.router.navigateByUrl('/dashboard')
              .then((navegou) => {

                console.log(
                  'Navegação para Dashboard:',
                  navegou
                );

                if (!navegou) {
                  this.mensagemErro =
                    'Cadastro e login realizados, mas não foi possível abrir o Dashboard.';
                }

              })
              .catch((erro) => {
                console.error('Erro ao abrir Dashboard:', erro);

                this.mensagemErro =
                  'Não foi possível abrir o Dashboard.';
              });
          },

          error: (erro) => {
            this.carregando = false;

            console.error('Erro no login automático:', erro);

            this.mensagemErro =
              'Cadastro realizado, mas não foi possível entrar automaticamente. Faça login com seus dados.';
          }

        });
      },

      error: (erro) => {
        this.carregando = false;

        console.error('Erro no cadastro:', erro);

        this.mensagemErro =
          erro.error?.mensagem ||
          'Não foi possível realizar o cadastro. Verifique os dados.';
      }

    });
  }
}
