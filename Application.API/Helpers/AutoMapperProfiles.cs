using System.Collections.Generic;
using System.Linq;
using Application.API.Dtos;
using Application.API.Models;
using AutoMapper;

namespace Application.API.Helpers {
    public class AutoMapperProfiles : Profile {
        public AutoMapperProfiles () {
            CreateMap<User, UserForListDto> ()
                .ForMember (dest => dest.PhotoUrl, opt => {
                    opt.MapFrom (src => src.Photos.FirstOrDefault (p => p.IsMain).Url);
                })
                .ForMember (dest => dest.Age, opt => {
                    opt.ResolveUsing (d => d.DateOfBirth.CalculateAge ());
                });
            CreateMap<User, UserForDetailedDto> ()
                .ForMember (dest => dest.PhotoUrl, opt => {
                    opt.MapFrom (src => src.Photos.FirstOrDefault (p => p.IsMain).Url);
                })
                .ForMember (dest => dest.Age, opt => {
                    opt.ResolveUsing (d => d.DateOfBirth.CalculateAge ());
                });

            CreateMap<UserForUpdateDto, User> ();
            CreateMap<Photo, PhotosForDetailedDto> ();
            CreateMap<Photo, PhotoForReturnDto> ();
            CreateMap<PhotoForCreationDto, Photo> ();
            CreateMap<UserForRegisterDto, User> ();
            CreateMap<UserForReferenceRegisterDto, User> ();
            CreateMap<VoterForCreationDto, Voter> ();
            CreateMap<OrganizationTypeForCreationDto, VoterType> ();
            CreateMap<Voter, VoterForReturnDto> ();
            CreateMap<OrganizationForCreationDto, Organization> ();
            CreateMap<OrganizationTypeUpdateDto, Organization> ();
            CreateMap<VotingYearForCreationDto, VotingYears> ();
            CreateMap<VoterForEditDto, Voter> ();
            CreateMap<ReferenceUpdateDto, Voter> ();
        }
    }
}