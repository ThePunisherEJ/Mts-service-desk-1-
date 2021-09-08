insert dbo.SupportTicket([DateCreated], [CreatedBy], [ClientId], [SystemId], [Description]) values ('2021-09-02','37fe74d7-d040-46b3-a2f0-0bb921ae657d', 1,1, 'This is a test of the Service Desk System') 
go
insert dbo.TicketComment([DateCreated], [CreatedBy], [TicketId], [Comment]) values ('2021-09-03','37fe74d7-d040-46b3-a2f0-0bb921ae657d',1, 'This is Comment 1') 
go
insert dbo.TicketComment([DateCreated], [CreatedBy], [TicketId], [Comment]) values (getdate(),'37fe74d7-d040-46b3-a2f0-0bb921ae657d',1, 'This is Comment 2') 
go
