USE [FindTech]
GO

/****** Object:  StoredProcedure [ifadmin].[SP_Article_SearchArticles]    Script Date: 11-Apr-15 10:04:04 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [ifadmin].[SP_Article_SearchArticles]
@keyword nvarchar(100),
@orderString varchar(100),
@skip int,
@take int
as 
declare @sql nvarchar(MAX),
		@nl  char(2) = char(13) + char(10)
declare @from int = @skip+1 
declare	@to   int = @skip+@take
declare @sqlOrder nvarchar(1000)
declare @sqlPagging nvarchar(1000)
begin

 If ( @orderString <> '')
begin 
select @sqlOrder = ', row_number () over ( partition by ArticleType order by '+@orderString+') as Number '	    
end
ELSE
begin 
select @sqlOrder = ' , row_number () over (  partition by ArticleType order by PublishedDate desc) as Number '
end

select @sqlPagging =  ' WHERE Number BETWEEN '+CAST(@from AS nvarchar(20))+' AND '+CAST(@to AS nvarchar(20))+' '



select @sql = N' select * from  ( select * '
       +@sqlOrder
       + ' from (' 
       
	   +' select a.ArticleId, a.Title, a.SeoTitle ,a.Description, a.Tags, a.ArticleType ,a.Avatar, a.PublishedDate, a.ArticleCategoryId, a.CreatedUserDisplayName, a.ViewCount, a.SquareAvatar, a.RectangleAvatar, ac.SeoName as ArticleCategorySeoName, ac.Color as ArticleCategoryColor '  + @nl
	   +', (select top 1 o.OpinionLevel from Opinions o where o.ArticleId = a.ArticleId order by o.OpinionCount desc, o.OpinionLevel desc) as OpinionLevel' + @nl
       +' from Articles a join  ArticleCategories ac on a.ArticleCategoryId = ac.ArticleCategoryId' + @nl
	   + ' where a.IsDeleted is null AND a.Title like N''%'+@keyword+'%'' ' + @nl
	   
	        
	    
select @sql += '  union '
select @sql += N'  '
	   +' select a.ArticleId, a.Title, a.SeoTitle ,a.Description, a.Tags, a.ArticleType ,a.Avatar, a.PublishedDate, a.ArticleCategoryId, a.CreatedUserDisplayName, a.ViewCount, a.SquareAvatar, a.RectangleAvatar, ac.SeoName as ArticleCategorySeoName, ac.Color as ArticleCategoryColor '  + @nl
	    +', (select top 1 o.OpinionLevel from Opinions o where o.ArticleId = a.ArticleId order by o.OpinionCount desc, o.OpinionLevel desc) as OpinionLevel' + @nl
	   +' from Articles a join  ArticleCategories ac on a.ArticleCategoryId = ac.ArticleCategoryId' + @nl
	   + ' where a.IsDeleted is null AND a.Description like N''%'+@keyword+'%'' ' + @nl
	   
	    

select @sql += '  union '
select @sql += N' '
	   +N'select a.ArticleId, a.Title, a.SeoTitle ,a.Description, a.Tags, a.ArticleType ,a.Avatar, a.PublishedDate, a.ArticleCategoryId, a.CreatedUserDisplayName, a.ViewCount, a.SquareAvatar, a.RectangleAvatar, ac.SeoName as ArticleCategorySeoName, ac.Color as ArticleCategoryColor '  + @nl
	   +', (select top 1 o.OpinionLevel from Opinions o where o.ArticleId = a.ArticleId order by o.OpinionCount desc, o.OpinionLevel desc) as OpinionLevel' + @nl
	   +'from Articles a join  ArticleCategories ac on a.ArticleCategoryId = ac.ArticleCategoryId' + @nl
	   + ' where a.IsDeleted is null AND a.Content like N''%'+@keyword+'%'' ' + @nl
	   
	   
select @sql += '  union '
select @sql += N''
	   +N'select a.ArticleId, a.Title, a.SeoTitle ,a.Description, a.Tags, a.ArticleType ,a.Avatar, a.PublishedDate, a.ArticleCategoryId, a.CreatedUserDisplayName, a.ViewCount, a.SquareAvatar, a.RectangleAvatar, ac.SeoName as ArticleCategorySeoName, ac.Color as ArticleCategoryColor'  + @nl
	   +', (select top 1 o.OpinionLevel from Opinions o where o.ArticleId = a.ArticleId order by o.OpinionCount desc, o.OpinionLevel desc) as OpinionLevel' + @nl
	   +'from Articles a join  ArticleCategories ac on a.ArticleCategoryId = ac.ArticleCategoryId' + @nl
	   + ' where a.IsDeleted is null AND ac.ArticleCategoryName like  N''%'+@keyword+'%''  ' + @nl
	   + '  ) as tbl1 ) AS tbl2   '
	   +@sqlPagging
	   
-- Print @sql
Exec sp_executesql @sql

end		



GO

