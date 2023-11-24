# Movie Management API

Uma API de Gerenciamento de Filmes construída com .NET Core que permite o gerenciamento de uma coleção de filmes.

## Pré-requisitos

Antes de começar, certifique-se de atender aos seguintes requisitos:

- SDK .NET 5.0 instalado em sua máquina. Você pode baixá-lo [aqui](https://dotnet.microsoft.com/download/dotnet/5.0).
- SQL Server ou outro banco de dados relacional compatível instalado.

## Inicializando o projeto
Para começar com este projeto, siga estas etapas:

1. Clone este repositório em sua máquina local:

   ```bash
   git clone https://github.com/ca-ayumi/MovieManagementAPI.git
   ```

2. Navegue até o diretório do projeto:

    ```bash
    $ cd MovieManagementAPI
    ```
3. Crie um arquivo appsettings.json no diretório do projeto MovieManagementAPI com a string de conexão do seu banco de dados:

      ```bash
    {
    "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=MovieManagementDB;User Id=sa;Password=<Coxinha123>;"    }
      }
      ```
4. Execute as migrações do banco de dados para criar o banco de dados:

    ```bash
    $ dotnet ef database update
    ```
5. Compile e execute a aplicação:

    ```bash
    $ dotnet build
    $ dotnet run
    ```
6. A API deve estar rodando localmente em https://localhost:5001 (HTTPS) e http://localhost:5000 (HTTP).

## Documentação da API

A documentação da API pode ser acessada em https://localhost:5001/swagger/index.html (HTTPS) e http://localhost:5000/swagger/index.html (HTTP).






