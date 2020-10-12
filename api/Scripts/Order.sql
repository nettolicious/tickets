IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Order]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Order](
	[OrderId] [int] IDENTITY(1,1) NOT NULL,
	[SysCreated] [datetimeoffset](7) NOT NULL,
	[SysCreatedBy] [nvarchar](128) NOT NULL,
	[SysLastModified] [datetimeoffset](7) NOT NULL,
	[SysLastModifiedBy] [nvarchar](128) NOT NULL,
	[ImportOrderId] [int] NOT NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Order_SysCreated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF_Order_SysCreated]  DEFAULT (sysdatetimeoffset()) FOR [SysCreated]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Order_SysCreatedBy]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF_Order_SysCreatedBy]  DEFAULT (suser_name()) FOR [SysCreatedBy]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Order_SysLastModified]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF_Order_SysLastModified]  DEFAULT (sysdatetimeoffset()) FOR [SysLastModified]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Order_SysLastModifiedBy]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF_Order_SysLastModifiedBy]  DEFAULT (suser_name()) FOR [SysLastModifiedBy]
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[trg_Order_SetSysLastModifiedDateAndUser]'))
EXEC dbo.sp_executesql @statement = N'create trigger [dbo].[trg_Order_SetSysLastModifiedDateAndUser]
   on  [dbo].[Order]
   after insert, update
as
begin
    if (exists(select 1 from deleted))
    begin
        -- trigger is firing after update
        update t
            set t.[SysLastModified] = sysdatetimeoffset(),
            t.[SysLastModifiedBy] = suser_name()
        from dbo.[Order] t
            inner join inserted i on i.OrderId = t.OrderId
    end
    else
    begin
        -- trigger is firing after insert
        update t
            set t.[SysCreated] = sysdatetimeoffset(),
            t.[SysCreatedBy] = suser_name(),
            t.[SysLastModified] = sysdatetimeoffset(),
            t.[SysLastModifiedBy] = suser_name()
        from dbo.[Order] t
            inner join inserted i on i.OrderId = t.OrderId
    end
end
' 
GO

ALTER TABLE [dbo].[Order] ENABLE TRIGGER [trg_Order_SetSysLastModifiedDateAndUser]
GO

