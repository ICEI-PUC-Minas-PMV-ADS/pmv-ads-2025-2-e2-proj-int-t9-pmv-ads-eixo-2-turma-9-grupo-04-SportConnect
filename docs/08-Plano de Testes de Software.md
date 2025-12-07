# Plano de Testes de Software

## CT-01: Cadastrar Usuário

| **Caso de Teste** 	| **CT01 – Cadastrar Usuário** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-01	A aplicação deve permitir o cadastro de usuários (CRUD)|
| Objetivo do Teste 	| Verificar se o usuário consegue se cadastrar na aplicação. |
| Passos 	| - Acessar o navegador <br>- Informar o endereço do site https://sportconnect.site<br>- Clicar em "Criar conta" <br> - Preencher os campos obrigatórios (e-mail, nome, sobrenome, celular, CPF, senha, confirmação de senha) <br>- Aceitar os termos de uso <br> - Clicar em "Registrar" |
|Critério de Êxito | - O cadastro foi realizado com sucesso. |
|  	|  	|

## CT-02: Efetuar login

| **Caso de Teste** 	| **CT02 – Efetuar login**	|
|:---:	|:---:	|
|Requisito Associado | RF-02	A aplicação deve permitir o login de usuários, para assim ser possível o acesso ao sistema	<br>RF-04	A aplicação deve permitir a entrada de usuários em grupos	 |
| Objetivo do Teste 	| Verificar se o usuário consegue realizar login. |
| Passos 	| - Acessar o navegador <br>- Informar o endereço do site https://adota-pet.herokuapp.com/src/index.html<br>- Clicar no botão "Entrar" <br>- Preencher o campo de e-mail <br>- Preencher o campo da senha <br> - Clicar em "Login" |
|Critério de Êxito | - O login foi realizado com sucesso. |
|  	|  	|

## CT-03: Criar Grupos

| **Caso de Teste** 	| **CT03 – Criar Grupos**	|
|:---:	|:---:	|
|Requisito Associado | RF-03	A aplicação deve permitir a criação de grupos (CRUD)<br>RF-04	A aplicação deve permitir a entrada de usuários em grupos<br>RF-07	A aplicação deve permitir que o usuário administrador de um grupo cadastre uma atividade no grupo em que ele gere |
| Objetivo do Teste 	| Verificar se o usuário consegue criar grupos no sistema. |
| Passos 	|- Acessar o navegador <br>- Informar o endereço do site : SportConnect.site<br>- Clicar em "Login" na barra de navegação <br>- inserir as credenciais de acesso <br>- Clicar em "Login"<br>- Aba de grupos<br>- Criar grupo |
|Critério de Êxito |  - O grupo foi criado com sucesso. |
|  	|  	|

## CT-04: Pesquisar Grupos

| **Caso de Teste** 	| **CT04 – Pesquisar Grupos**	|
|:---:	|:---:	|
|Requisito Associado | RF-05	A aplicação deve permitir a pesquisa de grupos, seja por nome, localidade ou modalidade. |
| Objetivo do Teste 	| Verificar se o usuário consegue pesquisar por grupos no sistema seja por nome, localidade ou modalidade. |
| Passos 	|- Acessar o navegador<br- Informar o endereço do site : SportConnect.com.br<br- Clicar em "Login" na barra de navegação<br>- Enserir as credenciais de acesso.<br>- Clicar em "Login"<br>- Aba de grupos<br>- Pesquisar por grupo|
|Critério de Êxito | - O usuário encontra o grupo de acordo com sua preferências. |
|  	|  	|

## CT-05: Sair do Grupo

| **Caso de Teste** 	| **CT05 – Sair do Grupo**	|
|:---:	|:---:	|
|Requisito Associado | RF-06	A aplicação deve permitir que o usuário saia de um grupo em que ele é participante. |
| Objetivo do Teste 	| Verificar se o usuário consegue sair de um grupo no qual ele participa |
| Passos 	|- Acessar o navegador<br>- Informar o endereço do site : SportConnect.com.br<br>- Clicar em "Login" na barra de navegação<br>- Enserir as credenciais de acesso.<br>- Clicar em "Login"<br>- Aba de grupos<br>- Meus grupos<br>- Seleciona o grupo no qual deseja sair<br>- Confirmar em sair do grupo.|
|Critério de Êxito | -O usuário consegue sair de grupo no qual ele participa.  |
|  	|  	|

