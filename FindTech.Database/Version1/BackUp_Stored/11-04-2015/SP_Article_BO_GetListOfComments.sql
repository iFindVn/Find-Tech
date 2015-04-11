USE [FindTech]
GO

/****** Object:  StoredProcedure [ifadmin].[SP_Comment_GetListOfComments]    Script Date: 11-Apr-15 10:04:34 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [ifadmin].[SP_Comment_GetListOfComments] 
(
 @objectId varchar(100),
 @objectType varchar(100),
 @skip int,
 @take int
)
AS
DECLARE @sqlParentAndChild nvarchar(max) ,@sqlParent nvarchar(MAX), @sqlNestedSelect  nvarchar(MAX), @sqlChild nvarchar(MAX), @sqlCount nvarchar(MAX)
DECLARE @from int = @skip+1 
DECLARE	@to  int = @skip+@take 
BEGIN
	
	select  @sqlParent   = N'SELECT tblResult.*, (SELECT COUNT (*) FROM Comments cmt WHERE cmt.ObjectId = tblResult.CommentId AND ObjectType = 3 ) AS ReplyCount  FROM ( '
	select  @sqlParent+= ' SELECT cm.* ,(SELECT COUNT(1) FROM Likes lk WHERE lk.ObjectId = cm.CommentID AND lk.objectType = 3) LikeCount , row_number() over( ORDER BY CreatedDate DESC) AS  Number FROM Comments cm '
	select  @sqlParent+= ' WHERE cm.ObjectType = ' +@objectType+ ''
	select  @sqlParent+= '  AND cm.ObjectId = '+@objectId+') AS tblResult' 
	select  @sqlParent+=  ' WHERE tblResult.Number BETWEEN '+CAST(@from AS nvarchar(20))+' AND '+CAST(@to AS nvarchar(20))+' '
	
	select  @sqlNestedSelect = N' SELECT tblNested.CommentId FROM ('+@sqlParent+') AS tblNested'
	
	select  @sqlChild = N'SELECT * FROM ( SELECT  cmt.*, (SELECT COUNT(1) FROM Likes lk WHERE lk.ObjectId = cmt.CommentID AND lk.objectType = 3) LikeCount , row_number() over (PARTITION  BY cmt.ObjectId ORDER BY CreatedDate) AS Number FROM Comments cmt WHERE cmt.objectId IN ( '+@sqlNestedSelect+' )'

	select  @sqlParentAndChild = @sqlParent+ ' ; ' +@sqlChild 
	select  @sqlParentAndChild += N' ) AS tblResult WHERE tblResult.Number <= 2 ';
	select  @sqlCount = N'SELECT COUNT(1) CommentCount FROM Comments WHERE ObjectId = '+@objectId+' ;'
	select  @sqlParentAndChild += @sqlCount
-- Print @sqlParentAndChild
Exec sp_executesql @sqlParentAndChild

END

GO

