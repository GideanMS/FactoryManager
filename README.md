# 🏭 FactoryManager

![CI](https://github.com/GideanMS/FactoryManager/actions/workflows/ci.yml/badge.svg)

> **FactoryManager** é uma API REST desenvolvida em **ASP.NET Core (.NET 9)** com foco em demonstrar boas práticas de desenvolvimento backend utilizadas no mercado. O projeto vai além de um CRUD tradicional, aplicando conceitos de arquitetura em camadas, Clean Code, SOLID e padrões de projeto para construir uma base escalável e de fácil manutenção.
>
> Este é um projeto de estudo incremental: novas funcionalidades são adicionadas continuamente, sempre priorizando a solidez arquitetural antes da expansão de escopo.

---

# 🌐 Demo ao vivo

A API está publicada e acessível publicamente:

```
https://factorymanager-api.onrender.com
```

Exemplo rápido:

```
GET https://factorymanager-api.onrender.com/machines
```

**Duas coisas importantes sobre esse ambiente de demonstração:**

* **Cold start**: hospedado no tier gratuito do Render, que desliga a instância após períodos de inatividade. A primeira requisição depois de um tempo parado pode levar até ~1 minuto para responder, enquanto o serviço "acorda". Requisições seguintes respondem normalmente.
* **Autenticação**: rotas de leitura (`GET`) são públicas. Rotas de escrita (`POST`, `PUT`, `DELETE`) exigem um header `X-Api-Key` — não incluído aqui por segurança. O banco pode ser reiniciado periodicamente.

---

# 🚀 Tecnologias

* ASP.NET Core (.NET 9)
* C#
* Minimal API
* Entity Framework Core 9
* SQL Server / Azure SQL Database
* FluentValidation
* Swagger / OpenAPI (Swashbuckle)
* Dependency Injection
* xUnit, Moq, FluentAssertions (testes)
* Docker
* GitHub Actions (CI)
* Render (hospedagem da API)
* Azure SQL Database (banco de dados gerenciado)
* Git & GitHub

---

# ☁️ Arquitetura de deploy

```text
GitHub (push na main)
        │
        ▼
GitHub Actions ── build + dotnet test
        │ (só segue se os testes passarem)
        ▼
Render ── build da imagem Docker + deploy automático
        │
        ▼
Azure SQL Database (banco gerenciado, free tier)
```

Todo push na branch `main` dispara automaticamente:

1. **Build** da solution completa.
2. **Execução dos 7 testes automatizados** (unitários + integração). Se algum teste falhar, o pipeline para aqui.
3. Só então o **Render** builda a imagem Docker (a partir do `Dockerfile` na raiz) e publica a nova versão.

A API se conecta a um **Azure SQL Database** (Serverless, free tier) hospedado separadamente — o container da API em si não guarda nenhum dado, é totalmente stateless.

---

# 🏛 Arquitetura da aplicação

O projeto segue uma arquitetura em camadas, onde cada camada possui uma responsabilidade bem definida.

```text
FactoryManager.API
        │
        ▼
FactoryManager.Application
        │
        ▼
FactoryManager.Domain
        │
        ▼
FactoryManager.Infrastructure
        │
        ▼
SQL Server / Azure SQL Database
```

## API

Responsável por:

* Endpoints
* Configuração da aplicação
* Swagger
* Middlewares
* Injeção de Dependência

A camada **não contém regras de negócio**.

---

## Application

Responsável por:

* Services
* Interfaces
* DTOs
* Validators
* Mappers
* Casos de uso

É a camada responsável por orquestrar a aplicação.

---

## Domain

Responsável por:

* Entidades
* Regras de negócio
* Domain Exceptions

Toda regra de negócio é implementada nesta camada.

---

## Infrastructure

Responsável por:

* Entity Framework Core
* DbContext
* Repositories
* Migrations
* Persistência de dados

Nenhuma regra de negócio é implementada aqui.

---

# ✨ Escopo atual do projeto

O domínio foi desenhado para um sistema completo de gerenciamento de fábrica (máquinas, produtos, recursos, receitas e usuários), mas **nem todos os módulos estão implementados de ponta a ponta ainda**. Abaixo está o estado real de cada um:

| Módulo    | Domain (entidade) | Application (DTO/Service) | API (endpoints) | Status               |
| --------- | :----------------: | :------------------------: | :---------------: | --------------------- |
| Machines  | ✅                  | ✅                          | ✅                 | **Completo**           |
| Products  | ✅                  | ⏳                          | ⏳                 | Modelado, em desenvolvimento |
| Resources | ✅                  | ⏳                          | ⏳                 | Modelado, em desenvolvimento |
| Recipes   | ✅                  | ⏳                          | ⏳                 | Modelado, em desenvolvimento |
| Users     | ✅                  | ⏳                          | ⏳                 | Modelado, em desenvolvimento |

## Machines (módulo completo)

* CRUD completo
* Validação de entrada
* Paginação
* Filtros
* Ordenação dinâmica
* DTOs para entrada e saída
* Mapeamento entre entidades e DTOs
* Proteção por API Key nas rotas de escrita (`POST`, `PUT`, `DELETE`)

---

## Validação

Utilização do **FluentValidation** para validação automática dos modelos.

Exemplos:

* Nome obrigatório
* Produção por minuto não pode ser negativa
* Limites para paginação

---

## Tratamento global de exceções

A API utiliza um **Exception Middleware** responsável por:

* Capturar exceções inesperadas
* Retornar respostas padronizadas
* Registrar logs utilizando `ILogger`

---

## Paginação

Foi implementada uma infraestrutura reutilizável para paginação utilizando componentes genéricos.

Recursos disponíveis:

* Número da página
* Quantidade de itens por página
* Total de registros
* Total de páginas
* HasNextPage
* HasPreviousPage

A paginação foi construída utilizando:

* `Skip()`
* `Take()`
* `CountAsync()`
* `PagedResult<T>`
* `QueryParameters`

---

# ✅ Testes

O projeto conta com testes automatizados cobrindo o módulo de Machines, usando uma stack de testes moderna:

* **xUnit** como framework de testes
* **Moq** para mocks de dependências
* **FluentAssertions** para assertions mais legíveis
* **Microsoft.AspNetCore.Mvc.Testing** (`WebApplicationFactory`) para testes de integração ponta a ponta
* **SQLite in-memory** como banco de dados isolado para os testes de integração, evitando dependência do SQL Server local

## Testes unitários (`FactoryManager.Tests/Services`)

Cobrem a camada de `MachineService`, incluindo:

* Busca de máquina existente e inexistente
* Criação de máquina com validação

## Testes de integração (`FactoryManager.Tests/Integration`)

Sobem a aplicação real via `CustomWebApplicationFactory` e testam o comportamento HTTP completo, incluindo:

* Criação de máquina com sucesso (`201 Created`)
* Rejeição de requisição inválida (`400 Bad Request`)

Para rodar os testes:

```bash
dotnet test
```

Os mesmos testes rodam automaticamente em todo push, via GitHub Actions (veja seção de CI/CD abaixo).

---

# 🔄 CI/CD

O projeto usa **GitHub Actions** para integração contínua. O workflow (`.github/workflows/ci.yml`) roda em todo push ou Pull Request para a `main`:

```yaml
name: CI

on:
  push:
    branches: [main]
  pull_request:
    branches: [main]

jobs:
  build-and-test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: 9.0.x
      - run: dotnet restore
      - run: dotnet build --no-restore --configuration Release
      - run: dotnet test --no-build --configuration Release --verbosity normal
```

O deploy no **Render** só é acionado depois — e só continua no ar se o build e os testes passarem antes. A branch `main` é protegida: Pull Requests só podem ser mergeadas se o check `build-and-test` passar.

---

# 🐳 Docker

A aplicação é totalmente containerizada.

```bash
# Build da imagem
docker build -t factorymanager-api .

# Rodar localmente com banco SQL Server em container
docker compose up --build
```

O `docker-compose.yml` sobe a API junto com uma instância local de SQL Server (útil para desenvolvimento sem depender de LocalDB). Em produção (Render), a API se conecta a um Azure SQL Database externo em vez de um SQL Server em container.

As migrations do Entity Framework são aplicadas automaticamente no startup da aplicação (`Database.Migrate()` no `Program.cs`), exceto em ambiente de teste, onde o schema é criado diretamente a partir do modelo (`EnsureCreated()`), evitando dependência de histórico de migrations nos testes.

---

# 📂 Estrutura do projeto

```text
FactoryManager
│
├── FactoryManager.API
│   ├── Endpoints
│   ├── Filters
│   ├── Middleware
│   └── Extensions
│
├── FactoryManager.Application
│   ├── Common
│   │   └── Pagination
│   ├── DTOs
│   ├── Extensions
│   ├── Interfaces
│   ├── Mappers
│   ├── Services
│   └── Validators
│
├── FactoryManager.Domain
│   ├── Entities
│   ├── Enums
│   ├── Exceptions
│   └── ValueObjects
│
├── FactoryManager.Infrastructure
│   ├── Extensions
│   ├── Persistence
│   ├── Repositories
│   └── Migrations
│
├── FactoryManager.Tests
│   ├── Builders
│   ├── Infrastructure
│   ├── Services
│   └── Integration
│
└── .github
    └── workflows
        └── ci.yml
```

---

# 🧠 Conceitos aplicados

* Clean Architecture
* SOLID
* Repository Pattern
* Dependency Injection
* Constructor Injection
* DTO Pattern
* Mapper Pattern
* FluentValidation
* Extension Methods
* Middleware
* Entity Framework Core
* Generic Types
* Encapsulamento (`private set`)
* Programação Assíncrona (`async/await`)
* Paginação reutilizável
* Tratamento global de exceções
* Testes unitários e de integração automatizados
* Containerização com Docker
* Integração contínua (CI) com GitHub Actions
* Deploy em nuvem com banco de dados gerenciado

---

# 📄 Endpoints

## Machines

| Método | Endpoint         | Descrição                    | Autenticação |
| ------ | ---------------- | ----------------------------- | ------------- |
| GET    | `/machines`      | Lista máquinas com paginação  | Não |
| GET    | `/machines/{id}` | Obtém uma máquina pelo ID     | Não |
| POST   | `/machines`      | Cria uma nova máquina         | `X-Api-Key` |
| PUT    | `/machines/{id}` | Atualiza uma máquina          | `X-Api-Key` |
| DELETE | `/machines/{id}` | Remove uma máquina            | `X-Api-Key` |

---

# 📌 Exemplo de paginação

```
GET /machines?page=1&pageSize=10
```

Resposta:

```json
{
  "items": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "name": "Steel Furnace",
      "productionPerMinute": 50,
      "isActive": true
    }
  ],
  "currentPage": 1,
  "pageSize": 10,
  "totalCount": 25,
  "totalPages": 3,
  "hasNextPage": true,
  "hasPreviousPage": false
}
```

---

# 🔒 Regras de negócio atuais

### Machine

* O nome é obrigatório.
* A produção por minuto não pode ser negativa.
* Produção igual a zero é permitida.
* As entidades utilizam encapsulamento com `private set`.

---

# ▶️ Como executar o projeto localmente

## Opção 1 — Docker (recomendado)

## Pré-requisitos

* [Docker Desktop](https://www.docker.com/products/docker-desktop/)

## Passos

```bash
git clone https://github.com/GideanMS/FactoryManager.git
cd FactoryManager

# Cria um arquivo .env na raiz com:
# SA_PASSWORD=SuaSenhaForte123!
# API_KEY=sua-chave-local

docker compose up --build
```

A API sobe em `http://localhost:8080`.

## Opção 2 — .NET local, sem Docker

## Pré-requisitos

* [.NET 9 SDK](https://dotnet.microsoft.com/download)
* SQL Server LocalDB (incluso no Visual Studio) ou uma instância SQL Server acessível

## Passos

```bash
git clone https://github.com/GideanMS/FactoryManager.git
cd FactoryManager

dotnet restore

dotnet ef database update --project FactoryManager.Infrastructure --startup-project FactoryManager.API

dotnet run --project FactoryManager.API
```

A API sobe em `https://localhost:7171` (ou `http://localhost:5122`). O Swagger fica disponível em `https://localhost:7171/swagger` (apenas em ambiente `Development`).

> A string de conexão padrão usa SQL Server LocalDB (`appsettings.json`). Se estiver usando outra instância de SQL Server, ajuste `ConnectionStrings:DefaultConnection` antes de rodar as migrations.

---

# 🚧 Roadmap

* [x] CRUD de Máquinas
* [x] Swagger/OpenAPI
* [x] FluentValidation
* [x] Repository Pattern
* [x] Exception Middleware
* [x] Paginação
* [x] Filtros
* [x] Ordenação dinâmica
* [x] Testes Unitários
* [x] Testes de Integração
* [x] Docker
* [x] Deploy (Render + Azure SQL Database)
* [x] CI/CD (GitHub Actions)
* [ ] ProblemDetails
* [ ] CRUD de Produtos
* [ ] CRUD de Recursos
* [ ] CRUD de Receitas
* [ ] CRUD de Usuários
* [ ] JWT Authentication
* [ ] Refresh Token
* [ ] Roles e Authorization

---

# 📈 Evolução do projeto

O FactoryManager está sendo desenvolvido incrementalmente, buscando aplicar boas práticas de engenharia de software à medida que novas funcionalidades são adicionadas. Cada implementação prioriza organização, reutilização de código e separação de responsabilidades antes da expansão das funcionalidades.

Esse repositório representa minha evolução no desenvolvimento backend com .NET e serve como um laboratório para estudar padrões arquiteturais e tecnologias amplamente utilizadas no mercado, incluindo containerização, deploy em nuvem e integração contínua.