## CT-06: Receber Notificações

| **Caso de Teste** 	| **CT06 – Receber Notificações**	|
|:---:	|:---:	|
|Requisito Associado | RF-08	A aplicação deve permitir que o usuário receba notificações sobre novas atividades adicionadas aos grupos onde o mesmo participa. |
| Objetivo do Teste 	| Verificar se o usuário recebe notificações de um grupo no qual ele participa. |
| Passos 	|- Acessar o navegador<br>- Informar o endereço do site : SportConnect.com.br<br>- Clicar em "Login" na barra de navegação<br>- Enserir as credenciais de acesso.<br>- Clicar em "Login"<br>- Aba de notificações, exibe se há notificações |
|Critério de Êxito | - O usuário recebe notificações de grupos em que ele participa.  |
|  	|  	|

## CT-07: Consultar Grupos

| **Caso de Teste** 	| **CT07 – Consultar Grupo**	|
|:---:	|:---:	|
|Requisito Associado | RF-09	A aplicação deve permitir que o usuário consulte todos os grupos em que ele participa.<br>RF-10	A aplicação deve permitir que o usuário consulte todos os grupos em que ele é administrador. |
| Objetivo do Teste 	| Verificar se o usuário pode consultar  o/os grupo no qual ele participa. |
| Passos 	|- Acessar o navegador<br>- Informar o endereço do site : SportConnect.com.br<br>- Clicar em "Login" na barra de navegação<br>- Enserir as credenciais de acesso.<br>- Clicar em "Login" <br>- Aba de grupos<br>- Meus grupos |
|Critério de Êxito | - O usuário consegue consultar o/os grupo no qual ele participa.  |
|  	|  	|

## CT-08: Lista de Espera

| **Caso de Teste**	| **CT08 – Lista de Espera**	|
|:---:	|:---:	|
|Requisito Associado | RF-11	A aplicação deve permitir que o usuário entre em uma lista de espera caso o grupo atinja sua capacidade máxima e o administrador tenha habilitado essa opção.<br>RF-04	A aplicação deve permitir a entrada de usuários em grupos. |
| Objetivo do Teste 	| Verificar se o usuário pode entrar em uma lista de espera caso o grupo já esteja cheio. |
| Passos 	|- Acessar o navegador<br>- Informar o endereço do site : SportConnect.com.br<br>- Clicar em "Login" na barra de navegação<br>- Enserir as credenciais de acesso.<br>- Clicar em "Login"<br>- Aba de grupos<br>- Seleciona um grupo no qual quer fazer parte.<br>- Verifica se há lista de espera para aquele grupo.<br>- Entrar na lista de espera.|
|Critério de Êxito | - O usuário consegue entrar em uma lista de espera se caso o grupo já estiver cheio. |
|  	|  	|

## CT-09: PDF com os participantes
| **Caso de Teste** 	| **CT09 – PDF com os participantes**	|
|:---:	|:---:	|
|Requisito Associado | RF-10	A aplicação deve permitir que o usuário consulte todos os grupos em que ele é administrador.<br>RF-12	A aplicação deve permitir que o administrador de um grupo faça o download de um PDF contendo todos os participantes do grupo em que o mesmo gere.|
| Objetivo do Teste 	| Verificar se o usuário Administrador de um grupo pode fazer o dowload de um PDF com todos os participantes do grupo. |
| Passos 	|- Acessar o navegador<br>- Informar o endereço do site : SportConnect.com.br<br>- Clicar em "Login" na barra de navegação<br>- Enserir as credenciais de acesso.<br>- Clicar em "Login"<br>- Aba de grupos<br>- Meus grupos<br>- Administrar grupo<br>- Gerar PDF contendo todos os participantes do seu grupo. |
|Critério de Êxito | - O usuário consegue gerar um PDF contendo todos os participantes do seu grupo. |
