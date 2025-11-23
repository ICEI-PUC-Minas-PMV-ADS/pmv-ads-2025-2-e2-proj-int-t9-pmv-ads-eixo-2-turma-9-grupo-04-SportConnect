# Especificações do Projeto

<span style="color:red">Pré-requisitos: <a href="1-Documentação de Contexto.md"> Documentação de Contexto</a></span>

Definição do problema e ideia de solução a partir da perspectiva do usuário. É composta pela definição do  diagrama de personas, histórias de usuários, requisitos funcionais e não funcionais além das restrições do projeto.

Apresente uma visão geral do que será abordado nesta parte do documento, enumerando as técnicas e/ou ferramentas utilizadas para realizar a especificações do projeto

## Apresentação do Projeto

https://github.com/user-attachments/assets/d8b0c424-748c-4ad3-a415-d52c8d043d3c

## Personas

### Persona 1
<img width="856" height="510" alt="Giovanna Elisa Pereira" src="https://github.com/user-attachments/assets/ad23b670-6a97-4c6c-bb42-f78b250a0fcd" />

### Persona 2 
<img width="856" height="510" alt="Pedro Souza dos Reis" src="https://github.com/user-attachments/assets/003ad835-bee8-4c7b-ba91-9de2bf78ae9f" />

### Persona 3
<img width="856" height="510" alt="Jorge Henrique Costa" src="https://github.com/user-attachments/assets/e12b74e2-f0d3-4ddf-94f3-b2904bf91d80" />

### Persona 4
<img width="856" height="510" alt="Priscila Martins Oliveira" src="https://github.com/user-attachments/assets/ada623aa-dbc5-49ab-a6a2-15daeffe7beb" />

### Persona 5
<img width="856" height="510" alt="William Porter Ferreira" src="https://github.com/user-attachments/assets/c798befb-d54c-4c60-96b2-982f9521a13e" />

### Persona 6
<img width="856" height="510" alt="Tereza Castro Rodriguez" src="https://github.com/user-attachments/assets/663734a6-e9be-42e0-90cc-cb9affe61a5e" />

## Histórias de Usuários

Com base na análise das personas foram identificadas as seguintes histórias de usuários:

|EU COMO... `PERSONA`| QUERO/PRECISO ... `FUNCIONALIDADE` |PARA ... `MOTIVO/VALOR`                 |
|--------------------|------------------------------------|----------------------------------------|
|Giovanna  | Participar de aulas de dança fitness em grupo          |Me exercitar de forma divertida e criar vínculos sociais               |
|Pedro      | Participar de jogos de futebol com grupos próximos               | Me manter ativo e conhecer novas pessoas |
|Jorge  | Encontrar grupos que se adaptem ao meu nível e tempo livre          | Aliviar o estresse sem comprometer meu dia          
|Priscila  |Praticar yoga e meditação em grupo          | Equilibrar mente e corpo, reduzindo o estresse        |
|William      | Achar amigos para treinar skate e funcionais ao ar livre               | Evoluir nas habilidades e me divertir ||
|Tereza      | Encontrar caminhadas e atividades leves para minha idade                |Cuidar da saúde, evitar lesões e socializar |


## Requisitos

As tabelas que se seguem apresentam os requisitos funcionais e não funcionais que detalham o escopo do projeto.

### Requisitos Funcionais

