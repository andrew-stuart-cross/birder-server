﻿using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;

namespace Birder.Tests.Services;

public class EmailSenderTests
{
    [Fact]
    public void CreateMailMessage_Returns_Valid_SendGridMessage()
    {
        // Arrange
        var someOptions = Options.Create(new AuthMessageSenderOptions());
        var sut = new EmailSender(someOptions);

        var templateId = "TestId";
        var recipent = "test@test.com";
        var model = new { test = "test" };

        // Act
        var result = sut.CreateMailMessage(templateId, recipent, model);

        // Assert
        Assert.IsType<SendGridMessage>(result);
        result.TemplateId.ShouldEqual(templateId);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Returns_ArgumentException_If_templateId_Argument_Is_Null_Or_Empty(string templateId)
    {
        // Arrange
        var someOptions = Options.Create(new AuthMessageSenderOptions());
        var service = new EmailSender(someOptions);

        // Act & Assert
        var result = Assert.Throws<ArgumentException>(() => service.CreateMailMessage(templateId, "recipient", It.IsAny<object>()));
        Assert.Equal("The argument is null or empty (Parameter 'templateId')", result.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Returns_ArgumentException_If_recipient_Argument_Is_Null_Or_Empty(string recipient)
    {
        // Arrange
        var someOptions = Options.Create(new AuthMessageSenderOptions());
        var service = new EmailSender(someOptions);

        //Act & Assert
        var result = Assert.Throws<ArgumentException>(() => service.CreateMailMessage("templateId", recipient, It.IsAny<object>()));
        Assert.Equal("The argument is null or empty (Parameter 'recipient')", result.Message);
    }

    [Fact]
    public void Returns_ArgumentException_If_model_Argument_Is_Null()
    {
        // Arrange
        var someOptions = Options.Create(new AuthMessageSenderOptions());
        var service = new EmailSender(someOptions);

        //Act & Assert
        var result = Assert.Throws<ArgumentException>(() => service.CreateMailMessage("templateId", "recipient", null));
        Assert.Equal("The argument is null (Parameter 'model')", result.Message);
    }
}