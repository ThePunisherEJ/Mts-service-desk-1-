CREATE TABLE [dbo].[SupportClient]
(
	[Id] INT Identity(1,1) NOT NULL ,
	[Name] varchar(150) not null,
    [Logo] varbinary(max) null,
	[DomainName] varchar(150) not null,
	[StatusId] int not null, 

    CONSTRAINT [PK_SupportClient] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_SupportClient_Status] FOREIGN KEY ([StatusId]) REFERENCES [Status]([Id]),

)
