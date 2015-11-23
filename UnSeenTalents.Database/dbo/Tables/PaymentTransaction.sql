CREATE TABLE [dbo].[PaymentTransaction] (
    [Id]                BIGINT         IDENTITY (1, 1) NOT NULL,
    [UserId]            BIGINT         NOT NULL,
    [TransactionNumber] NVARCHAR (500) NOT NULL,
    [Amount]            FLOAT (53)     NOT NULL,
    [TokenId]           BIGINT         NOT NULL,
    [TransactionDate]   DATETIME       NOT NULL,
    [IsDeleted]         BIT            NOT NULL,
    [IsActive]          BIT            NOT NULL,
    CONSTRAINT [PK_tbl_paymenttransaction] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PaymentTransaction_Token] FOREIGN KEY ([TokenId]) REFERENCES [dbo].[Token] ([Id]),
    CONSTRAINT [FK_PaymentTransaction_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);

