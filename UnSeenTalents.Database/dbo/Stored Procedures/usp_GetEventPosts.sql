-- =============================================
-- Author:		<Vishal Mandloi>
-- Create date: <6 Sep 2015>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetEventPosts]
@eventId BIGINT,
@userId BIGINT
AS
BEGIN

  DECLARE  @eventDiff INT=(SELECT DATEDIFF(DAY,eventstartdate,getdate()) FROM Events WHERE Id=@eventId)

	 IF(@eventDiff<14)
	 BEGIN
		SELECT 
			vd.id as videoId,
			usr.id as userId,
			vd.videoPath,
			vd.videoTitle,
			vd.VideoDesc,
			vd.CreatedDate,
			usr.Username,
			vd.Eventid,
			COALESCE(usr.profilepic,'') As profilePic,
			(select count(VideoId) from Comment where VideoId=vd.id) as videoCount,
			(select count(VideoId) from Vote where VideoId=vd.id and isvote=1) as voteCount,
			CASE WHEN vt.id IS NOT NULL THEN 'True' ELSE 'False' END AS isVote
		   FROM Video vd
		   JOIN Users usr on usr.id=vd.userid
		   LEFT JOIN Vote vt on vt.userid=@userId and vt.VideoId=vd.id and isvote=1
		   WHERE vd.eventid =@eventId
		   ORDER BY vd.id DESC
	END
	IF(@eventDiff>14 AND @eventDiff<=28)
	 BEGIN

		SELECT TOP(24)
			vd.id as videoId,
			usr.id as userId, 
			vd.videoPath,
			vd.videoTitle,
			usr.UserName,
			vd.EventId,
			COALESCE(usr.profilepic,'') As profilePic,
			(select count(VideoId) from Comment where VideoId=vd.id) as videoCount,
			(select count(VideoId) from Vote where VideoId=vd.id and isvote=1) as voteCount,
			CASE WHEN vt.id IS NOT NULL THEN 'True' ELSE 'False' END AS isVote
		   FROM Video vd
		   JOIN Users usr on usr.id=vd.userid
		   LEFT JOIN Vote vt on vt.userid=@userId and vt.VideoId=vd.id and isvote=1
		   WHERE vd.eventid=@eventId 
		   ORDER BY (select count(VideoId) from Comment where VideoId=vd.id) DESC
 
		END

	IF(@eventDiff>28 AND @eventDiff<=35)
	 BEGIN

		SELECT TOP(12)
			vd.id as videoId,
			usr.id as userId, 
			vd.videoPath,
			vd.videoTitle,
			usr.username,
			vd.EventId,
			COALESCE(usr.profilepic,'') As profilePic,
			(select count(VideoId) from Comment where VideoId=vd.id) as videoCount,
			(select count(VideoId) from Vote where VideoId=vd.id and isvote=1) as voteCount,
			CASE WHEN vt.id IS NOT NULL THEN 'True' ELSE 'False' END AS isVote
		   FROM Video vd
		   JOIN Users usr on usr.id=vd.userid
		   LEFT JOIN Vote vt on vt.userid=@userId and vt.VideoId=vd.id and isvote=1
		   WHERE vd.eventid=@eventId 
		   ORDER BY (select count(VideoId) from Comment where VideoId=vd.id) DESC
 
		END
	 IF(@eventDiff>35 AND @eventDiff<=42)
	 BEGIN

		SELECT TOP(6)
			vd.id as videoId,
			usr.id as userId, 
			vd.videoPath,
			vd.videoTitle,
			usr.username,
			vd.eventid,
			COALESCE(usr.profilepic,'') As profilePic,
			(select count(VideoId) from Comment where VideoId=vd.id) as videoCount,
			(select count(VideoId) from Vote where VideoId=vd.id and isvote=1) as voteCount,
			CASE WHEN vt.id IS NOT NULL THEN 'True' ELSE 'False' END AS isVote
		   FROM Video vd
		   JOIN Users usr on usr.id=vd.userid
		   LEFT JOIN Vote vt on vt.userid=@userId and vt.VideoId=vd.id and isvote=1
		   WHERE vd.eventid=@eventId 
		   ORDER BY (select count(VideoId) from Comment where VideoId=vd.id) DESC
 
		END
		ELSE
		BEGIN
		
		SELECT TOP(3)
			vd.id as videoId,
			usr.id as userId, 
			vd.videoPath,
			vd.videoTitle,
			usr.username,
			vd.eventid,
			COALESCE(usr.profilepic,'') As profilePic,
			(select count(VideoId) from Comment where VideoId=vd.id) as videoCount,
			(select count(VideoId) from Vote where VideoId=vd.id and isvote=1) as voteCount,
			CASE WHEN vt.id IS NOT NULL THEN 'True' ELSE 'False' END AS isVote
		   FROM Video vd
		   JOIN Users usr on usr.id=vd.userid
		   LEFT JOIN Vote vt on vt.userid=@userId and vt.VideoId=vd.id and isvote=1
		   WHERE vd.eventid=@eventId 
		   ORDER BY (select count(VideoId) from Comment where VideoId=vd.id) DESC
 

		END
END