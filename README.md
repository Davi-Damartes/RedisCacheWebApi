# RedisWebApi: Web API Utilizando Redis e Banco de Dados PostgreSQL

O RedisWebApi é um projeto que exemplifica a integração de uma Web API com caching usando Redis e persistência de dados com PostgreSQL. Este projeto foi desenvolvido para demonstrar como utilizar o Entity Framework Core (EFCore) para interagir com o PostgreSQL, e como implementar um sistema de cache eficiente através de um container Docker com a imagem Redis.

## 🖼 Demonstração  

## Tecnologias utilizadas
- ASP.NET Core: Framework para construir a Web API.
- Entity Framework Core (EFCore): ORM para interagir com o banco de dados PostgreSQL.
- PostgreSQL: Banco de dados relacional utilizado para persistência dos dados.
- Redis: Sistema de cache em memória, executado através de um container Docker.
- Docker: Para gerenciar o container do Redis, facilitando a configuração e a execução.

## ⚙️🛠 Ferramentas e Bibliotecas Utilizadas  
- Docker Desktop
- Docker image: Redis 
- Microsoft.EntityFrameworkCore 8.4
- StackExchange.Redis 
- Npgsql.EntityFrameworkCore.PostgreSQL 8.4














# Rodando o projeto na seu computador localmente.

Clone o projeto

```bash
  git clone https://link-para-o-projeto
```

Entre no diretório do projeto

```bash
  cd my-project
```

Instale as dependências que estão citados mais acima
```bash
  dotnet add package "Nomes.Dos.Pacotes"
```

Faça a migrations criação do Banco de Dados com tabelas e relacionamentos:
```bash
  Add-migration "Iniciando Migration" -Context AppDbContext
  Update-Database -Context AppDbContext
```

Faça o download do "docker Desktop" e dentro do seu projeto rode esse comando para baixar a imagen do Redis e rodar o container: 
```bash
  docker run -run -d -p 6379:6379 --name RedisNome redis
```
Após isso o container estará rodando agora basta iniciar a aplicação com: 

```bash
  "dotnet watch run" ou clicando em "F5" dentro da sua IDE.
```

## Observação!!! É necessário que ambos container Redis e o Banco de dados estejam rodando para aplicação Funcionar.
