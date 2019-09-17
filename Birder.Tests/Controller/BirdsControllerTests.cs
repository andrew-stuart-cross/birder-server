﻿using AutoMapper;
using Birder.Controllers;
using Birder.Data;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Xunit;

namespace Birder.Tests.Controller
{
    public class BirdsControllerTests
    {
        private IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<BirdsController>> _logger;

        public BirdsControllerTests()
        {
            _cache = new MemoryCache(new MemoryCacheOptions()); 
            _logger = new Mock<ILogger<BirdsController>>();
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BirderMappingProfile());
            });
            _mapper = mappingConfig.CreateMapper();
        }

        #region GetBirds (Collection)

        [Fact]
        public async Task GetBirds_ReturnsOkObjectResult_WithABirdsObject()
        {
            // Arrange
            var mockRepo = new Mock<IBirdRepository>();
            mockRepo.Setup(repo => repo.GetBirdSummaryListAsync())
                 .ReturnsAsync(GetTestBirds()); //--> needs a real SystemClockService
            //    mockRepo.Setup(repo => repo.GetTweetOfTheDayAsync(It.IsAny<DateTime>()))
            //        .ReturnsAsync(GetTestTweetDay());

            var controller = new BirdsController(_mapper, _cache, _logger.Object, mockRepo.Object);

            // Act
            var result = await controller.GetBirdsAsync(BirderStatus.Common);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }



        #endregion

        private IEnumerable<Bird> GetTestBirds()
        {
            var birds = new List<Bird>();
            birds.Add(new Bird
            {
                BirdId = 1,
                Class = "",
                Order = "",
                Family = "",
                Genus = "",
                Species = "",
                EnglishName = "Test species 1",
                InternationalName = "",
                Category = "",
                PopulationSize = "",
                BtoStatusInBritain = "",
                ThumbnailUrl = "",
                SongUrl = "",
                CreationDate = DateTime.Now.AddDays(-4),
                LastUpdateDate = DateTime.Now.AddDays(-4),
                ConservationStatusId = 0,
                Observations = null,
                BirdConservationStatus = null,
                BirderStatus = BirderStatus.Common,
                TweetDay = null
            });
            birds.Add(new Bird
            {
                BirdId = 2,
                Class = "",
                Order = "",
                Family = "",
                Genus = "",
                Species = "",
                EnglishName = "Test species 2",
                InternationalName = "",
                Category = "",
                PopulationSize = "",
                BtoStatusInBritain = "",
                ThumbnailUrl = "",
                SongUrl = "",
                CreationDate = DateTime.Now.AddDays(-4),
                LastUpdateDate = DateTime.Now.AddDays(-4),
                ConservationStatusId = 0,
                Observations = null,
                BirdConservationStatus = null,
                BirderStatus = BirderStatus.Common,
                TweetDay = null
            });

            return birds;
        }
    }
}
