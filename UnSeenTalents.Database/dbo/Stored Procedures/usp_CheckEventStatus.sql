-- =============================================
-- Author:		<Vishal Mandloi>
-- Create date: <6 Sep 2015>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_CheckEventStatus]
@eventId BIGINT,
@userId BIGINT
AS
BEGIN
  DECLARE  @eventDiff INT=(SELECT DATEDIFF(DAY,eventstartdate,getdate()) FROM Events WHERE id=@eventId)
  SELECT CASE WHEN @eventDiff>14 THEN 0 ELSE 1 END isUploading --isUploading 1: Event is in BackStage Period 
														       --isUploading 0: Event is Ready to vote
END