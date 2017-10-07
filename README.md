# APIZeroAnomaly

 - Cadastro e configuração de sensores
 - Cadastro de rede de sensores
 - Inserção de dados de sensores
 - Tratamento individual de dados
 - Tratamento em tempo real baseado na rede de sensores
 - Banco NoSql

## Dependências

 - MongoDB um banco de dados NoSql. Para instalá-lo: [Documentação MongoDB](https://docs.mongodb.com/tutorials/).
 - Visual Studio 2017 ferramenta para compilação da API: [Documentação Visual Studio 2017](https://docs.microsoft.com/en-us/visualstudio/).

##Configurando a API

Para utilizar a API precisam ser configuradas algumas coisas antes, começando pelo banco de dados. É necessário criar a DataBase com o nome de dados e as Coleções de acordo com o que a API espera, então é necessário cria-lás com os seguintes nomes: configSensor (coleção onde é armazenado o cadastro e configuração dos sensores), dadosSensor (coleção onde são armazenados os dados) e sensorRede (coleção onde são armazenados as redes de sensores).

Após isto, seguir os passos de:
 - Realizar o cadastro dos sensores;
 - Realizar o cadastro das redes dos sensores;
 - Inserir dados para os sensores (No mínimo  10 dados para cada sensor que queira fazer tratamento correto);

## Expondo modelo da coleção no banco de dados MongoDB
 - configSensor (
 	_id ObjectId,			  --Identificador do banco
 	IdSensor int,			  --Identificador do sensor
 	Descricao string,         --Nome do sensor
 	Min double,				  --Valor mínimo  aceito para tratamento da anomalia nos dados
 	Max double,				  --Valor máximo aceito para tratamento de anomalia nos dados
 	UnidadeMedida string,	  --Tipo de unidade de medida do sensor
 	Metodo string,			  --Qual método utilizar para tratar anomalia (No momento só existe o Range)
 	VizinhoPadrao int,		  --Quantidade de vizinhos para frente e para trás do dado atual a ser tratado
 	Data ISODate			  --Data para histórico
 	);
 - dadosSensor (
 	_id ObjectId,			  --Identificador do banco
 	IdSensor int,			  --Identificador do sensor
 	Valor double,			  --Dado enviado pelo sensor
 	ValorOriginal,			  --Dado enviado pelo sensor sem tratamento
 	Data ISODate			  --Data para histórico
 	);
 - sensorData (
 	_id ObjectId,			  --Identificador do banco
 	IdRede int,				  --Identificador da Rede
 	IdSensor int,			  --Identificador do sensor
 	Data ISODate			  --Data para histórico
 	).

## Como realizar Testes

Para testar utilize uma interface que se comunique com a API ou utilize o Postman importando todas as rotas já prontas disponíveis abaixo:

 - [Rota para configuração dos sensores](https://documenter.getpostman.com/view/958522/configuracao-sensores/716canR).

 - [Rota para rede de sensores](https://documenter.getpostman.com/view/958522/configuracao-redes/716canQ);

 - [Rota para os dados](https://documenter.getpostman.com/view/958522/dados/716canP);