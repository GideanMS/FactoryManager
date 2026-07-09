# Factory Manager

Factory Manager é um projeto de estudo desenvolvido em C# e .NET com o objetivo de simular a gestão de uma fábrica. O sistema permitirá administrar máquinas, recursos, produtos e receitas de produção, além de simular processos industriais inspirados em jogos do gênero factory builder como Satisfactory e Factorio.

A ideia desse projeto é melhorar minhas habilidades de desenvolvimento praticando:

- ASP.NET Core
- REST API Design
- Entity Framework Core
- Software Architecture
- Design Patterns
- Dependency Injection
- Clean Code

## Tecnologias

* C#
* .NET 9
* ASP.NET Core
* Entity Framework Core
* SQL Server
* Git
* GitHub
* Swagger

## Arquitetura

O projeto segue uma arquitetura em camadas:

## Architecture

```text
FactoryManager
│
├── FactoryManager.API
│   ├── Endpoints
│   └── Middlewares
│
├── FactoryManager.Application
│   ├── DTOs
│   ├── Interfaces
│   ├── Mappers
│   ├── Services
│   └── Validators
│
├── FactoryManager.Domain
│   ├── Entities
│   └── Exceptions
│
└── FactoryManager.Infrastructure
    ├── Persistence
    ├── Repositories
    └── Migrations
```

## Roadmap

- [x] Estrutura inicial do projeto
- [x] Entity Framework Core
- [x] Machine CRUD
- [x] Repository Pattern
- [x] Service Layer
- [x] DTOs
- [x] FluentValidation
- [x] Exception Middleware
- [x] Swagger
- [ ] Product CRUD
- [ ] User CRUD
- [ ] JWT Authentication
- [ ] Docker
- [ ] Unit Tests
- [ ] Integration Tests

## Status

Atualmente o Factory Manager possui:

- Estrutura em camadas (Domain, Application, Infrastructure e API)
- Entity Framework Core
- SQL Server
- Migrations
- Repository Pattern
- Service Layer
- Cadastro de máquinas
- Listagem de máquinas
- Persistência de dados em banco
- CRUD completo de Machine
- DTOs de Request e Response
- MachineMapper
- FluentValidation
- Tratamento centralizado de exceções
- Swagger/OpenAPI
- Dependency Injection

## Funcionalidades

### Máquinas

- Criar máquina
- Listar máquinas
- Buscar máquina por Id
- Persistir dados no banco SQL Server
- Atualizar máquina
- Remover máquina
  
## Autor

Desenvolvido por Gidean como projeto de portfólio e aprendizado.
