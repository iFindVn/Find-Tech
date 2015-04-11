USE [FindTech]
GO

/****** Object:  StoredProcedure [ifadmin].[SP_Article_GetListOfArticles]    Script Date: 11-Apr-15 10:03:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [ifadmin].[SP_Article_GetListOfArticles]
@tags nvarchar(1000),
@categories varchar(100),
@articleType int,
@whereClauseMore nvarchar(max),
@orderString varchar(1000),
@skipArticleIds nvarchar(1000),
@skip int,
@take int

as 
declare @sql nvarchar(MAX), @sqlPartition nvarchar(100) = ''
declare @from int = @skip+1 
declare	@to  int = @skip+@take
declare @nl  char(2) = char(13) + char(10) 
begin

IF (@articleType = 3)
begin 
set @sqlPartition = ' PARTITION BY subTblResult.ArticleType '
end

select @sql = 
	   N' SELECT * FROM ('
	   +' SELECT subTblResult.*'
	   +', row_number() over(' 
	    IF(@orderString <> '')
	   begin
	   select @sql += ' ' +@orderString + ' '
	   end
	   Else
	   begin
	   select @sql += +@sqlPartition + ' ORDER BY subTblResult.PublishedDate DESC '
	   end
	   select @sql += ') AS Number '
	   +' FROM ('
	   +' SELECT DISTINCT (a.ArticleId), a.Title, a.SeoTitle ,a.Description, a.Tags, a.ArticleType ,a.Avatar, a.PublishedDate, a.ArticleCategoryId, a.CreatedUserDisplayName, a.ViewCount, a.SquareAvatar, a.RectangleAvatar '
	   +' ,ac.ArticleCategoryName,ac.Color as ArticleCategoryColor,ac.SeoName as ArticleCategorySeoName'
	   +' ,(SELECT COUNT(1) FROM Comments cmt WHERE cmt.ObjectType = 1 AND cmt.ObjectId = a.ArticleId ) AS CommentCount'
	   +' ,(SELECT TOP 1 o.OpinionLevel FROM Opinions o WHERE o.ArticleId = a.ArticleId ORDER BY o.OpinionCount DESC, o.OpinionLevel DESC) as OpinionLevel' + @nl
	   +' FROM Articles a join ArticleCategories ac on ac.ArticleCategoryId = a.ArticleCategoryId '
	   IF(@tags <> '' )
	   begin
		select @sql += ''
       +'JOIN fn_Split('''+@tags+''', '','') on a.Tags LIKE N''%,'' +value + ''%'' or a.Tags LIKE N''%'' + value + '',%'' ' 
	   end

	   IF (@categories <> '')
	   begin 
select @sql += ' JOIN ( SELECT * FROM  fn_Split('''+@categories+''', '','') ) AS splitCategory on ac.SeoName = splitCategory.value '
	   end
	   select @sql += ' WHERE a.IsActived = 1 AND a.IsDeleted IS NULL '
	   IF (@articleType = 1 OR @articleType = 2)
	   begin 
	      select @sql +=  ' AND a.ArticleType = '+CAST(@articleType AS nvarchar(20))+'  ' + @nl
	   end
	   IF (@articleType = 3)
	   begin 
	      select @sql +=  ' AND ( a.ArticleType = 1 OR a.ArticleType = 2   ) ' + @nl
	   end
	   if(@whereClauseMore <> '')
	   begin
		  select @sql += ' AND ' +@whereClauseMore + ' '
	   end
	   select @sql += ' ) as subTblResult '
	   if(@skipArticleIds <> '')
	   begin 
	      select @sql += 'WHERE subTblResult.ArticleId not in ( SELECT Value FROM fn_Split('''+@skipArticleIds+''', '',''))'
	   end
	   select @sql += ' ) as tblResult ' 
	   If(@from <> '' AND @to <> '')
	   begin
		  select @sql += '  WHERE tblResult.Number BETWEEN '+CAST(@from AS nvarchar(20))+' AND '+CAST(@to AS nvarchar(20))+' '
	   end
	   
	   
--Print @sql
Exec sp_executesql @sql
end		


GO

