using AutoMapper;
using NewsAPI.Models;
using NewsApp.Models.AppModels;

namespace NewsApp.Data.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Source, NewsSourceModel>(MemberList.None)
				.ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
				.ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name)).ReverseMap();

			CreateMap<Article, NewsModel>(MemberList.None)
			   .ForMember(d => d.Author, opt => opt.MapFrom(s => s.Author))
			   .ForMember(d => d.Title, opt => opt.MapFrom(s => s.Title))
			   .ForMember(d => d.Source, opt => opt.MapFrom(s => s.Source))
			   .ForMember(d => d.Description, opt => opt.MapFrom(s => s.Description))
			   .ForMember(d => d.Url, opt => opt.MapFrom(s => s.Url))
			   .ForMember(d => d.UrlToImage, opt => opt.MapFrom(s => s.UrlToImage))
			   .ForMember(d => d.PublishedAt, opt => opt.MapFrom(s => s.PublishedAt))
			   .ForMember(d => d.Content, opt => opt.MapFrom(s => s.Content)).ReverseMap();

			CreateMap<ArticlesResult, NewsResultModel>(MemberList.None)
				.ForMember(d => d.TotalResults, opt => opt.MapFrom(s => s.TotalResults))
				.ForMember(d => d.Articles, opt => opt.MapFrom(s => s.Articles)).ReverseMap();
		}
	}
}