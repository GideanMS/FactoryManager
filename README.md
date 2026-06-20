# Factory Manager

Sistema de gerenciamento industrial inspirado em jogos de automação como Factorio e Satisfactory.

## Objetivo

O Factory Manager é um projeto de estudo desenvolvido para aprofundar conhecimentos em desenvolvimento backend com .NET, arquitetura em camadas, APIs REST e persistência de dados.

O sistema permitirá gerenciar recursos, máquinas e processos produtivos, simulando uma fábrica automatizada.

## Tecnologias

* .NET 9
* ASP.NET Core
* Entity Framework Core
* SQL Server
* Git
* GitHub

## Arquitetura

O projeto segue uma arquitetura em camadas:

```text
FactoryManager.API
FactoryManager.Application
FactoryManager.Domain
FactoryManager.Infrastructure
FactoryManager.Tests
```

### Domain

Contém as entidades e regras de negócio.

### Application

Contém os casos de uso e serviços da aplicação.

### Infrastructure

Responsável pelo acesso a dados e integrações externas.

### API

Responsável pela exposição dos endpoints HTTP.

## Funcionalidades Planejadas

* [ ] Cadastro de recursos
* [ ] Cadastro de máquinas
* [ ] Cadastro de produtos
* [ ] Sistema de receitas
* [ ] Produção de itens
* [ ] Controle de estoque
* [ ] Sistema financeiro
* [ ] Dashboard de produção

## Status

🚧 Em desenvolvimento

## Autor

Desenvolvido por Gidean como projeto de portfólio e aprendizado.
