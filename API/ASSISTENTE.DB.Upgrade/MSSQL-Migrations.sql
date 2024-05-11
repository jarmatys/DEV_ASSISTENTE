IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240511081934_Init'
)
BEGIN
    CREATE TABLE [Questions] (
        [Id] uniqueidentifier NOT NULL,
        [Text] nvarchar(max) NOT NULL,
        [ConnectionId] nvarchar(max) NULL,
        [Context] nvarchar(max) NULL,
        [Embeddings] nvarchar(max) NULL,
        [CreatedBy] nvarchar(max) NULL,
        [Created] datetime2 NOT NULL,
        [ModifiedBy] nvarchar(max) NULL,
        [Modified] datetime2 NULL,
        CONSTRAINT [PK_Questions] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240511081934_Init'
)
BEGIN
    CREATE TABLE [Resources] (
        [Id] uniqueidentifier NOT NULL,
        [Content] nvarchar(max) NOT NULL,
        [Title] nvarchar(max) NOT NULL,
        [Type] nvarchar(max) NOT NULL,
        [Embeddings] nvarchar(max) NOT NULL,
        [CreatedBy] nvarchar(max) NULL,
        [Created] datetime2 NOT NULL,
        [ModifiedBy] nvarchar(max) NULL,
        [Modified] datetime2 NULL,
        CONSTRAINT [PK_Resources] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240511081934_Init'
)
BEGIN
    CREATE TABLE [Answers] (
        [Id] int NOT NULL IDENTITY,
        [Text] nvarchar(max) NOT NULL,
        [Prompt] nvarchar(max) NOT NULL,
        [Client] nvarchar(max) NOT NULL,
        [Model] nvarchar(max) NOT NULL,
        [PromptTokens] int NOT NULL,
        [CompletionTokens] int NOT NULL,
        [QuestionId] uniqueidentifier NOT NULL,
        [CreatedBy] nvarchar(max) NULL,
        [Created] datetime2 NOT NULL,
        [ModifiedBy] nvarchar(max) NULL,
        [Modified] datetime2 NULL,
        CONSTRAINT [PK_Answers] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Answers_Questions_QuestionId] FOREIGN KEY ([QuestionId]) REFERENCES [Questions] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240511081934_Init'
)
BEGIN
    CREATE TABLE [QuestionResources] (
        [Id] int NOT NULL IDENTITY,
        [QuestionId] uniqueidentifier NOT NULL,
        [ResourceId] uniqueidentifier NOT NULL,
        [CreatedBy] nvarchar(max) NULL,
        [Created] datetime2 NOT NULL,
        [ModifiedBy] nvarchar(max) NULL,
        [Modified] datetime2 NULL,
        CONSTRAINT [PK_QuestionResources] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_QuestionResources_Questions_QuestionId] FOREIGN KEY ([QuestionId]) REFERENCES [Questions] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_QuestionResources_Resources_ResourceId] FOREIGN KEY ([ResourceId]) REFERENCES [Resources] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240511081934_Init'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Answers_QuestionId] ON [Answers] ([QuestionId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240511081934_Init'
)
BEGIN
    CREATE INDEX [IX_QuestionResources_QuestionId] ON [QuestionResources] ([QuestionId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240511081934_Init'
)
BEGIN
    CREATE INDEX [IX_QuestionResources_ResourceId] ON [QuestionResources] ([ResourceId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240511081934_Init'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240511081934_Init', N'8.0.3');
END;
GO

COMMIT;
GO