|ID    | Descrição do Requisito  | Prioridade |
|------|-----------------------------------------|----|
|RF-01| A aplicação deve permitir o cadastro de usuários (CRUD) | ALTA | 
|RF-02 | A aplicação deve permitir o login de usuários, para assim ser possível o acesso ao sistema     | ALTA |
|RF-03 | A aplicação deve permitir a criação de grupos (CRUD) | ALTA |
|RF-04 | A aplicação deve permitir a entrada de usuários em grupos   | ALTA |
|RF-05| A aplicação deve permitir a pesquisa de grupos, seja por nome, localidade ou modalidade | MÉDIA |
|RF-06| A aplicação deve permitir que o usuário saia de um grupo em que ele é participante | MÉDIA |
|RF-07| A aplicação deve permitir que o usuário administrador de um grupo cadastre uma atividade no grupo em que ele gere | ALTA |
|RF-08| A aplicação deve permitir que o usuário receba notificações sobre novas atividades adicionadas aos grupos onde o mesmo participa | BAIXA |
|RF-09| A aplicação deve permitir que o usuário consulte todos os grupos em que ele participa   | BAIXA |
|RF-10| A aplicação deve permitir que o usuário consulte todos os grupos em que ele é administrador  | BAIXA |
|RF-11| A aplicação deve permitir que o usuário entre em uma lista de espera caso o grupo atinja sua capacidade máxima e o administrador tenha habilitado essa opção  | BAIXA |
|RF-12| A aplicação deve permitir que o administrador de um grupo faça o download de um PDF contendo todos os participantes do grupo em que o mesmo gere  | BAIXA |
|RF-13| A aplicação deve permitir que o administrador remova participantes de seu grupo  | MÉDIA |
|RF-14| A aplicação deve permitir que o usuário consulte o endereço de uma atividade por meio de um mapa | MÉDIA |

### Requisitos não Funcionais

|ID     | Descrição do Requisito  |Prioridade |
|-------|-------------------------|----|
|RNF-01| A aplicação deve ser responsiva, sendo possível acessá-la a partir da maioria dos dispositivos disponíveis | ALTA | 
|RNF-02| A aplicação deve possuir um sistema de autenticação e autorização, a fim de evitar acesso de usuários não autorizados |  ALTA | 
|RNF-03| A aplicação deve possuir integrações com APIs externas  |  BAIXA | 
|RNF-04| A aplicação deve possuir mensagens de erros e sucessos claras, para assim proporcionar uma melhor experiência de usuário |  BAIXA | 

## Restrições

O projeto está restrito pelos itens apresentados na tabela a seguir.

|ID| Restrição                                             |
|--|-------------------------------------------------------|
|01| O projeto deverá ser entregue até o fim do semestre, respeitando o cronograma estabelecido |
|02| O front-end da aplicação deverá ser desenvolvido utilizando HTML, CSS e Javascript        |
|03| A linguagem que deve ser utilizada na construção do back-end deverá ser o C#      |
|04| A aplicação deve estar em conformidade com a Lei Geral de Proteção de Dados (LGPD) do Brasil, garantindo a segurança e privacidade dos dados dos usuários       |

## Diagrama de Casos de Uso
<img alt="Diagrama de Casos de Uso" src="https://github.com/ICEI-PUC-Minas-PMV-ADS/pmv-ads-2025-2-e2-proj-int-t9-pmv-ads-eixo-2-turma-9-grupo-04-SportConnect/blob/a4e5c17aed74bd79b1f0d814343f381b32e50680/docs/img/DiagramUC.png"/>
O diagrama de casos de uso é o próximo passo após a elicitação de requisitos, que utiliza um modelo gráfico e uma tabela com as descrições sucintas dos casos de uso e dos atores. Ele contempla a fronteira do sistema e o detalhamento dos requisitos funcionais com a indicação dos atores, casos de uso e seus relacionamentos. 

As referências abaixo irão auxiliá-lo na geração do artefato “Diagrama de Casos de Uso”.

> **Links Úteis**:
> - [Criando Casos de Uso](https://www.ibm.com/docs/pt-br/elm/6.0?topic=requirements-creating-use-cases)
> - [Como Criar Diagrama de Caso de Uso: Tutorial Passo a Passo](https://gitmind.com/pt/fazer-diagrama-de-caso-uso.html/)
> - [Lucidchart](https://www.lucidchart.com/)
> - [Astah](https://astah.net/)
> - [Diagrams](https://app.diagrams.net/)
