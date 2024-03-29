﻿using Microsoft.EntityFrameworkCore;
using TestSupport.EfHelpers;

namespace Birder.Tests.Controller;

public class Request_Network_Feed
{
    private readonly Mock<ILogger<ObservationFeedController>> _logger;
    private readonly Mock<IBirdThumbnailPhotoService> _mockProfilePhotosService;

    public Request_Network_Feed()
    {
        _logger = new Mock<ILogger<ObservationFeedController>>();
        _mockProfilePhotosService = new Mock<IBirdThumbnailPhotoService>();
    }

    [Fact]
    public async Task Returns_OkResult_With_Network_Records()
    {
        // when handling Network object, need to use sql database not sqlite in-memory
        var options = this.CreateUniqueMethodOptions<ApplicationDbContext>();

        using var context = new ApplicationDbContext(options);
        // context.Database.EnsureClean();
        //temporary increase timeout only for one Context instance.
        // //context.Database.SetCommandTimeout(TimeSpan.FromMinutes(1));

        context.Database.EnsureClean();

        context.Users.Add(SharedFunctions.CreateUser("testUser1"));
        context.Users.Add(SharedFunctions.CreateUser("testUser2"));

        context.SaveChanges();

        context.Users.Count().ShouldEqual(2);

        // Arrange
        var userManager = SharedFunctions.InitialiseUserManager(context);
        var requestingUsername = "testUser1";
        var mockObsRepo = new Mock<IObservationQueryService>();
        var mockHelper = new Mock<IUserNetworkHelpers>();
        mockHelper.Setup(i => i.GetFollowingUserNames(It.IsAny<ICollection<Network>>()))
                  .Returns(new List<string>());

        var model = new List<ObservationFeedDto>() { new ObservationFeedDto() };
        mockObsRepo.SetupSequence(obs => obs.GetPagedObservationsFeedAsync(It.IsAny<Expression<Func<Observation, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(model);

        var controller = new ObservationFeedController(_logger.Object, userManager, mockObsRepo.Object, mockHelper.Object);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            { User = SharedFunctions.GetTestClaimsPrincipal(requestingUsername) }
        };

        // Act
        var result = await controller.GetNetworkFeedAsync(It.IsAny<int>(), It.IsAny<int>());

        // Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        Assert.IsAssignableFrom<List<ObservationFeedDto>>(objectResult.Value);
    }

    // [Fact]
    // public async Task Returns_500_When_Requesting_User_Is_Null()
    // {
    //     var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

    //     using (var context = new ApplicationDbContext(options))
    //     {
    //         
    //         context.Database.EnsureClean();
    //         
    //         context.Users.Add(SharedFunctions.CreateUser("testUser1"));
    //         context.Users.Add(SharedFunctions.CreateUser("testUser2"));

    //         context.SaveChanges();

    //         context.Users.Count().ShouldEqual(2);

    //         // Arrange
    //         var userManager = SharedFunctions.InitialiseUserManager(context);
    //         var requestingUsername = "Does not exist";
    //         var mockObsRepo = new Mock<IObservationQueryService>();
    //         var controller = new ObservationFeedController(_logger.Object, userManager, mockObsRepo.Object);

    //         controller.ControllerContext = new ControllerContext()
    //         {
    //             HttpContext = new DefaultHttpContext()
    //             { User = SharedFunctions.GetTestClaimsPrincipal(requestingUsername) }
    //         };

    //         // Act
    //         var result = await controller.GetNetworkFeedAsync(It.IsAny<int>(), It.IsAny<int>());

    //         // Assert
    //         var objectResult = Assert.IsType<ObjectResult>(result);
    //         Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    //         var actual = Assert.IsType<string>(objectResult.Value);
    //         Assert.Equal("requesting user not found", actual);
    //     }
    // }

    // this 

    // [Fact]
    // public async Task Returns_500_When_Repository_Returns_Null()
    // {
    //     var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

    //     using (var context = new ApplicationDbContext(options))
    //     {
    //         //  create the database
    //         context.Database.EnsureClean();
    //         
    //         context.Users.Add(SharedFunctions.CreateUser("testUser1"));
    //         context.Users.Add(SharedFunctions.CreateUser("testUser2"));

    //         context.SaveChanges();

    //         context.Users.Count().ShouldEqual(2);

    //         // Arrange
    //         var userManager = SharedFunctions.InitialiseUserManager(context);
    //         var requestingUsername = "testUser1";

    //         var mockObsRepo = new Mock<IObservationQueryService>();
    //         mockObsRepo.Setup(obs => obs.GetPagedObservationsFeedAsync(It.IsAny<Expression<Func<Observation, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
    //             .Returns(Task.FromResult<IEnumerable<ObservationFeedDto>>(null));

    //         var controller = new ObservationFeedController(_logger.Object, userManager, mockObsRepo.Object);

    //         controller.ControllerContext = new ControllerContext()
    //         {
    //             HttpContext = new DefaultHttpContext()
    //             { User = SharedFunctions.GetTestClaimsPrincipal(requestingUsername) }
    //         };

    //         // Act
    //         var result = await controller.GetNetworkFeedAsync(It.IsAny<int>(), It.IsAny<int>());

    //         // Assert
    //         var objectResult = Assert.IsType<ObjectResult>(result);
    //         Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    //         var actual = Assert.IsType<string>(objectResult.Value);
    //         Assert.Equal("an unexpected error occurred", actual);
    //     }
    // }

    [Fact]
    public async Task Returns_500_On_Exception()
    {
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();
        //
        context.Users.Add(SharedFunctions.CreateUser("testUser1"));
        context.Users.Add(SharedFunctions.CreateUser("testUser2"));

        context.SaveChanges();
        context.Users.Count().ShouldEqual(2);

        // Arrange
        var mockHelper = new Mock<IUserNetworkHelpers>();
        var userManager = SharedFunctions.InitialiseUserManager(context);
        var requestingUsername = "testUser1";

        var mockObsRepo = new Mock<IObservationQueryService>();
        mockObsRepo.Setup(obs => obs.GetPagedObservationsFeedAsync(It.IsAny<Expression<Func<Observation, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
            .ThrowsAsync(new InvalidOperationException());

        var controller = new ObservationFeedController(_logger.Object, userManager, mockObsRepo.Object, mockHelper.Object);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            { User = SharedFunctions.GetTestClaimsPrincipal(requestingUsername) }
        };

        // Act
        var result = await controller.GetNetworkFeedAsync(It.IsAny<int>(), It.IsAny<int>());

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        var actual = Assert.IsType<string>(objectResult.Value);
        Assert.Equal($"an unexpected error occurred", actual);
    }
}