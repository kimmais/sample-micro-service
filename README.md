# sample-micro-service
sample-micro-service
Esté repositório é um modelo que deve ser usado para ser base de qualquer micro-serviço que seja criado para o pprojeto Kim+ Benefíciosn.

### Conteúdo
[Visão Geral da Arquitetura](#visao-geral-da-arquitetura)

# Visão Geral da Arquitetura
O projeto é composto pelas camadas Api, Core, Business, Data e Service, cada uma responsável por uma determinada parte do código. Essa arquitetura permite a adição de novas camadas dependendo da demanda de cada micro-serviço.

### Api
Essa camada é  responsável pela criação dos endpoints pelos controller, configuração do startup da aplicação, da aInjeção de dependência e da view models que são responsáveis por fazer as validações mais básicas.
Possui uma classe BaseController com alguns métodos que são comuns entre todos os controllers possibilitando assim padronizar os retornos de sucesse e erro da api.

### Business
Camada responsável por concentrar todos os objetos relacionados ao negócio como as própprias models e as interfaces.

### Data
Camada responsávell por toda lógica relacionada ao banco de dados, como migrations, mappings e contexts.

### Core
Camada responsável por concentrar os componentes que serão utilizados pelas demais camadas.

##
# Service
Camada responsável por conentrar a regra de negócio de toda a aplicação e integrações.

