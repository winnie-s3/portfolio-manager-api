# Portfolio Manager

Aplicação full stack para gerenciamento de carteiras de investimentos, desenvolvida com **ASP.NET Core e Angular**.

O projeto nasceu como um estudo prático do ecossistema .NET moderno e vem sendo evoluído para explorar problemas mais próximos de aplicações reais, como **autenticação, autorização por usuário, integrações externas, tratamento de falhas, modelagem financeira e frontend consumindo APIs**.

> 🚧 Projeto em desenvolvimento contínuo.

---

## Sobre o projeto

O **Portfolio Manager** permite que usuários autenticados gerenciem suas próprias carteiras de investimentos.

A aplicação possui frontend em Angular, backend em ASP.NET Core e persistência com Entity Framework Core.

Entre os principais objetivos técnicos do projeto estão:

* isolamento de dados por usuário;
* autenticação e autorização;
* integração com serviços externos;
* tratamento centralizado de erros;
* registro de operações de investimento;
* evolução da modelagem financeira;
* testes automatizados;
* separação de responsabilidades.

---

## Tecnologias

### Backend

* C#
* ASP.NET Core Web API
* Entity Framework Core
* LINQ
* REST APIs
* Dependency Injection
* HttpClient
* Swagger / OpenAPI

### Frontend

* Angular
* TypeScript
* HTML
* CSS
* Angular Router
* HttpClient
* Signals
* Interceptors
* Route Guards

### Banco de dados

* SQLite
* Entity Framework Core Migrations

### Segurança

* JWT
* JWT armazenado em cookie `HttpOnly`
* BCrypt para hash de senhas
* .NET User Secrets para secrets locais
* autorização por recurso e usuário
* CORS configurado para o frontend

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

* cadastro de usuários;
* login;
* hash de senhas com BCrypt;
* geração de JWT;
* armazenamento do JWT em cookie `HttpOnly`;
* validação de sessão através do backend;
* logout;
* proteção de endpoints autenticados.

### Autorização

O backend identifica o usuário através das claims do JWT e valida o acesso aos recursos.

Um usuário autenticado não pode consultar ou alterar carteiras pertencentes a outro usuário.

A segurança não depende apenas do frontend: as verificações são realizadas pela própria API.

### Carteiras

* criação;
* consulta;
* atualização;
* exclusão;
* relacionamento com usuário;
* filtragem das carteiras pelo usuário autenticado.

### Ativos

* cadastro;
* consulta;
* atualização;
* pesquisa;
* paginação;
* validação de acesso através da carteira relacionada.

A exclusão pública de ativos foi removida durante a evolução da modelagem para evitar a remoção indevida do histórico financeiro relacionado.

### Operações de investimento

Foi iniciada a modelagem baseada em operações financeiras.

Atualmente existem operações com informações como:

* carteira;
* ativo;
* tipo da operação;
* quantidade;
* preço unitário;
* data da operação;
* data de criação.

O backend já permite registrar uma operação e consultar uma operação por identificador.

A evolução planejada inclui histórico completo por carteira, validação de vendas, cálculo de posição e preço médio.

---

## Integração com dados de mercado

A aplicação possui integração externa para consulta de preços de ativos.

Exemplo:

```text
GET /api/market-data/PETR4
```

A integração foi isolada através de um contrato interno:

```text
IMarketDataProvider
        │
        ▼
BrapiMarketDataProvider
        │
        ▼
     Brapi API
```

O restante da aplicação não precisa conhecer diretamente:

* URL do fornecedor;
* estrutura específica da resposta;
* detalhes da comunicação HTTP.

A resposta externa é convertida para um modelo interno da aplicação.

Isso reduz o acoplamento com o fornecedor e facilita futuras mudanças na integração.

---

## Tratamento de falhas

A aplicação possui tratamento global de exceções.

Erros conhecidos são convertidos para respostas HTTP controladas, enquanto detalhes técnicos permanecem nos logs da aplicação.

Exemplos tratados:

* recurso inexistente;
* acesso proibido;
* falha em serviço externo;
* erros inesperados.

O objetivo é evitar `try/catch` repetido nos controllers e impedir a exposição desnecessária de detalhes internos para o cliente.

---

## Arquitetura atual

```text
Angular
   │
   ▼
ASP.NET Core API
   │
   ▼
Controllers
   │
   ▼
Services
   │
   ├──────────────► External Integrations
   │
   ▼
Entity Framework Core
   │
   ▼
SQLite
```

Principais áreas do backend:

```text
Controllers/
Services/
Dtos/
Mappings/
Models/
Data/
Configuration/
Integrations/
Migrations/
```

---

## Algumas decisões técnicas

### Por que DTOs?

Para evitar que as entidades persistidas sejam utilizadas diretamente como contratos HTTP e permitir maior controle sobre entrada e saída de dados.

