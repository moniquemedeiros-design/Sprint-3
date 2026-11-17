
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

export interface LoginRequest {
  email: string;
  senha: string;
}

export interface LoginResponse {
  mensagem: string;
  token: string;
}

@Injectable({
  providedIn: 'root'
})
export class Auth {
  private http = inject(HttpClient);

  private readonly apiUrl = 'http://localhost:5168/api/Auth';

  // Realizar login
  login(dados: LoginRequest): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(`${this.apiUrl}/login`, dados)
      .pipe(
        tap(resposta => {
          if (resposta.token) {
            localStorage.setItem('token', resposta.token);
          }
        })
      );
  }

  // Encerrar sessão e remover JWT
  logout(): void {
    localStorage.removeItem('token');
  }

  // Recuperar JWT
  obterToken(): string | null {
    return localStorage.getItem('token');
  }

  // Verificar se existe token
  estaAutenticado(): boolean {
    return !!this.obterToken();
  }
}
