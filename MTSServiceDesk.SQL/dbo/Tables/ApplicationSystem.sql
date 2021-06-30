CREATE TABLE [dbo].[ApplicationSystem]
(
	[Id] INT NOT NULL ,
	[Name] varchar(150) not null,
	[Description] varchar(150) not null,
	[ClientId] int not null,
	[UnderSLA] bit not null,
	[StatusId] int not null, 

    CONSTRAINT [PK_ApplicationSystem] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_ApplicationSystem_Client] FOREIGN KEY ([ClientId]) REFERENCES [SupportClient]([Id]), 
    CONSTRAINT [FK_ApplicationSystem_Status] FOREIGN KEY ([StatusId]) REFERENCES [Status]([Id]),

)
