CREATE TABLE [dbo].[Systems]
(
	[Id] INT Identity(1,1) NOT NULL,
	[Name] varchar(250) not null,
	[Description] varchar(250) null,
	[ClientId] int not null,
	[StatusId] int not null, 


    CONSTRAINT [PK_Systems] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_Systems_SupportCliet] FOREIGN KEY ([ClientId]) REFERENCES [SupportClient]([Id]), 
    CONSTRAINT [FK_Systems_Status] FOREIGN KEY ([StatusId]) REFERENCES [Status]([Id]) 
)

GO

CREATE INDEX [IX_Systems_ClientId] ON [dbo].[Systems] ([ClientId])
