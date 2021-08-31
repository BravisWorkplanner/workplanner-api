using API;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace UnitTests
{
    public class AutoMapperTests
    {
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public AutoMapperTests()
        {
            var profiles = GetAllMappingProfiles();
            _configuration = new MapperConfiguration(cfg => cfg.AddProfiles(profiles));
            _mapper = _configuration.CreateMapper();
        }

        [Fact]
        public void ValidateAllMappingProfiles()
        {
            _configuration.AssertConfigurationIsValid();
        }

        [Fact]
        public void ShouldSupportMappingFromSourceToDestination()
        {
            var typeMaps = _configuration.GetAllTypeMaps();
            foreach (var map in typeMaps)
            {
                var instance = Activator.CreateInstance(map.SourceType);
                _mapper.Map(instance, map.SourceType, map.DestinationType);
            }
        }

        private static IEnumerable<Profile> GetAllMappingProfiles()
        {
            var profileTypes = Assembly.GetAssembly(typeof(AutoMapping))?.GetExportedTypes().Where(t => t.IsSubclassOf(typeof(Profile)));

            return (profileTypes ?? Type.EmptyTypes).Select(Activator.CreateInstance).Select(obj => (Profile) obj).ToList();
        }
    }
}