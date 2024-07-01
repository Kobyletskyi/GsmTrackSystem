using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BusinessLayer.Models;
using Repositories.UnitOfWork;
using System.Security.Cryptography;
using Repositories.Dto;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using BusinessLayer.Parsers;
using BusinessLayer.Helpers.Location;
using GeoCoordinatePortable;
using System.Reflection;
using BusinessLayer.Transformation;

namespace BusinessLayer
{
    public abstract class BaseLogic
    {
        public BaseLogic(IUnitOfWorkRepositories repositories, IPropertyMappingService propertyMappingService)
        {
            _repositories = repositories;
            _propertyMappingService = propertyMappingService;
        }
        protected IUnitOfWorkRepositories _repositories;
        protected IPropertyMappingService _propertyMappingService;
    }
}
