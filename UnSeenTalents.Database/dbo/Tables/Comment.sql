CREATE TABLE [dbo].[Comment] (
    [Id]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [UserId]      BIGINT         NULL,
    [VideoId]     BIGINT         NULL,
    [CommentText] NVARCHAR (MAX) NULL,
    [CreatedDate] DATETIME       NULL,
    [IsActive]    BIT            NULL,
    [IsDeleted]   BIT            NULL,
    CONSTRAINT [PK_tbl_comment] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_tbl_comment_tbl_users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);

