# Registro de Testes de Software

| **Caso de Teste** 	| **CT01 – Cadastrar perfil** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-01 - A aplicação deve permitir o cadastro de usuários (CRUD)|
|Registro de evidência | [www.teste.com.br/drive/ct-01](https://github.com/user-attachments/assets/99986ccd-a683-4feb-8c22-7a082d0e1686) |


| **Caso de Teste** 	| **CT02 – Realizar login** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-02 A aplicação deve permitir o login de usuários, para assim ser possível o acesso ao sistema |
|Registro de evidência | [www.teste.com.br/drive/ct-02](https://github.com/user-attachments/assets/7a7bb921-2da9-4608-b7c6-718104954fe1) |


| **Caso de Teste** 	| **CT03 – Criar grupos** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-03 A aplicação deve permitir a criação de grupos (CRUD) |
|Registro de evidência | [www.teste.com.br/drive/ct-03](https://github.com/user-attachments/assets/84be05b7-27bf-4bf9-b4eb-3a32f641bc6d) |


| **Caso de Teste** 	| **CT04 – Pesquisar grupos** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-05 A aplicação deve permitir a pesquisa de grupos, seja por nome, localidade ou modalidade. |
|Registro de evidência | [www.teste.com.br/drive/ct-04](https://github.com/user-attachments/assets/bc26e5f8-6ca4-470a-8d71-311959125be8) |


| **Caso de Teste** 	| **CT05 – Sair do grupo** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-06 A aplicação deve permitir que o usuário saia de um grupo em que ele é participante. |
|Registro de evidência | [www.teste.com.br/drive/ct-05](https://github.com/user-attachments/assets/262bf050-a51f-476d-ada6-b1f1027022aa) |

| **Caso de Teste** 	| **CT06 – Receber notificação** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-08 A aplicação deve permitir que o usuário receba notificações sobre novas atividades adicionadas aos grupos onde o mesmo participa. |
|Registro de evidência | [www.teste.com.br/drive/ct-06](https://github.com/user-attachments/assets/25e3fe71-b6f9-4c02-a0d8-21b83b0b70eb) |

| **Caso de Teste** 	| **CT07 – Consultar grupo** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-09 A aplicação deve permitir que o usuário consulte todos os grupos em que ele participa. <br> RF-10 A aplicação deve permitir que o usuário consulte todos os grupos em que ele é administrador. |
|Registro de evidência | [www.teste.com.br/drive/ct-07](https://github.com/user-attachments/assets/d4876f13-2a16-4d88-bc70-079aed5634f7) |


| **Caso de Teste** 	| **CT08 – Lista de espera** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-11 A aplicação deve permitir que o usuário entre em uma lista de espera caso o grupo atinja sua capacidade máxima e o administrador tenha habilitado essa opção. <br> RF-04 A aplicação deve permitir a entrada de usuários em grupos. |
|Registro de evidência | [www.teste.com.br/drive/ct-08](https://github.com/user-attachments/assets/e27760f8-c00f-49c9-abad-337b355b2262) |


| **Caso de Teste** 	| **CT09 – PDF com os participantes** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-10 A aplicação deve permitir que o usuário consulte todos os grupos em que ele é administrador. <br> RF-12 A aplicação deve permitir que o administrador de um grupo faça o download de um PDF contendo todos os participantes do grupo em que o mesmo gere. |
|Registro de evidência | [www.teste.com.br/drive/ct-09](https://github.com/user-attachments/assets/f8b038eb-f3d7-496e-b5dd-ed9d063e463a) |


## Relatório de testes de software

A equipe de testes focou na avaliação nos requisitos funcionais já implementados: o cadastro de usuários (RF-01), o login (RF-02) e a criação de grupos (RF-03). Os demais requisitos do projeto ainda não foram desenvolvidos e, por isso, não foram avaliados.
De modo geral, os testes realizados apresentaram um desempenho positivo, confirmando que o sistema atende às necessidades principais de acesso e gerenciamento inicial de grupos. As funções de cadastro e login (RF-01 e RF-02) funcionaram corretamente, e a validação dos dados obrigatórios está em conformidade. Além disso, a funcionalidade de criação de grupos (RF-03) foi executada sem falhas.
A consulta dos grupos administrados também teve um desempenho satisfatório, exibindo os grupos vinculados ao perfil do usuário. Em relação à facilidade de uso, o design da interface é simples e direto, facilitando a utilização.

Apesar dos resultados positivos, os testes mostraram falhas que exigem atenção, especialmente nas áreas de segurança e na experiência do usuário. 
No quesito segurança, foi identificada a ausência de um limite de tentativas de login mal-sucedidas. Isso torna o sistema vulnerável a ataques que tentam descobrir a senha por repetição. Outra falha grave está no processo de recuperação de senha: o sistema permite a troca apenas com a inserção do e-mail, sem exigir uma confirmação adicional (como um código de segurança), o que representa um altíssimo risco de acesso não autorizado.
Quanto à usabilidade, as mensagens de retorno para o usuário são pouco detalhadas, dificultando a compreensão imediata sobre as ações realizadas ou as falhas. Além disso, as listagens de grupos administrados são simples, não oferecendo filtros avançados, o que pode dificultar a busca à medida que o volume de grupos aumentar.

Para solucionar os problemas e aprimorar a aplicação, iremos implementar um mecanismo de bloqueio temporário após um número definido de tentativas incorretas de login para diminuir os riscos de acesso e reformularemos o fluxo de troca de senha para que exija, obrigatoriamente, uma confirmação via e-mail ou outro método de segurança. Já para melhorar a experiência do usuário, revisaremos todas as mensagens de retorno para que se tornem mais informativas, garantindo que elas apresentem confirmações mais claras e visuais.
