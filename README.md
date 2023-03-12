### SITE DE RECEITAS ###

### Running ###
* Executar todas as aplicações com o comando `dotnet run`.

### Pastas ###

#### ClassificaCategoriaGPTQueue ####
* Contém o recebimento da fila do RabbitMQ onde faz uma requisição para a API do chatGPT e classifica a receita postada, e se não se enquadrar em nenhuma das categorias ele coloca o status da receita 0, o qual não será exibido na página inicial.
![ChatGPT](/imagensRedme/RedmeGPT.png)

* Também faz uma outra requisição para o chatGPT onde pergunta quantas calorias aproximadas tem os ingredientes da receita e salva no banco de dados.
![Modificar](/imagensRedme/ModificaReceitaQueue.jpg)


#### SalvaReceitaQueue ####
* Recebe a mensagem na fila do RabbitMQ e também cria uma mensagem para a fila do chatGPT.
![FilaGPT](/imagensRedme/PublicaFilaGPT.jpg)

* Salva as informações no banco de dados, para que nos próximos passos possam ser alteradas.
![SalvaUsuario](/imagensRedme/SalvaUsuarioeReceitaQUEUE.jpg)


#### createAPI ####
* Cria a primeira fila de mensagem do RabbitMQ através de uma API com os campos recebidos do formulário de cadastro de receitas do Razor Page.
![CriaFilaQueue](/imagensRedme/createAPIFilaQueue.jpg)

* Envia para a fila através de um post: Email, NomeReceita, TempoPreparo, Porções, Ingredientes, Descrição e pega o IPv4 do usuário que postou a receita.

#### coreApi ####
* A coreApi é onde fica a API do banco de dados, com as seguintes tabelas: Receitas e Usuários.
* Tem o `get` ajustado para fazer uma busca por todas as receitas ou somente por status que for requisitado.
![getApi](/imagensRedme/coreAPIget.jpg)

#### razor ####

##### Funcionalidades do Razor #####

###### Página inicial ######
Recebe um `fetch` da API para realizar uma busca em todas as receitas que têm o status 1, no caso somente as receitas que estão aprovadas para o usuário ver. Dentro da página inicial pode-se fazer uma busca por nome de receita, 
onde ela pega o campo, transforma tudo em minúsculo e faz uma busca por `includes`, que no caso busca todas as receitas com algo assim no nome, não precisando colocar o nome específico para realizar a busca, e caso 
nenhum nome corresponda ele exibe na tela que não encontrou nada com aquele nome.
![Incial](/imagensRedme/SitePaginaInicial.jpg)
![pesquisa](/imagensRedme/busca.jpg)

###### Página Cadastrar Receitas ######
Tem um formulário que faz uma validação de campos com quantidade mínima para ser preenchidos e também modifica o texto da label do campo para informar o que está acontecendo.
Faz um `post` para a createAPI com os seguintes campos: "email", "nomeReceita", "tempoPreparo", "porções", "ingredientes", "descrição".
![formuario](/imagensRedme/cadastroReceita.jpg)

#### Imagens ####
* As imagens que estão contidas no repositório são da modelagem do banco de dados e dos sistemas.
![banco](/ModelSQLogico.png)
![sistema](/ModelagemVersão2.png)

### Futuras melhorias ###
* Fazer ajustes na tabela usuário para mostrar mais informações.
* Colocar tudo em servidores para rodar.
* Fazer um melhoria na tabela Usuarios.
* Implementar mais funcionalidades para o Razor Pages.
* Melhoras o css da pagina.
