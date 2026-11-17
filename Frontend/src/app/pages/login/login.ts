
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { Auth } from '../../services/auth';

@Component({
  selector: 'app-login',
  imports: [FormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  private auth = inject(Auth);
  private router = inject(Router);

  email = '';
  senha = '';

  mostrarSenha = false;
  aceiteTermos = false;
  carregando = false;

  mensagemErro = '';

  entrar(): void {
    this.mensagemErro = '';

    // Validar aceite dos termos
    if (!this.aceiteTermos) {
      this.mensagemErro =
        'Você precisa aceitar os Termos de Uso e confirmar a ciência do Aviso de Privacidade.';
      return;
    }

    // Validar campos
    if (!this.email.trim() || !this.senha) {
      this.mensagemErro =
        'Informe seu e-mail e sua senha.';
      return;
    }

    this.carregando = true;

    console.log('1. Iniciando login...');

    this.auth.login({
      email: this.email.trim(),
      senha: this.senha
    }).subscribe({

      // Login realizado com sucesso
      next: (resposta) => {
        console.log(
          '2. Resposta recebida:',
          resposta.mensagem
        );

        console.log(
          '3. Token armazenado:',
          !!this.auth.obterToken()
        );

        this.carregando = false;

        // Registrar autorização de acesso
        sessionStorage.setItem('acessoSistema', 'true');

        // Liberar a primeira entrada no Dashboard
        sessionStorage.setItem('entradaAposLogin', 'true');

        // Limpar autorização de navegação anterior
        sessionStorage.removeItem('navegacaoMenu');

        console.log('4. Autorizações registradas.');

        // Abrir Dashboard
        this.router.navigateByUrl('/dashboard')
          .then((navegou) => {
            console.log(
              '5. Navegação realizada:',
              navegou
            );

            if (!navegou) {
              this.mensagemErro =
                'Login realizado, mas o Dashboard continua bloqueado. Verifique o Guard das rotas.';
            }
          })
          .catch((erro) => {
            console.error(
              'Erro ao navegar:',
              erro
            );

            this.mensagemErro =
              'Não foi possível abrir o Dashboard.';
          });
      },

      // Erro na autenticação
      error: (erro) => {
        console.error(
          'ERRO NO LOGIN:',
          erro
        );

        this.carregando = false;

        this.mensagemErro =
          erro.error?.mensagem ||
          'Não foi possível realizar o login. Verifique seus dados.';
      }
    });
  }
}
