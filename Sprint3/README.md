# Sprint 3 — Sistema de Gestão Acadêmica

## 1. Sobre o Projeto

O Sistema de Gestão Acadêmica é uma aplicação desenvolvida como parte do Desafio Sprint 3 — Desenvolvimento de APIs .NET.

O objetivo é oferecer uma solução para gerenciamento de informações acadêmicas, permitindo cadastrar, consultar, atualizar e excluir registros de alunos, professores, disciplinas, turmas, matrículas e notas.

O sistema possui autenticação de usuários e integração entre frontend, backend e banco de dados relacional.

## 2. Tecnologias Utilizadas

### Backend
- C#
- ASP.NET Core Web API (.NET 10)
- Entity Framework Core
- Pomelo.EntityFrameworkCore.MySql
- JWT — autenticação e autorização
- Swagger — documentação e testes da API

### Frontend
- Angular
- TypeScript
- HTML
- CSS

### Banco de Dados
- MySQL 8.0

### Ferramentas
- Visual Studio
- Visual Studio Code
- MySQL Workbench
- Git e GitHub

## 3. Funcionalidades

O sistema possui os seguintes módulos:

- **Alunos:** gerenciamento de alunos.
- **Professores:** gerenciamento de professores.
- **Disciplinas:** gerenciamento de disciplinas.
- **Turmas:** gerenciamento de turmas.
- **Matrículas:** gerenciamento de matrículas.
- **Notas:** gerenciamento de notas.

Também possui:
- Cadastro e login de usuários.
- Autenticação por token JWT.
- Proteção de rotas e endpoints.
- Dashboard.
- Menu de navegação.
- Logout.

## 4. Banco de Dados

O banco de dados utilizado é o MySQL.

Schema: `sprint3_gestao_academica`

Tabelas principais:
- alunos
- professores
- disciplinas
- turmas
- matriculas
- notas
- usuarios

O sistema utiliza Entity Framework Core para o mapeamento objeto-relacional e gerenciamento dos dados.

## 5. Relacionamentos

Os relacionamentos identificados no banco de dados são:

- Matrículas relacionadas a alunos.
- Matrículas relacionadas a turmas.
- Notas relacionadas a matrículas.
- Turmas relacionadas a disciplinas.
- Turmas relacionadas a professores.

## 6. Arquitetura

O backend segue uma organização em camadas, separando responsabilidades:

- **Controllers:** recebem e respondem às requisições HTTP.
- **Services:** concentram as regras de negócio.
- **Repositories:** realizam operações de acesso aos dados.
- **Models:** representam as entidades do sistema.
- **DTOs:** organizam os dados utilizados na comunicação da API.

## 7. Como Executar o Projeto

### Backend

1. Abra a solução `Sprint3.slnx` no Visual Studio.
2. Configure a conexão com o MySQL no arquivo `appsettings.json`.
3. Verifique se o serviço MySQL está em execução.
4. Execute o backend pelo Visual Studio ou pelo terminal:

```powershell
dotnet run --project "C:\Users\025.025102\Sprint3\Sprint3\Sprint3.csproj"
```

A API utiliza o endereço local:

`http://localhost:5168`

A documentação Swagger fica disponível em:

`http://localhost:5168/swagger`

### Frontend

1. Abra a pasta do frontend no VS Code.
2. Abra o terminal na pasta `sprint3-front`.
3. Instale as dependências, caso necessário:

```bash
npm install
```

4. Inicie a aplicação:

```bash
ng serve --open
```

O frontend fica disponível em:

`http://localhost:4200`

## 8. Autenticação e Segurança

O sistema utiliza JWT (JSON Web Token) para autenticação.

Após o login, a API fornece um token que é utilizado pelo frontend nas requisições aos endpoints protegidos.

O sistema também possui:
- Validação de autenticação.
- Autorização dos endpoints.
- Interceptor HTTP no Angular.
- Remoção do token durante o logout.

## 9. Considerações Finais

O projeto integra conhecimentos de desenvolvimento backend com C#, construção de APIs REST, desenvolvimento frontend com Angular, persistência de dados com MySQL e implementação de autenticação JWT.

A aplicação foi desenvolvida com foco na organização do código, separação de responsabilidades e integração entre as tecnologias utilizadas.