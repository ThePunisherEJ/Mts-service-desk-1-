CREATE TABLE [dbo].[SupportTicket]
(
	[Id] INT NOT NULL identity(1,1),
	[DateCreated] datetime not null,
	[CreatedBy] VARCHAR (150)     NOT NULL,
	[ClientId] int not null,
	[SystemId] int not null,
	[Description] varchar(max) not null, 


    CONSTRAINT [PK_SupportTicket] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_SupportTicket_SupportClient] FOREIGN KEY ([ClientId]) REFERENCES [SupportClient]([Id]), 
    CONSTRAINT [FK_SupportTicket_Systems] FOREIGN KEY ([SystemId]) REFERENCES [Systems]([Id]),
)
