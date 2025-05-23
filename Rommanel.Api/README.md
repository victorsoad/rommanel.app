# 📝 CustomerApp - Desafio Técnico

## ✅ Descrição

Este projeto é uma **API RESTful** para gerenciamento de clientes, construída com **.NET 8**, seguindo os princípios de **Clean Architecture** e aplicando boas práticas de desenvolvimento como **CQRS** e **MediatR**.

---

## 🚀 Tecnologias Utilizadas e Por Quê

### ✅ .NET 8
- **Por quê:** Última versão estável, com melhorias de performance e novos recursos.
- **Uso:** Backend da API, com suporte nativo ao C# 12 e recursos modernos.

---

### ✅ Clean Architecture
- **Por quê:** Mantém o projeto organizado, separando regras de negócio da infraestrutura.
- **Uso:** Separação em camadas: Domain, Application, Infrastructure e API.

---

### ✅ CQRS (Command Query Responsibility Segregation)
- **Por quê:** Separar operações de escrita (Commands) das de leitura (Queries) facilita manutenção e testes.
- **Uso:** Implementação via MediatR, com Handlers distintos para cada operação.

---

### ✅ MediatR
- **Por quê:** Facilita a implementação de CQRS, promovendo um código desacoplado.
- **Uso:** Gerenciamento centralizado de comandos e queries.

---

### ✅ FluentValidation
- **Por quê:** Validação robusta e expressiva dos dados recebidos na API.
- **Uso:** Garantia que dados inválidos não prossigam para a lógica de negócios.

---

### ✅ Entity Framework Core + SQLite
- **Por quê:** ORM moderno e produtivo, com banco de dados leve e fácil de configurar.
- **Uso:** Persistência de dados utilizando **SQLite** em modo **arquivo (.db)**, para facilitar o versionamento e simplificar a entrega do projeto.

---

### ✅ Docker
- **Por quê:** Garantir que qualquer pessoa consiga rodar o projeto de forma padronizada, independente do sistema operacional.
- **Uso:** Contêineriza a aplicação e configura facilmente o ambiente via **docker-compose**.

---

### ✅ Swagger (Swashbuckle)
- **Por quê:** Facilitar a documentação e testes manuais da API.
- **Uso:** Disponibiliza uma interface gráfica para testar todos os endpoints.

---

## 🏗️ Estrutura do Projeto