### Por que Services?

Para manter regras e comportamentos da aplicação fora dos controllers.

### Por que abstrair a integração externa?

O sistema precisa apenas saber que consegue consultar o preço de um ativo.

Detalhes como fornecedor, URL e formato da resposta ficam isolados na implementação responsável pela integração.

### Por que JWT em cookie HttpOnly?

Inicialmente o frontend armazenava o JWT em `localStorage`.

A autenticação foi evoluída para utilizar um cookie `HttpOnly`, evitando que o JavaScript da aplicação tenha acesso direto ao token.

O navegador envia o cookie automaticamente nas requisições, enquanto a validação continua sendo responsabilidade do backend.

### User Secrets e HttpOnly resolvem o mesmo problema?

Não.

**.NET User Secrets** protege informações privadas utilizadas pelo backend durante o desenvolvimento, como a chave usada para assinar os JWTs.

**HttpOnly Cookie** protege o token entregue ao navegador, impedindo que o JavaScript da página consiga acessá-lo diretamente.

### Por que verificar o proprietário do recurso?

Autenticação responde:

> Quem está fazendo a requisição?

Autorização responde:

> Esse usuário pode acessar este recurso?

Por isso, apenas utilizar `[Authorize]` não é suficiente para garantir isolamento entre usuários.

---

## Frontend Angular

O frontend oferece atualmente:

* tela de login;
* listagem das carteiras do usuário;
* detalhes da carteira;
* proteção de rotas;
* autenticação integrada ao backend;
* logout;
* interface responsiva.

Fluxo simplificado:

```text
Login
  │
  ▼
ASP.NET Core valida as credenciais
  │
  ▼
JWT é criado
  │
  ▼
Cookie HttpOnly
  │
  ▼
Angular acessa endpoints protegidos
```

Para verificar uma sessão, o frontend consulta o backend em vez de tentar ler o JWT diretamente.

---

## Modelagem do domínio

A direção da modelagem é:

```text
User
 │
 └── Portfolio
       │
       └── InvestmentTransaction
              │
              └── Asset
```

Uma transação representa um fato ocorrido na carteira:

```text
Compra
PETR4
Quantidade: 10
Preço: R$ 32,50
```

A intenção é que o histórico de operações seja a base para informações derivadas como:

* posição atual;
* preço médio;
* custo total;
* valorização.

---

## Executando localmente

### Pré-requisitos

* .NET SDK
* Node.js
* npm
* Git

Clone o repositório:

```bash
git clone https://github.com/winnie-s3/portfolio-manager-api.git
cd portfolio-manager-api
```

### Backend

Entre no projeto:

```bash
cd PortfolioManager.Api
```

Restaure as dependências:

```bash
dotnet restore
```

Configure a chave JWT utilizando User Secrets:

```bash
dotnet user-secrets init
dotnet user-secrets set "Jwt:Key" "SUA_CHAVE_SEGURA"
```

Aplique as migrations:

```bash
dotnet ef database update
```

Execute:

```bash
dotnet run
```

A documentação Swagger ficará disponível no endereço exibido pela aplicação.

### Frontend

Em outro terminal:

```bash
cd PortfolioManager.Web
npm install
npm start
```

O frontend ficará disponível normalmente em:

```text
http://localhost:4200
```

---

## Executando os testes

Na raiz da solução:

```bash
dotnet test
```

---

## Próximas evoluções

* [ ] histórico de operações por carteira;
* [ ] validação de posição disponível em vendas;
* [ ] cálculo de posição;
* [ ] cálculo de preço médio;
* [ ] evolução da modelagem de `Asset`;
* [ ] ampliação dos testes automatizados;
* [ ] integração entre preço de mercado e valorização da carteira;
* [ ] configuração de ambientes e deploy.

### Estudos futuros

Depois da consolidação do domínio principal, o projeto poderá ser usado para explorar:

* PostgreSQL;
* Docker;
* processamento assíncrono;
* mensageria;
* retry e resiliência;
* observabilidade;
* CI/CD;
* cloud.

Esses itens representam **roadmap de estudo** e ainda não fazem parte da implementação atual.

---

## Status

🟡 **Em desenvolvimento**

A aplicação já possui um fluxo full stack funcional com autenticação, autorização por usuário, gerenciamento de carteiras, integração externa de preços e início da modelagem de operações financeiras.

---

## Autor

**Winnie Silva**

Desenvolvedora .NET com foco em backend, APIs, sistemas financeiros e evolução contínua em arquitetura de software.

GitHub: [@winnie-s3](https://github.com/winnie-s3)

---

⭐ Projeto desenvolvido de forma incremental, buscando compreender os motivos e trade-offs por trás das decisões técnicas em vez de apenas adicionar tecnologias.
