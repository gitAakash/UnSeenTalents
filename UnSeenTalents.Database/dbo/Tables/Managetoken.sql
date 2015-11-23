CREATE TABLE [dbo].[Managetoken] (
    [Id]                   BIGINT        IDENTITY (1, 1) NOT NULL,
    [UserId]               BIGINT        NOT NULL,
    [TokenId]              BIGINT        NOT NULL,
    [CreateDate]           DATETIME      NOT NULL,
    [IsActive]             BIT           NOT NULL,
    [UniqueTokenId]        NVARCHAR (50) NOT NULL,
    [WillExpireOn]         DATETIME      NOT NULL,
    [RemainingUploadCount] INT           NOT NULL,
    CONSTRAINT [PK_tbl_managetoken] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Managetoken_Token] FOREIGN KEY ([TokenId]) REFERENCES [dbo].[Token] ([Id]),
    CONSTRAINT [FK_tbl_managetoken_tbl_users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);

