# Factory Manager

Factory Manager é um projeto de estudo desenvolvido em C# e .NET com o objetivo de simular a gestão de uma fábrica. O sistema permitirá administrar máquinas, recursos, produtos e receitas de produção, além de simular processos industriais inspirados em jogos do gênero factory builder como Satisfactory e Factorio.

O projeto está sendo desenvolvido seguindo conceitos de arquitetura em camadas, Domain-Driven Design, Entity Framework Core e SQL Server.

## Objetivo

O Factory Manager é um projeto de estudo desenvolvido para aprofundar conhecimentos em desenvolvimento backend com .NET, arquitetura em camadas, APIs REST e persistência de dados.

O sistema permitirá gerenciar recursos, máquinas e processos produtivos, simulando uma fábrica automatizada.

## Tecnologias

* C#
* .NET 9
* ASP.NET Core
* Entity Framework Core
* SQL Server
* Git
* GitHub

## Arquitetura

O projeto segue uma arquitetura em camadas:

```text
FactoryManager

├── FactoryManager.API

├── FactoryManager.Application

├── FactoryManager.Domain

└── FactoryManager.Infrastructure
```

### Domain

Contém as entidades e regras de negócio.

### Application

Contém os casos de uso e serviços da aplicação.

### Infrastructure

Responsável pelo acesso a dados e integrações externas.

### API

Responsável pela exposição dos endpoints HTTP.

## Roadmap

- [x] Estrutura inicial do projeto
- [x] Entity Framework Core
- [x] SQL Server
- [x] Cadastro de máquinas
- [x] Listagem de máquinas
- [ ] Buscar máquina por ID
- [ ] Atualizar máquina
- [ ] Remover máquina
- [ ] Sistema de produtos
- [ ] Sistema de receitas
- [ ] Simulação de produção
- [ ] Sistema econômico

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

## Funcionalidades

### Máquinas

- Criar máquina
- Listar máquinas
- Persistir dados no banco SQL Server

## Autor

Desenvolvido por Gidean como projeto de portfólio e aprendizado.
