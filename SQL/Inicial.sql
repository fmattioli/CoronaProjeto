--Config banco de dados bot:
--drop table ConversaBot
--drop table ConfigBot
Create table ConfigBot
(
	PK_Id int Primary Key identity not null,
	Nome  varchar(50) not null,
	Card varchar(50) not null,
	Descricao varchar(500) not null
)

Create table ConversaBot
(
	PK_Id int Primary Key identity not null,
	DataConversa varchar(25),
	JsonConversa nvarchar(max),
	FK_ConfigID int null --FOREIGN KEY (PK_Id) REFERENCES ConfigBot(PK_Id)
)

ALTER TABLE ConversaBot
ADD CONSTRAINT FK_ConfigID FOREIGN KEY (FK_ConfigID) REFERENCES ConfigBot(PK_Id);

INSERT INTO  
ConfigBot
(
Nome,
Card,
Descricao
)
VALUES
(
'Idade', 
'Idade',
'Representação de um botão que terá em seu retorno a idade do usuário'
)

INSERT INTO  
ConfigBot
(
Nome,
Card,
Descricao
)
VALUES
(
'Sexo', 
'Sexo',
'Representação de um botão que terá em seu retorno o sexo do usuário'
)


INSERT INTO  
ConfigBot
(
Nome,
Card,
Descricao
)
VALUES
(
'Doencas', 
'Doencas',
'Representação de um card com diversas checkbox que terá em seu retorno as doenças do usuário'
)

select * from ConversaBot
select * from ConfigBot


