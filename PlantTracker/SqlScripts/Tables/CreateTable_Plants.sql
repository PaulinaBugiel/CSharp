CREATE TABLE dbo.Plants
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL,
    [PlantPositionId] INT NOT NULL,
	[PlantingDate] datetime NOT NULL DEFAULT((getdate())),
	[LastModificationDate] datetime NOT NULL DEFAULT((getdate())),
    CONSTRAINT [FK_Plants_PlantPositions] FOREIGN KEY ([PlantPositionId]) REFERENCES [PlantPositions]([Id])
)