CREATE PROC [dbo].[usp_GetRecentEvents]
AS
BEGIN

	SELECT TOP(6) id
		  ,[creatorid]
		  ,[eventtitle]
	  FROM [dbo].[events]

END