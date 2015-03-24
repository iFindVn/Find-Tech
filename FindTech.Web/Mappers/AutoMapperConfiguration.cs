using System;
using AutoMapper;
using FindTech.Entities.Models;
using FindTech.Entities.Models.Enums;
using FindTech.Entities.StoredProcedures.Models;
using FindTech.Web.Areas.BO.Models;
using FindTech.Web.Models;
using Microsoft.Ajax.Utilities;

namespace FindTech.Web.Mappers
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<DomainToViewModelMappingProfile>();
                x.AddProfile<ViewModelToDomainMappingProfile>();
            });
        }

        public class ViewModelToDomainMappingProfile : Profile
        {
            public override string ProfileName
            {
                get { return "ViewModelToDomainMappings"; }
            }

            protected override void Configure()
            {
                Mapper.CreateMap<CommentModel, Comment>();
                Mapper.CreateMap<ArticleGridBOViewModel, Article>()
                    .ForMember(a => a.BoxSize, o => o.MapFrom(x => x.BoxSize.BoxSizeId))
                    .ForMember(a => a.ArticleType, o => o.MapFrom(x => x.ArticleType.ArticleTypeId));
                Mapper.CreateMap<ArticleBOViewModel, Article>()
                    .ForMember(a => a.IsActived, o => o.MapFrom(x => x.IsActived == "on"));
                Mapper.CreateMap<ArticleCategoryBOViewModel, ArticleCategory>();
                Mapper.CreateMap<ContentSectionBOViewModel, ContentSection>();
                Mapper.CreateMap<SourceBOViewModel, Source>();
                Mapper.CreateMap<XpathBOViewModel, Xpath>();
                Mapper.CreateMap<BrandBOViewModel, Brand>();
                Mapper.CreateMap<SpecGroupBOViewModel, SpecGroup>();
                Mapper.CreateMap<SpecBOViewModel, Spec>();
                Mapper.CreateMap<BenchmarkGroupBOViewModel, BenchmarkGroup>();
            }
        }

        public class DomainToViewModelMappingProfile : Profile
        {
            public override string ProfileName
            {
                get { return "DomainToViewModelMappings"; }
            }

            protected override void Configure()
            {
                Mapper.CreateMap<Article, ArticleBOViewModel>();
                Mapper.CreateMap<Article, ArticleGridBOViewModel>()
                    .ForMember(a => a.ArticleType, o => o.MapFrom(x => new ArticleTypeDropDown{ArticleTypeId = (int)x.ArticleType, ArticleTypeName = Enum.GetName(typeof(ArticleType), x.ArticleType)}))
                    .ForMember(a => a.BoxSize, o => o.MapFrom(x => new BoxSizeDropDown { BoxSizeId = (int)x.BoxSize, BoxSizeName = x.BoxSize.ToString()}));
                Mapper.CreateMap<Article, ArticleViewModel>()
                    .ForMember(a => a.ArticleCategoryColor, o => o.MapFrom(x => x.ArticleCategory.Color))
                    .ForMember(a => a.ArticleCategoryName, o => o.MapFrom(x => x.ArticleCategory.ArticleCategoryName))
                    .ForMember(a => a.ArticleCategorySeoName, o => o.MapFrom(x => x.ArticleCategory.SeoName))
                    .ForMember(a => a.SourceName, o => o.MapFrom(x => x.Source.SourceName))
                    .ForMember(a => a.SourceLogo, o => o.MapFrom(x => x.Source.Logo));
                Mapper.CreateMap<ArticleResult, ArticleViewModel>()
                    .ForMember(a => a.HighestOpinionText, o => o.ResolveUsing(x => GetOpinionText(x.OpinionLevel)))
                    .ForMember(a => a.HighestOpinionBackground, o => o.ResolveUsing(x => GetOpinionBackground(x.OpinionLevel)));
                Mapper.CreateMap<Opinion, OpinionViewModel>()
                    .ForMember(a => a.OpinionText, o => o.ResolveUsing(x => GetOpinionText(x.OpinionLevel)))
                    .ForMember(a => a.OpinionBackground, o => o.ResolveUsing(x => GetOpinionBackground(x.OpinionLevel)));
                Mapper.CreateMap<Comment, CommentModel>();
                Mapper.CreateMap<ContentSection, ContentSectionBOViewModel>();
                Mapper.CreateMap<ArticleCategory, ArticleCategoryBOViewModel>();
                Mapper.CreateMap<Source, SourceBOViewModel>();
                Mapper.CreateMap<Xpath, XpathBOViewModel>();
                Mapper.CreateMap<Brand, BrandBOViewModel>();
                Mapper.CreateMap<SpecGroup, SpecGroupBOViewModel>();
                Mapper.CreateMap<Spec, SpecBOViewModel>();
                Mapper.CreateMap<BenchmarkGroup, BenchmarkGroupBOViewModel>()
                    .ForMember(a => a.Parent, o => o.ResolveUsing(x => Mapper.Map<BenchmarkGroupBOViewModel>(x.Parent) ?? new BenchmarkGroupBOViewModel { BenchmarkGroupId = 0, BenchmarkGroupName = "Root" }));
            }

            private string GetOpinionText(OpinionLevel? opinionLevel)
            {
                switch (opinionLevel)
                {
                    case OpinionLevel.Excellent:
                        return "Tuyệt";
                    case OpinionLevel.Good:
                        return "Hay";
                    case OpinionLevel.Average:
                        return "Tạm";
                    case OpinionLevel.Bad:
                        return "Dở";
                    default:
                        return "???";
                }
            }

            private string GetOpinionBackground(OpinionLevel? opinionLevel)
            {
                switch (opinionLevel)
                {
                    case OpinionLevel.Excellent:
                        return "background-danger";
                    case OpinionLevel.Good:
                        return "background-warning";
                    case OpinionLevel.Average:
                        return "background-info";
                    case OpinionLevel.Bad:
                        return "background-success";
                    default:
                        return "background-info";
                }
            }
        }
    }
}