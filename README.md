# sample-micro-service
sample-micro-service
Esté repositório é um modelo que deve ser usado para ser base de qualquer micro-serviço que seja criado para o pprojeto Kim+ Benefíciosn.

### Conteúdo
[Visão Geral da Arquitetura](#visao-geral-da-arquitetura)

# Visão Geral da Arquitetura
O projeto é composto pelas camadas Api, Core, Business, Data e Service, cada uma responsável por uma determinada parte do código. Essa arquitetura permite a adição de novas camadas dependendo da demanda de cada micro-serviço.

Todo o projeto segue o padrão de contratos, ou seja, nenhum serviço, repository ou componente pode ser criado sem o seu contrato(interface). Isso é importante para que possamos manter o padrão de qualidade, facilitar a injeção de dependência e um código que seja fácil de testar

### Api
Essa camada é  responsável pela criação dos endpoints pelos controller, configuração do startup da aplicação, da aInjeção de dependência e da view models que são responsáveis por fazer as validações mais básicas.
Possui uma classe BaseController com alguns métodos que são comuns entre todos os controllers possibilitando assim padronizar os retornos de sucesse e erro da api.

### Business
Camada responsável por concentrar todos os objetos relacionados ao negócio:
Models
Interfaces

Essa camada servirá de ponte da camada de aplicação as outras camadas que são necessárias um acesso direto

### Data
Camada responsável por toda lógica relacionada ao banco de dados, como migrations, mappings e contexts.
Existe um Repository base com as principais funcionalidades de acesso ao banco: update, get, add, delete e etc que serão utilizados pelos demais repositorys específicos do negócio.

### Core
Camada responsável por concentrar os componentes que serão utilizados pelas demais camadas.
Notificador: Um observable que é utilizado para gerenciar as mensagens de erros relacionadas ao negócio que pode ser acessado de qualquer camada. Cada camada pode realizar sua devida validação, esse componente tem como função gerenciar e padronizar todas as mensagens de erros sendo elas genéricas ou específicas.

### Service
Camada responsável por conentrar a regra de negócio de toda a aplicação e integrações.
Possui um ServiceBase com algumas funcionalidades padrões utilizadas para realizar validações de negócio mais complexas.
Utilizamos o FluentAssertion para fazer toda a regra de validação complexs.

### Tests
Como o próprio nome já diz, utilizado para realizarmos os testes de unidade e integração para garantir a qualidade do código nos dando assim segurança em qualquer manutenção que iremos realizar. A itenção é sempre ter uma cobertura acima de 70% com testes que realmente garantam que a aplicação funcione como esperadop.


