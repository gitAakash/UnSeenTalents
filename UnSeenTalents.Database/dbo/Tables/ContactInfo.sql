CREATE TABLE [dbo].[ContactInfo] (
    [pkId]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [contactName] NVARCHAR (100) NULL,
    [emailId]     NVARCHAR (100) NULL,
    [description] NVARCHAR (MAX) NULL,
    [createdDate] DATETIME       NULL,
    CONSTRAINT [PK_tblContactInfo] PRIMARY KEY CLUSTERED ([pkId] ASC)
);

