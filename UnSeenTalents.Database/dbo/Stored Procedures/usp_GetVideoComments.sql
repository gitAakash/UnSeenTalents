-- =============================================
-- Author:		<Vishal Mandloi>
-- Create date: <6 Sep 2015>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetVideoComments]
@videoId BIGINT
AS
BEGIN

SELECT 
    comm.id as commentId,
	usr.id as userId,
	usr.username,
	COALESCE(usr.profilepic,'') As profilePic,
	comm.CommentText as commentText
   FROM Comment comm
   JOIN Users usr on usr.id=comm.userid
   WHERE comm.VideoId=@videoId
   ORDER BY comm.id  DESC
 
END