CREATE TABLE [dbo].[PlantsContainers]
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	PlantId INT NOT NULL,
	ContainerId INT NOT NULL,
	
    CONSTRAINT [FK_PlantsContainers_Plants] FOREIGN KEY ([PlantId]) REFERENCES [Plants]([Id]),
    CONSTRAINT [FK_PlantsContainers_PlantContainers] FOREIGN KEY ([ContainerId]) REFERENCES [PlantContainers]([Id])
);