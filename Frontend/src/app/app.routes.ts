
import { Routes, CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';

import { Login } from './pages/login/login';
import { Cadastro } from './pages/cadastro/cadastro';
import { Dashboard } from './pages/dashboard/dashboard';
import { Alunos } from './pages/alunos/alunos';
import { Professores } from './pages/professores/professores';
import { Disciplinas } from './pages/disciplinas/disciplinas';
import { Turmas } from './pages/turmas/turmas';
import { Matriculas } from './pages/matriculas/matriculas';
import { Notas } from './pages/notas/notas';

// Proteção das páginas internas
const acessoGuard: CanActivateFn = () => {
  const router = inject(Router);

  const autenticado =
    sessionStorage.getItem('acessoSistema') === 'true' &&
    !!localStorage.getItem('token');

  if (autenticado) {
    return true;
  }

  return router.createUrlTree(['/login']);
};

export const routes: Routes = [

  // Páginas públicas
  {
    path: 'login',
    component: Login
  },
  {
    path: 'cadastro',
    component: Cadastro
  },

  // Páginas protegidas
  {
    path: 'dashboard',
    component: Dashboard,
    canActivate: [acessoGuard]
  },
  {
    path: 'alunos',
    component: Alunos,
    canActivate: [acessoGuard]
  },
  {
    path: 'professores',
    component: Professores,
    canActivate: [acessoGuard]
  },
  {
    path: 'disciplinas',
    component: Disciplinas,
    canActivate: [acessoGuard]
  },
  {
    path: 'turmas',
    component: Turmas,
    canActivate: [acessoGuard]
  },
  {
    path: 'matriculas',
    component: Matriculas,
    canActivate: [acessoGuard]
  },
  {
    path: 'notas',
    component: Notas,
    canActivate: [acessoGuard]
  },

  // Rotas padrão
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: '**',
    redirectTo: 'login'
  }
];
