using AutoMapper;
using BusinessLayer.Helpers.Location;
using BusinessLayer.Models;
using Repositories.Dto;

namespace BusinessLayer.Mappers
{
    
    public static class AutoMapperInitializer {
        public static void Init(IMapperConfigurationExpression cfg){
                cfg.CreateMap<UserEntity, User>();
                cfg.CreateMap<TrackEntity, Track>();
                cfg.CreateMap<DeviceEntity, Device>();
                cfg.CreateMap<DeviceUpdate, DeviceEntity>();
                    //.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
                cfg.CreateMap<NewDevice, DeviceEntity>();
                cfg.CreateMap<AuthCodeEntity, DeviceCode>()
                    .ForMember(dest => dest.IMEI, opt => opt.MapFrom(src => src.IMEI));
                cfg.CreateMap<GpsResponseEntity, GpsLocation>();
                cfg.CreateMap<GpsResponse, GpsResponseEntity>()
                    .ForMember(dest => dest.NorthOrSouth, opt => opt.MapFrom(src => src.NorthOrSouth.ToString()))
                    .ForMember(dest => dest.EastOrWest, opt => opt.MapFrom(src => src.EastOrWest.ToString()))
                    .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => CoordinatesExtention.ConvertNmeaToDeg(src.Latitude)))
                    .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => CoordinatesExtention.ConvertNmeaToDeg(src.Longitude)));
            
        }
    }
}