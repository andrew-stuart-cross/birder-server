﻿namespace Birder.Tests.Controller;

public class GetObservationAnalysisAsyncTests
{
    [Fact]
    public async Task GetObservationAnalysisAsync_ReturnsOkObjectResult_WithOkResult()
    {
        // Arrange
        Mock<ILogger<ObservationAnalysisController>> loggerMock = new();
        var mockService = new Mock<IObservationsAnalysisService>();

        mockService.Setup(serve => serve.GetObservationsSummaryAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
            .ReturnsAsync(new ObservationAnalysisViewModel { TotalObservationsCount = 2, UniqueSpeciesCount = 2 });

        var controller = new ObservationAnalysisController(loggerMock.Object, mockService.Object);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal() }
        };

        // Act
        var result = await controller.GetObservationAnalysisAsync("test");

        // Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        var actualObs = Assert.IsType<ObservationAnalysisViewModel>(objectResult.Value);
        Assert.True(objectResult is OkObjectResult);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        Assert.IsType<ObservationAnalysisViewModel>(objectResult.Value);
        Assert.Equal(2, actualObs.TotalObservationsCount);
        Assert.Equal(2, actualObs.UniqueSpeciesCount);
    }


    [Fact]
    public async Task GetObservationAnalysisAsync_ReturnsBadRequestResult_WhenExceptionIsRaised()
    {
        // Arrange
        Mock<ILogger<ObservationAnalysisController>> loggerMock = new();
        var mockService = new Mock<IObservationsAnalysisService>();

        mockService.Setup(serve => serve.GetObservationsSummaryAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
                  .ThrowsAsync(new InvalidOperationException());

        var controller = new ObservationAnalysisController(loggerMock.Object, mockService.Object);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal() }
        };

        // Act
        var result = await controller.GetObservationAnalysisAsync("test");

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = result as ObjectResult;
        Assert.Equal("an unexpected error occurred", objectResult.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task GetObservationAnalysisAsync_ReturnsBadRequest_WhenArgument_Is_Invalid(string requstedUsername)
    {
        // Arrange
        Mock<ILogger<ObservationAnalysisController>> loggerMock = new();
        var mockService = new Mock<IObservationsAnalysisService>();

        mockService.Setup(serve => serve.GetObservationsSummaryAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
                  .ThrowsAsync(new InvalidOperationException());

        var controller = new ObservationAnalysisController(loggerMock.Object, mockService.Object);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal() }
        };

        // Act
        var result = await controller.GetObservationAnalysisAsync(requstedUsername);

        // Assert
        var objectResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        var actual = Assert.IsType<string>(objectResult.Value);
        Assert.Equal("requestedUsername is null or empty", objectResult.Value);
    }

    [Fact]
    public async Task GetObservationCountAsync_When_Object_Is_NULL_Returns_500()
    {
        // Arrange
        Mock<ILogger<ObservationAnalysisController>> loggerMock = new();

        var mockService = new Mock<IObservationsAnalysisService>();
        mockService.Setup(serve => serve.GetObservationsSummaryAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
            .Returns(Task.FromResult<ObservationAnalysisViewModel>(null));

        var controller = new ObservationAnalysisController(loggerMock.Object, mockService.Object);

        // Act
        var result = await controller.GetObservationAnalysisAsync("requstedUsername");

        // Assert
        var expectedResponseObject = "an unexpected error occurred";
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        var actual = Assert.IsType<string>(objectResult.Value);
        Assert.Equal(expectedResponseObject, actual);

        loggerMock.Verify(x => x.Log(
           It.Is<LogLevel>(l => l == LogLevel.Warning),
           It.IsAny<EventId>(),
           It.Is<It.IsAnyType>((v, t) => true),//It.Is<It.IsAnyType>((o, t) => string.Equals(expectedExceptionMessage, o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
           It.IsAny<Exception>(),
           It.IsAny<Func<It.IsAnyType, Exception, string>>()),
           Times.Once);
    }
}