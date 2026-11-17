
import { Component, signal, inject } from '@angular/core';

import {
  RouterOutlet,
  RouterLink,
  RouterLinkActive,
  Router,
  NavigationEnd
} from '@angular/router';

import { filter } from 'rxjs';
import { Auth } from './services/auth';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    RouterLink,
    RouterLinkActive
  ],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  private router = inject(Router);
  private auth = inject(Auth);

  protected readonly title = signal('sprint3-front');

  menuAberto = signal(true);
  mostrarLayout = signal(false);

  constructor() {
    this.atualizarLayout();

    this.router.events
      .pipe(
        filter(event => event instanceof NavigationEnd)
      )
      .subscribe(() => this.atualizarLayout());
  }

  // Controlar a exibição do layout
  private atualizarLayout(): void {
    const rota = this.router.url.split('?')[0];

    this.mostrarLayout.set(
      rota !== '/login' &&
      rota !== '/cadastro' &&
      rota !== '/'
    );
  }

  // Abrir ou recolher menu
  alternarMenu(): void {
    this.menuAberto.update(aberto => !aberto);
  }

  // Fechar menu
  fecharMenu(): void {
    this.menuAberto.set(false);
  }

  // Navegar pelo menu
  navegarPeloMenu(rota: string): void {
    sessionStorage.setItem('navegacaoMenu', rota);

    this.fecharMenu();

    this.router.navigateByUrl(rota);
  }

  // Encerrar sessão
  sair(): void {
    // Remover JWT armazenado
    this.auth.logout();

    // Remover autorizações temporárias
    sessionStorage.removeItem('acessoSistema');
    sessionStorage.removeItem('navegacaoMenu');
    sessionStorage.removeItem('entradaAposLogin');

    this.menuAberto.set(false);

    // Retornar ao login
    this.router.navigateByUrl('/login');
  }
}
