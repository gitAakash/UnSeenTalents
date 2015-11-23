CREATE TABLE [dbo].[Users] (
    [Id]            BIGINT         IDENTITY (1, 1) NOT NULL,
    [UserName]      NVARCHAR (50)  NOT NULL,
    [Email]         NVARCHAR (50)  NOT NULL,
    [Password]      NVARCHAR (100) NOT NULL,
    [FirstName]     NVARCHAR (50)  NULL,
    [LastName]      NVARCHAR (50)  NULL,
    [ProfilePic]    NVARCHAR (100) NOT NULL,
    [CreatedDate]   DATETIME       NOT NULL,
    [IsActive]      BIT            NOT NULL,
    [IsDeleted]     BIT            NOT NULL,
    [IsToken]       BIT            NULL,
    [NumberOfToken] INT            NULL,
    CONSTRAINT [PK_tbl_users] PRIMARY KEY CLUSTERED ([Id] ASC)
);

