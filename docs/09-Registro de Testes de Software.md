# Registro de Testes de Software
Registro dos resultados obtidos durante a validação da aplicação, seguindo o plano de testes previamente definido.

## CT-01: Cadastrar Usuário

| **Caso de Teste** 	| **CT01 – Cadastrar Usuário** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-01 - A aplicação deve permitir o cadastro de usuários (CRUD)|
|Registro de evidência | [www.teste.com.br/drive/ct-01](https://github.com/user-attachments/assets/99986ccd-a683-4feb-8c22-7a082d0e1686) |

## CT-02: Efetuar login

| **Caso de Teste** 	| **CT02 – Efetuar login** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-02 A aplicação deve permitir o login de usuários, para assim ser possível o acesso ao sistema |
|Registro de evidência | [www.teste.com.br/drive/ct-02](https://github.com/user-attachments/assets/7a7bb921-2da9-4608-b7c6-718104954fe1) |

## CT-03: Criar Grupos

| **Caso de Teste** 	| **CT03 – Criar grupos** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-03 A aplicação deve permitir a criação de grupos (CRUD) |
|Registro de evidência | [www.teste.com.br/drive/ct-03](https://github.com/user-attachments/assets/84be05b7-27bf-4bf9-b4eb-3a32f641bc6d) |

## CT-04: Pesquisar Grupos

| **Caso de Teste** 	| **CT04 – Pesquisar grupos** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-05 A aplicação deve permitir a pesquisa de grupos, seja por nome, localidade ou modalidade. |
|Registro de evidência | [www.teste.com.br/drive/ct-04](https://github.com/user-attachments/assets/bc26e5f8-6ca4-470a-8d71-311959125be8) |

## CT-05: Sair do Grupo

| **Caso de Teste** 	| **CT05 – Sair do grupo** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-06 A aplicação deve permitir que o usuário saia de um grupo em que ele é participante. |
|Registro de evidência | [www.teste.com.br/drive/ct-05](https://github.com/user-attachments/assets/262bf050-a51f-476d-ada6-b1f1027022aa) |

## CT-06: Receber Notificações

| **Caso de Teste** 	| **CT06 – Receber notificações** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-08 A aplicação deve permitir que o usuário receba notificações sobre novas atividades adicionadas aos grupos onde o mesmo participa. |
|Registro de evidência | [www.teste.com.br/drive/ct-06](https://github.com/user-attachments/assets/25e3fe71-b6f9-4c02-a0d8-21b83b0b70eb) |

## CT-07: Consultar Grupos

| **Caso de Teste** 	| **CT07 – Consultar grupos** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-09 A aplicação deve permitir que o usuário consulte todos os grupos em que ele participa. <br> RF-10 A aplicação deve permitir que o usuário consulte todos os grupos em que ele é administrador. |
|Registro de evidência | [www.teste.com.br/drive/ct-07](https://github.com/user-attachments/assets/d4876f13-2a16-4d88-bc70-079aed5634f7) |

## CT-08: Lista de Espera

| **Caso de Teste** 	| **CT08 – Lista de espera** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-11 A aplicação deve permitir que o usuário entre em uma lista de espera caso o grupo atinja sua capacidade máxima e o administrador tenha habilitado essa opção. <br> RF-04 A aplicação deve permitir a entrada de usuários em grupos. |
|Registro de evidência | [www.teste.com.br/drive/ct-08](https://github.com/user-attachments/assets/e27760f8-c00f-49c9-abad-337b355b2262) |

## CT-09: PDF com os participantes

| **Caso de Teste** 	| **CT09 – PDF com os participantes** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-10 A aplicação deve permitir que o usuário consulte todos os grupos em que ele é administrador. <br> RF-12 A aplicação deve permitir que o administrador de um grupo faça o download de um PDF contendo todos os participantes do grupo em que o mesmo gere. |
|Registro de evidência | [www.teste.com.br/drive/ct-09](https://github.com/user-attachments/assets/f8b038eb-f3d7-496e-b5dd-ed9d063e463a) |


## Relatório de testes de software

### CT-01: Cadastrar Usuário
| **Caso de Teste** 	| **CT01 – Cadastrar Usuário** 	|
|:---:	|:---:	|
|	Pontos Fortes 	| As informação descritas pelo usuário são cadastradas com sucesso |
| Pontos a Melhorar | No momento do cadastro não há um aviso de que o cadastro foi realizado com sucesso |

### CT-02: Efetuar login
| **Caso de Teste** 	| **CT02 – Efetuar login** 	|
|:---:	|:---:	|
|	Pontos Fortes 	| A sessão do usuário é estabelecida sem falhas  |
| Pontos a Melhorar | Não foi encontrado |

### CT-03: Criar Grupos
| **Caso de Teste** 	| **CT03 – Criar grupos** 	|
|:---:	|:---:	|
|	Pontos Fortes 	| O grupo é cadastrado no sistema com sucesso |
| Pontos a Melhorar | Não há feedbacks por parte da aplicação informando a criação do grupo |

### CT-04: Pesquisar Grupos
| **Caso de Teste** 	| **CT04 – Pesquisar grupos** 	|
|:---:	|:---:	|
|	Pontos Fortes 	| A filtragem dos grupos ocorre normalmente mediante as informações inseridas |
| Pontos a Melhorar | Não foi encontrado |

### CT-05: Sair do Grupo
| **Caso de Teste** 	| **CT05 – Sair do Grupo** 	|
|:---:	|:---:	|
|	Pontos Fortes 	| É possível que o usuário saia do grupo quando ele bem entender. |
| Pontos a Melhorar | Ter um período para o usuário retornar ao grupo em que ele acaba de sair, de forma que ele tenha que esperar um tempo para se reingressar na quele mesmo grupo no qual ele saiu. |

### CT-06: Receber Notificações
| **Caso de Teste** 	| **CT06 – Receber Notificações** 	|
|:---:	|:---:	|
|	Pontos Fortes 	| O usuário recebe notificações referente a sua entrada em um grupo |
| Pontos a Melhorar | O usuário receber notificações ao ser criado um evento no grupo em que ele participa. |

### CT-07: Consultar Grupos
| **Caso de Teste** 	| **CT07 – Consultar Grupos** 	|
|:---:	|:---:	|
|	Pontos Fortes 	| O usuário consegue consultar todos os grupos no qual ele participa e ou admnistra. |
| Pontos a Melhorar | Não foi encontrado. |

### CT-08: Lista de Espera
| **Caso de Teste** 	| **CT08 – Lista de Espera** 	|
|:---:	|:---:	|
|	Pontos Fortes 	| A lista de espera funciona como um fila de forma automatíca.  |
| Pontos a Melhorar | O Administrador daquele grupo não tem conhecimento de quantos estão na fila de espera. |

### CT-09: PDF com os participantes
| **Caso de Teste** 	| **CT09 – PDF com os Participantes** 	|
|:---:	|:---:	|
|	Pontos Fortes 	| É gerado um PDF com todos os participantes do grupo no qual aquele admnistrador gere. |
| Pontos a Melhorar | Não foi encontrado |
