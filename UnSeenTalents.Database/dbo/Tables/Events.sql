CREATE TABLE [dbo].[Events] (
    [Id]             BIGINT         IDENTITY (1, 1) NOT NULL,
    [CreatorId]      BIGINT         NULL,
    [EventTitle]     NVARCHAR (50)  NULL,
    [EventDesc]      NVARCHAR (MAX) NULL,
    [EventStartDate] DATE           NULL,
    [EventImage]     NVARCHAR (200) NULL,
    [EventEndDate]   DATE           NULL,
    [CreateDate]     DATETIME       NULL,
    [IsActive]       BIT            NULL,
    [IsDeleted]      BIT            NULL,
    CONSTRAINT [PK_tbl_events] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_tbl_events_tbl_users] FOREIGN KEY ([CreatorId]) REFERENCES [dbo].[Users] ([Id])
);

