CREATE TABLE [dbo].[Token] (
    [Id]                   BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]                 NVARCHAR (100) NOT NULL,
    [NoOfUploadsAllowed]   INT            NOT NULL,
    [Amount]               INT            NOT NULL,
    [IsActive]             BIT            NOT NULL,
    [ExpireDurationInDays] INT            NOT NULL,
    CONSTRAINT [PK_Token] PRIMARY KEY CLUSTERED ([Id] ASC)
);

