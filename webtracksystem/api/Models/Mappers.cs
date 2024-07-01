using Api.Models;
using AutoMapper;
using BusinessLayer.Helpers.Location;
using BusinessLayer.Models;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Repositories.Dto;

namespace Api
{
    public static class AutoMapperApiInitializer {
        public static void Init(IMapperConfigurationExpression cfg){
                cfg.CreateMap<Device, DeviceForUpdate>();
                cfg.CreateMap<DeviceForUpdate, DeviceUpdate>();
                cfg.CreateMap<DeviceForPostWithoutUser, NewDevice>();
                cfg.CreateMap<DeviceForCreation, NewDevice>();
                cfg.CreateMap<Operation<DeviceForCreation>, Operation<DeviceForUpdate>>();
                
        }
    }
}