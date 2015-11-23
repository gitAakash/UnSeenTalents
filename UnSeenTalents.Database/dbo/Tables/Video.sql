CREATE TABLE [dbo].[Video] (
    [Id]            BIGINT         IDENTITY (1, 1) NOT NULL,
    [EventId]       BIGINT         NOT NULL,
    [UserId]        BIGINT         NOT NULL,
    [VideoPath]     NVARCHAR (MAX) NOT NULL,
    [VideoTitle]    NVARCHAR (MAX) NOT NULL,
    [VideoDesc]     NVARCHAR (MAX) NULL,
    [CreatedDate]   DATETIME       NOT NULL,
    [IsActive]      BIT            NOT NULL,
    [IsDeleted]     BIT            NOT NULL,
    [CategoryId]    BIGINT         NOT NULL,
    [TokenUniqueId] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_tbl_vedio] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Video_Category] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Category] ([Id]),
    CONSTRAINT [FK_Video_Events] FOREIGN KEY ([EventId]) REFERENCES [dbo].[Events] ([Id]),
    CONSTRAINT [FK_Video_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);

