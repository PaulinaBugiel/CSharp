CREATE TABLE [dbo].[PlantsPositionsJunction]
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	PlantId INT NOT NULL,
	PositionId INT NOT NULL,
	
    CONSTRAINT [FK_PlantsPositionsJunction_Plants] FOREIGN KEY ([PlantId]) REFERENCES [Plants]([Id]),
    CONSTRAINT [FK_PlantsPositionsJunction_PlantPositions] FOREIGN KEY ([PositionId]) REFERENCES [PlantPositions]([Id])
);