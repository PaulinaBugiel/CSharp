CREATE TABLE [dbo].[PlantsTypesJunction]
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	PlantId INT NOT NULL,
	TypeId INT NOT NULL,
	
    CONSTRAINT [FK_PlantsTypesJunction_Plants] FOREIGN KEY ([PlantId]) REFERENCES [Plants]([Id]),
    CONSTRAINT [FK_PlantsTypesJunction_PlantTypes] FOREIGN KEY ([TypeId]) REFERENCES [PlantTypes]([Id])
);