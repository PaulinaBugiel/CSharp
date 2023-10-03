CREATE TABLE [dbo].[PlantsWateringSystemsJunction]
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	PlantId INT NOT NULL,
	WateringSystemId INT NOT NULL,
	
    CONSTRAINT [FK_PlantsWateringSystemsJunction_Plants] FOREIGN KEY ([PlantId]) REFERENCES [Plants]([Id]),
    CONSTRAINT [FK_PlantsWateringSystemsJunction_WateringSystems] FOREIGN KEY ([WateringSystemId]) REFERENCES [WateringSystems]([Id])
);