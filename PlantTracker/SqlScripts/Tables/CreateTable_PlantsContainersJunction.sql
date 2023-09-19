CREATE TABLE [dbo].[PlantsContainersJunction]
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	PlantId INT NOT NULL,
	ContainerId INT NOT NULL,
	
    CONSTRAINT [FK_PlantsContainersJunction_Plants] FOREIGN KEY ([PlantId]) REFERENCES [Plants]([Id]),
    CONSTRAINT [FK_PlantsContainersJunction_PlantContainers] FOREIGN KEY ([ContainerId]) REFERENCES [PlantContainers]([Id])
);