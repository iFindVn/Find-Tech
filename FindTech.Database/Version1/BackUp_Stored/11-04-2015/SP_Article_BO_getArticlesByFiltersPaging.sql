USE [FindTech]
GO

/****** Object:  StoredProcedure [ifadmin].[SP_Article_BO_getArticlesByFiltersPaging]    Script Date: 11-Apr-15 10:03:28 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [ifadmin].[SP_Article_BO_getArticlesByFiltersPaging]
@whereClause nvarchar(max),
@skip int,
@take int

as 
declare @sql nvarchar(MAX)
declare @from int = @skip+1 
declare	@to  int = @skip+@take
declare @nl  char(2) = char(13) + char(10) 
begin
select @sql = N''
	   +' SELECT * FROM ( SELECT a.*, row_number() over( ORDER BY ArticleId DESC ) AS Number FROM ARTICLES a WHERE a.IsDeleted IS NULL ' + @nl
	    IF(@whereClause <> '')
		   begin
		   select @sql += ' AND ' +@whereClause + ' ' + @nl
		   End
		select @sql += ' ) as TblResult'
	    select @sql += ' WHERE TblResult.Number BETWEEN '+CAST(@from AS nvarchar(20))+' AND ' +CAST(@to AS nvarchar(20))
	   	 
--Print @sql
Exec sp_executesql @sql

end		

GO

