using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers;

public class AutoMapperProfiles : Profile
{
  public AutoMapperProfiles()
  {
    // Mapping configuration from AppUser entity to MemberDto
    CreateMap<AppUser, MemberDto>()
      // Maps the Age property in MemberDto using the CalculateAge() extension method on DateOfBirth
      .ForMember(d => d.Age, o => o.MapFrom(s => s.DateOfBirth.CalculateAge()))
      // Maps the PhotoUrl property by selecting the URL of the user's main photo (IsMain = true)
      .ForMember(d => d.PhotoUrl, o =>
        o.MapFrom(s => s.Photos.FirstOrDefault(x => x.IsMain)!.Url));

    CreateMap<Photo, PhotoDto>();
    CreateMap<MemberUpdateDto, AppUser>();
    CreateMap<RegisterDto, AppUser>();

    // Mapping configuration from string to DateOnly
    // Converts a string into a DateOnly object using the DateOnly.Parse method
    CreateMap<string, DateOnly>().ConstructUsing(s => DateOnly.Parse(s));
  }
}
