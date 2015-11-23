CREATE TABLE [dbo].[Category] (
    [Id]   BIGINT        IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NULL,
    CONSTRAINT [PK_tbl_Category] PRIMARY KEY CLUSTERED ([Id] ASC)
);

