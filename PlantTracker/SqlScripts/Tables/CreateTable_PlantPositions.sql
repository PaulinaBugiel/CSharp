CREATE TABLE dbo.PlantPositions
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL,
	[SunExpositionId] INT NOT NULL,
	CONSTRAINT [FK_PlantPositions_ToSunExpositions] FOREIGN KEY ([SunExpositionId]) REFERENCES [SunExpositions]([Id])
);