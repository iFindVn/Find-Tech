using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using FindTech.Entities.Models.Enums;
using FindTech.Entities.StoredProcedures;
using FindTech.Entities.StoredProcedures.Models;

namespace FindTech.Entities
{
    public partial class FindTechContext : IFindTechStoredProcedures
    {
        #region Article Stored Procedures

        public IEnumerable<SearchArticlesResult> SearchArticles(string keyword, string orderString = "")
        {
            var keywordParameter = keyword != null ?
            new SqlParameter("@keyword", keyword) :
            new SqlParameter("@keyword", typeof(string));

            var orderStringParameter = orderString != null ?
            new SqlParameter("@orderString", orderString) :
            new SqlParameter("@orderString", typeof(string));

            return Database.SqlQuery<SearchArticlesResult>("SP_Article_SearchArticles @keyword, @orderString", keywordParameter, orderStringParameter);
        }

        public IEnumerable<ArticleResult> GetListOfArticles(GetListOfArticlesParameters getListOfArticlesParameters)
        {
            var tagsParameter = getListOfArticlesParameters.Tags != null ?
            new SqlParameter("@tags", getListOfArticlesParameters.Tags) :
            new SqlParameter("@tags", typeof(string));

            var categoriesParameter = getListOfArticlesParameters.Categories != null ?
            new SqlParameter("@categories", getListOfArticlesParameters.Categories) :
            new SqlParameter("@categories", typeof(string));

            var articleTypeParameter = new SqlParameter("@articleType", (int)getListOfArticlesParameters.ArticleType);

            var whereClauseMoreParameter = getListOfArticlesParameters.WhereClauseMore != null ?
            new SqlParameter("@whereClauseMore", getListOfArticlesParameters.WhereClauseMore) :
            new SqlParameter("@whereClauseMore", typeof(string));

            var orderStringParameter = getListOfArticlesParameters.OrderString != null ?
            new SqlParameter("@orderString", getListOfArticlesParameters.OrderString) :
            new SqlParameter("@orderString", typeof(string));

            var skipArticleIdsParameter = getListOfArticlesParameters.SkipArticleIds != null ?
            new SqlParameter("@skipArticleIds", getListOfArticlesParameters.SkipArticleIds) :
            new SqlParameter("@skipArticleIds", typeof(string));

            var skipParameter = new SqlParameter("@skip", getListOfArticlesParameters.Skip);

            var takeParameter = new SqlParameter("@take", getListOfArticlesParameters.Take);

            return
                Database.SqlQuery<ArticleResult>(
                    "SP_Article_GetListOfArticles @tags, @categories, @articleType, @whereClauseMore, @orderString, @skipArticleIds, @skip, @take",
                    tagsParameter, categoriesParameter, articleTypeParameter, whereClauseMoreParameter,
                    orderStringParameter, skipArticleIdsParameter, skipParameter, takeParameter).AsEnumerable();
        }

        public IEnumerable<CommentResult> GetListOfComments(int objectId, ObjectType objectType, int skip, int take, ref int commentCount)
        {
            using (var db = new FindTechContext())
            {
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandText = "SP_Comment_GetListOfComments";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("objectId", objectId));
                cmd.Parameters.Add(new SqlParameter("objectType", (int) objectType));
                cmd.Parameters.Add(new SqlParameter("skip", skip));
                cmd.Parameters.Add(new SqlParameter("take", take));
                try
                {
                    db.Database.Connection.Open();
                    var reader = cmd.ExecuteReader();
                    var comments = new List<CommentResult>();
                    while (reader.Read())
                    {
                        comments.Add(new CommentResult
                        {
                            CommentatorEmail = !reader.IsDBNull(reader.GetOrdinal("CommentatorEmail")) ? reader.GetString(reader.GetOrdinal("CommentatorEmail")) : "",
                            CommentId = !reader.IsDBNull(reader.GetOrdinal("CommentId")) ? reader.GetInt32(reader.GetOrdinal("CommentId")) : 0,
                            Content = !reader.IsDBNull(reader.GetOrdinal("Content")) ? reader.GetString(reader.GetOrdinal("Content")) : "",
                            CreatedDate = !reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ? reader.GetDateTime(reader.GetOrdinal("CreatedDate")) : DateTime.MinValue,
                            LikeCount = !reader.IsDBNull(reader.GetOrdinal("LikeCount")) ? reader.GetInt32(reader.GetOrdinal("LikeCount")) : 0,
                            ObjectId = !reader.IsDBNull(reader.GetOrdinal("ObjectId")) ? reader.GetInt32(reader.GetOrdinal("ObjectId")) : 0,
                            ObjectType = !reader.IsDBNull(reader.GetOrdinal("ObjectType")) ? (ObjectType)reader.GetInt32(reader.GetOrdinal("ObjectType")) : ObjectType.Article,
                            ReplyCount = !reader.IsDBNull(reader.GetOrdinal("ReplyCount")) ? reader.GetInt32(reader.GetOrdinal("ReplyCount")) : 0
                        });
                    }
                    reader.NextResult();
                    var replies = new List<CommentResult>();
                    while (reader.Read())
                    {
                        replies.Add(new CommentResult
                        {
                            CommentatorEmail = !reader.IsDBNull(reader.GetOrdinal("CommentatorEmail")) ? reader.GetString(reader.GetOrdinal("CommentatorEmail")) : "",
                            CommentId = !reader.IsDBNull(reader.GetOrdinal("CommentId")) ? reader.GetInt32(reader.GetOrdinal("CommentId")) : 0,
                            Content = !reader.IsDBNull(reader.GetOrdinal("Content")) ? reader.GetString(reader.GetOrdinal("Content")) : "",
                            CreatedDate = !reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ? reader.GetDateTime(reader.GetOrdinal("CreatedDate")) : DateTime.MinValue,
                            LikeCount = !reader.IsDBNull(reader.GetOrdinal("LikeCount")) ? reader.GetInt32(reader.GetOrdinal("LikeCount")) : 0,
                            ObjectId = !reader.IsDBNull(reader.GetOrdinal("ObjectId")) ? reader.GetInt32(reader.GetOrdinal("ObjectId")) : 0,
                            ObjectType = !reader.IsDBNull(reader.GetOrdinal("ObjectType")) ? (ObjectType)reader.GetInt32(reader.GetOrdinal("ObjectType")) : ObjectType.Article
                        });
                    }
                    reader.NextResult();
                    while (reader.Read())
                    {
                        commentCount = !reader.IsDBNull(reader.GetOrdinal("CommentCount"))
                            ? reader.GetInt32(reader.GetOrdinal("CommentCount"))
                            : 0;
                    }
                    reader.Close();
                    foreach (var comment in comments)
                    {
                        comment.Replies = replies.Where(a => a.ObjectId == comment.CommentId);
                    }
                    return comments;
                }
                catch (Exception e)
                {
                    return null;
                }
                finally
                {
                    db.Database.Connection.Close();
                }
            }
        } 
        #endregion
    }
}
