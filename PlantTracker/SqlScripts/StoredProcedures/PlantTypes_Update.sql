SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.spPlantTypes_Update
	@TypeId int,
	@Name nvarchar(50)
AS
BEGIN
	update dbo.PlantTypes
	set Name = @Name
	where Id = @TypeId;
END
GO