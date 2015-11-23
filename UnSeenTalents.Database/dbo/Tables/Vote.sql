CREATE TABLE [dbo].[Vote] (
    [Id]          BIGINT   IDENTITY (1, 1) NOT NULL,
    [UserId]      BIGINT   NULL,
    [VideoId]     BIGINT   NULL,
    [IsVote]      BIT      NULL,
    [CreatedDate] DATETIME NULL,
    CONSTRAINT [PK_tbl_vote] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_tbl_vote_tbl_users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]),
    CONSTRAINT [FK_Vote_Video] FOREIGN KEY ([VideoId]) REFERENCES [dbo].[Video] ([Id])
);

