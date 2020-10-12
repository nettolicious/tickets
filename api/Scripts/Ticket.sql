SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ticket]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ticket](
	[TicketId] [int] IDENTITY(1,1) NOT NULL,
	[SysCreated] [datetimeoffset](7) NOT NULL,
	[SysCreatedBy] [nvarchar](128) NOT NULL,
	[SysLastModified] [datetimeoffset](7) NOT NULL,
	[SysLastModifiedBy] [nvarchar](128) NOT NULL,
	[OrderId] [int] NOT NULL,
	[FirstName] [nvarchar](100) NOT NULL,
	[LastName] [nvarchar](100) NOT NULL,
	[TicketNumber] [nvarchar](25) NOT NULL,
	[EventDate] [datetimeoffset](7) NOT NULL,
 CONSTRAINT [PK_Ticket] PRIMARY KEY CLUSTERED 
(
	[TicketId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
END
GO

SET ANSI_PADDING ON
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Ticket]') AND name = N'UQIX_Ticket_TicketNumber')
CREATE UNIQUE NONCLUSTERED INDEX [UQIX_Ticket_TicketNumber] ON [dbo].[Ticket]
(
	[TicketNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ticket_SysCreated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ticket] ADD  CONSTRAINT [DF_Ticket_SysCreated]  DEFAULT (sysdatetimeoffset()) FOR [SysCreated]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ticket_SysCreatedBy]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ticket] ADD  CONSTRAINT [DF_Ticket_SysCreatedBy]  DEFAULT (suser_name()) FOR [SysCreatedBy]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ticket_SysLastModified]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ticket] ADD  CONSTRAINT [DF_Ticket_SysLastModified]  DEFAULT (sysdatetimeoffset()) FOR [SysLastModified]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ticket_SysLastModifiedBy]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ticket] ADD  CONSTRAINT [DF_Ticket_SysLastModifiedBy]  DEFAULT (suser_name()) FOR [SysLastModifiedBy]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ticket_Order]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ticket]'))
ALTER TABLE [dbo].[Ticket]  WITH CHECK ADD  CONSTRAINT [FK_Ticket_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([OrderId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ticket_Order]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ticket]'))
ALTER TABLE [dbo].[Ticket] CHECK CONSTRAINT [FK_Ticket_Order]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[trg_Ticket_SetSysLastModifiedDateAndUser]'))
EXEC dbo.sp_executesql @statement = N'create trigger [dbo].[trg_Ticket_SetSysLastModifiedDateAndUser]
   on  [dbo].[Ticket]
   after insert, update
as
begin
    if (exists(select 1 from deleted))
    begin
        -- trigger is firing after update
        update t
            set t.[SysLastModified] = sysdatetimeoffset(),
            t.[SysLastModifiedBy] = suser_name()
        from dbo.[Ticket] t
            inner join inserted i on i.TicketId = t.TicketId
    end
    else
    begin
        -- trigger is firing after insert
        update t
            set t.[SysCreated] = sysdatetimeoffset(),
            t.[SysCreatedBy] = suser_name(),
            t.[SysLastModified] = sysdatetimeoffset(),
            t.[SysLastModifiedBy] = suser_name()
        from dbo.[Ticket] t
            inner join inserted i on i.TicketId = t.TicketId
    end
end
' 
GO

ALTER TABLE [dbo].[Ticket] ENABLE TRIGGER [trg_Ticket_SetSysLastModifiedDateAndUser]
GO

