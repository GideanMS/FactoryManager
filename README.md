# 🏭 FactoryManager

> **FactoryManager** é uma API REST desenvolvida em **ASP.NET Core (.NET 9)** com foco em demonstrar boas práticas de desenvolvimento backend utilizadas no mercado. O projeto vai além de um CRUD tradicional, aplicando conceitos de arquitetura em camadas, Clean Code, SOLID e padrões de projeto para construir uma base escalável e de fácil manutenção.
>
> Este é um projeto de estudo incremental: novas funcionalidades são adicionadas continuamente, sempre priorizando a solidez arquitetural antes da expansão de escopo.

---

# 🚀 Tecnologias

* ASP.NET Core (.NET 9)
* C#
* Minimal API
* Entity Framework Core 9
* SQL Server (MSSQLLocalDB)
* FluentValidation
* Swagger / OpenAPI (Swashbuckle)
* Dependency Injection
* xUnit, Moq, FluentAssertions (testes)
* Git & GitHub

---

# 🏛 Arquitetura

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
SQL Server
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

---

# 📂 Estrutura do projeto

```text
FactoryManager
│
├── FactoryManager.API
│   ├── Endpoints
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
└── FactoryManager.Tests
    ├── Builders
    ├── Infrastructure
    ├── Services
    └── Integration
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

---

# 📄 Endpoints

## Machines

| Método | Endpoint         | Descrição                    |
| ------ | ---------------- | ----------------------------- |
| GET    | `/machines`      | Lista máquinas com paginação  |
| GET    | `/machines/{id}` | Obtém uma máquina pelo ID     |
| POST   | `/machines`      | Cria uma nova máquina         |
| PUT    | `/machines/{id}` | Atualiza uma máquina          |
| DELETE | `/machines/{id}` | Remove uma máquina            |

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

# ▶️ Como executar o projeto

## Pré-requisitos

* [.NET 9 SDK](https://dotnet.microsoft.com/download)
* SQL Server LocalDB (incluso no Visual Studio) ou uma instância SQL Server acessível

## Passos

```bash
# Clonar o repositório
git clone https://github.com/GideanMS/FactoryManager.git
cd FactoryManager

# Restaurar dependências
dotnet restore

# Aplicar as migrations e criar o banco de dados
dotnet ef database update --project FactoryManager.Infrastructure --startup-project FactoryManager.API

# Rodar a API
dotnet run --project FactoryManager.API
```

A API sobe em `https://localhost:7171` (ou `http://localhost:5122`). O Swagger fica disponível em:

```
https://localhost:7171/swagger
```

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
* [ ] ProblemDetails
* [ ] CRUD de Produtos
* [ ] CRUD de Recursos
* [ ] CRUD de Receitas
* [ ] CRUD de Usuários
* [ ] JWT Authentication
* [ ] Refresh Token
* [ ] Roles e Authorization
* [ ] Docker
* [ ] Deploy (Azure / Railway)
* [ ] CI/CD (GitHub Actions)

---

# 📈 Evolução do projeto

O FactoryManager está sendo desenvolvido incrementalmente, buscando aplicar boas práticas de engenharia de software à medida que novas funcionalidades são adicionadas. Cada implementação prioriza organização, reutilização de código e separação de responsabilidades antes da expansão das funcionalidades.

Esse repositório representa minha evolução no desenvolvimento backend com .NET e serve como um laboratório para estudar padrões arquiteturais e tecnologias amplamente utilizadas no mercado.
