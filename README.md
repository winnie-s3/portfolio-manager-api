# Portfolio Manager API

API REST desenvolvida em **C# e ASP.NET Core** para gerenciamento de carteiras de investimentos.

O projeto nasceu como um estudo prático do ecossistema .NET moderno e está sendo evoluído para uma aplicação de investimentos mais completa, com foco em **boas práticas de backend, regras de negócio, segurança, testes e arquitetura**.

> 🚧 Projeto em desenvolvimento contínuo.

---

## Sobre o projeto

O **Portfolio Manager API** permite gerenciar usuários, carteiras e ativos financeiros por meio de uma API REST.

A aplicação está sendo refatorada para utilizar **operações de compra e venda como fonte de verdade**, permitindo futuramente calcular posições, preço médio, custos e resultados de cada carteira de forma auditável.

O projeto também é utilizado como laboratório técnico para aprofundamento em desenvolvimento backend e arquitetura de sistemas.

---

## Tecnologias

### Backend

* C#
* ASP.NET Core Web API
* Entity Framework Core
* LINQ
* REST APIs
* Dependency Injection
* Swagger / OpenAPI

### Banco de dados

* SQLite
* Entity Framework Core Migrations

### Segurança

* JWT Authentication
* BCrypt para hash de senhas
* .NET User Secrets para armazenamento local de secrets

### Testes

* xUnit
* Entity Framework Core InMemory
* Arrange / Act / Assert

### Versionamento

* Git
* GitHub

---

## Funcionalidades implementadas

### Autenticação

* Cadastro de usuários
* Login
* Hash seguro de senhas com BCrypt
* Geração de JWT
* Proteção de endpoints autenticados

### Carteiras

* Criação de carteiras
* Consulta de carteiras
* Atualização
* Exclusão
* Relacionamento entre usuário e carteira

### Ativos

* Cadastro de ativos
* Consulta
* Atualização
* Exclusão
* Pesquisa e paginação

### Testes

Testes unitários utilizando **xUnit** e banco em memória com **EF Core InMemory**.

Entre os cenários já testados:

* criação válida de recurso;
* validação de relacionamentos inexistentes;
* tentativa de exclusão de recurso inexistente.

---

## Arquitetura atual

A aplicação utiliza separação de responsabilidades entre as principais camadas:

```text
HTTP Request
     │
     ▼
Controller
     │
     ▼
Service
     │
     ▼
Entity Framework Core
     │
     ▼
Database
```

Além disso, o projeto utiliza:

```text
Controllers/
Services/
DTOs/
Mappings/
Models/
Data/
Configuration/
Migrations/
Tests/
```

### Responsabilidades

**Controllers**

Responsáveis pela camada HTTP, recebendo requisições e retornando respostas.

**Services**

Concentram regras de negócio e operações da aplicação.

**DTOs**

Definem os contratos de entrada e saída da API, evitando exposição direta das entidades persistidas.

**Mappings**

Responsáveis pela conversão entre entidades e DTOs.

**Entity Framework Core**

Responsável pelo acesso e persistência dos dados.

---

## Estrutura do projeto

```text
PortfolioManager.Api
│
├── Configuration
│   └── JwtSettings.cs
│
├── Controllers
│   ├── AssetsController.cs
│   ├── AuthController.cs
│   └── PortfoliosController.cs
│
├── Data
│   └── AppDbContext.cs
│
├── Dtos
│   ├── AssetDto.cs
│   ├── CreateAssetDto.cs
│   ├── CreatePortfolioDto.cs
│   ├── LoginRequestDto.cs
│   ├── LoginResponseDto.cs
│   ├── PagedResponse.cs
│   ├── PortfolioDto.cs
│   ├── RegisterUserDto.cs
│   ├── UpdateAssetDto.cs
│   ├── UpdatePortfolioDto.cs
│   └── UserDto.cs
│
├── Mappings
│   ├── AssetMapper.cs
│   └── PortfolioMapper.cs
│
├── Migrations
│
├── Models
│   ├── Asset.cs
│   ├── Portfolio.cs
│   ├── TransactionType.cs
│   └── User.cs
│
├── Services
│   ├── AssetService.cs
│   ├── PortfolioService.cs
│   ├── UserService.cs
│   └── Interfaces
│
└── Program.cs


PortfolioManager.Api.Tests
│
└── Services
    └── AssetServiceTests.cs
```

---

## Modelagem do domínio

A modelagem está sendo evoluída para separar:

```text
Asset
```

do conceito de:

```text
InvestmentTransaction
```

Um `Asset` representa o ativo financeiro em si:

```text
PETR4
VALE3
MXRF11
```

Enquanto uma operação representa um fato ocorrido na carteira:

```text
Compra de 10 PETR4
Preço unitário: R$ 32,50
Data: 17/08/2026
```

A direção arquitetural do projeto é:

```text
User
 │
 └── Portfolio
       │
       └── InvestmentTransaction
              │
              └── Asset
```

As **transações serão a fonte de verdade**, enquanto informações como posição e preço médio poderão ser calculadas a partir do histórico de operações.

---

## Próximas evoluções

### Em desenvolvimento

* [ ] Entidade `InvestmentTransaction`
* [ ] Registro de compras
* [ ] Registro de vendas
* [ ] Validação de posição para vendas
* [ ] Histórico de operações
* [ ] Cálculo de posição
* [ ] Cálculo de preço médio
* [ ] Testes das regras financeiras

