CREATE TABLE [dbo].[TicketComment]
(
	[Id] INT NOT NULL Identity(1,1), 
	[DateCreated] datetime not null,
	[CreatedBy] VARCHAR (150) NOT NULL,
	[TicketId] int not null,
	[Comment] varchar(max) not null

    CONSTRAINT [PK_TicketComment] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_TicketComment_SupportTicket] FOREIGN KEY ([TicketId]) REFERENCES [SupportTicket]([Id]),

)
