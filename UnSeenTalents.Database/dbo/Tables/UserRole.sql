CREATE TABLE [dbo].[UserRole] (
    [Id]     BIGINT IDENTITY (1, 1) NOT NULL,
    [UserId] BIGINT NOT NULL,
    [RoleId] BIGINT NOT NULL,
    CONSTRAINT [PK_tbl_userrole] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_tbl_userrole_tbl_users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]),
    CONSTRAINT [FK_UserRole_Role] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([Id])
);