### Roadmap técnico

Após consolidar o domínio principal da aplicação, o projeto será utilizado para estudar e aplicar gradualmente conceitos de backend distribuído:

* [ ] PostgreSQL
* [ ] Apache Kafka
* [ ] Producer e Consumer com .NET
* [ ] Worker Service / BackgroundService
* [ ] Processamento assíncrono
* [ ] Idempotência
* [ ] Retry
* [ ] Dead Letter Topic
* [ ] Outbox Pattern
* [ ] Docker
* [ ] Logs estruturados
* [ ] Métricas e health checks
* [ ] CI/CD
* [ ] Cloud

> As tecnologias desta seção representam **roadmap de estudo e evolução** e ainda não fazem parte da versão atual da aplicação.

---

## Arquitetura futura para processamento assíncrono

Uma das evoluções planejadas é utilizar eventos de domínio para processamentos que não precisam ocorrer durante a requisição HTTP.

Exemplo:

```text
POST /transactions
       │
       ▼
ASP.NET Core API
       │
       ├── valida operação
       │
       ├── persiste transação
       │
       ▼
InvestmentTransactionCreated
       │
       ▼
     Kafka
       │
       ▼
.NET Worker
       │
       ▼
Atualização de projeções
```

A intenção não é adicionar complexidade apenas por tecnologia, mas estudar cenários onde processamento assíncrono, desacoplamento e escalabilidade realmente façam sentido.

---

## Executando localmente

### Pré-requisitos

* .NET SDK
* Git

Clone o repositório:

```bash
git clone https://github.com/winnie-s3/portfolio-manager-api.git
```

Entre na pasta:

```bash
cd portfolio-manager-api
```

Restaure as dependências:

```bash
dotnet restore
```

---

## Configurando o JWT

Por segurança, a chave utilizada para assinatura do JWT **não é armazenada no repositório**.

Entre na pasta do projeto da API:

```bash
cd PortfolioManager.Api
```

Inicialize o User Secrets, caso necessário:

```bash
dotnet user-secrets init
```

Configure uma chave local:

```bash
dotnet user-secrets set "Jwt:Key" "SUA_CHAVE_SEGURA_COM_PELO_MENOS_32_CARACTERES"
```

---

## Banco de dados

A versão atual utiliza **SQLite**.

Aplique as migrations:

```bash
dotnet ef database update
```

O arquivo local do banco de dados não é versionado no Git.

---

## Executando a API

```bash
dotnet run
```

Após iniciar a aplicação, acesse o Swagger pela URL exibida no terminal.

O Swagger permite visualizar e testar os endpoints disponíveis.

---

## Executando os testes

Na raiz da solução:

```bash
dotnet test
```

Os testes são executados separadamente da API e utilizam banco em memória nos cenários unitários.

---

## Decisões técnicas

### Por que DTOs?

Para evitar que as entidades persistidas sejam utilizadas diretamente como contratos HTTP e permitir maior controle sobre entrada e saída de dados.

### Por que Services?

Para manter regras de negócio fora dos Controllers e melhorar separação de responsabilidades, testabilidade e manutenção.

### Por que Dependency Injection?

Para reduzir acoplamento entre componentes e permitir que dependências sejam fornecidas externamente às classes.

### Por que JWT?

Para implementar autenticação stateless nos endpoints da API.

### Por que `decimal` para valores financeiros?

Valores monetários exigem precisão decimal. Tipos binários como `float` e `double` podem introduzir erros de representação inadequados para cálculos financeiros.

### Por que separar Asset de Transaction?

Porque o ativo representa **o que existe no mercado**, enquanto a transação representa **o que aconteceu na carteira do usuário**.

Essa separação permite manter histórico e calcular informações derivadas posteriormente.

---

## Segurança

Algumas práticas adotadas:

* senhas armazenadas com hash utilizando BCrypt;
* autenticação JWT;
* chave JWT fora do código-fonte;
* uso de .NET User Secrets no desenvolvimento;
* arquivos locais de banco excluídos do Git;
* separação progressiva dos dados por usuário;
* DTOs para controlar dados expostos pela API.

---

## Objetivo de aprendizado

Além da construção da aplicação, este projeto tem como objetivo consolidar conhecimentos relacionados a:

* arquitetura backend;
* APIs REST;
* modelagem de domínio;
* regras financeiras;
* persistência;
* autenticação e autorização;
* testes automatizados;
* refatoração;
* qualidade de código;
* arquitetura orientada a eventos;
* sistemas distribuídos.

O desenvolvimento é feito de forma incremental, buscando compreender **os motivos e trade-offs por trás das decisões técnicas**, e não apenas adicionar tecnologias ao projeto.

---

## Status

🟡 **Em desenvolvimento**

A fundação da API está implementada e o domínio está sendo evoluído para suportar operações financeiras e cálculos derivados.

---

## Autor

**Winnie Silva**

Desenvolvedora .NET focada em backend, APIs, sistemas financeiros e evolução contínua em arquitetura de software.

GitHub: [@winnie-s3](https://github.com/winnie-s3)

---

⭐ Este projeto está sendo desenvolvido continuamente como parte do meu portfólio técnico e aprofundamento em desenvolvimento backend com .NET.
