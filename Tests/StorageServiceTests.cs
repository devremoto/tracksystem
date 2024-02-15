using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Models;
using Moq;
using System.Linq;

namespace TrackSystem.Consumer.Services.Tests
{
    public class StorageServiceTests
    {


        private readonly IConfiguration _configuration;
        private readonly string _filePath;
        private readonly StorageService _storageService;

        public StorageServiceTests()
        {
            var builder = WebApplication.CreateBuilder();
            _configuration = builder.Configuration;
            _filePath = builder.Configuration["StorageSettings:VisitLogFilePath"];
            _storageService = new StorageService(_configuration);
        }
        [Fact]
        public void Constructor_CreatesLogFile_IfNotExist()
        {
            // Act & Assert
            Assert.True(File.Exists(_filePath));
        }

        [Fact]
        public async Task StoreInformationAsync_WritesToLogFile()
        {
            // Arrange
            var trackRequest = new TrackRequest
            {
                Referrer = "http://example.com",
                UserAgent = $"Test User Agent {DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")}",
                IpAddress = "127.0.0.1"
            };

            // Act
            await _storageService.StoreInformationAsync(trackRequest);

            // Assert
            var lines = await File.ReadAllLinesAsync(_filePath);
            var expectedLine = $"{DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")} | {trackRequest.Referrer} | {trackRequest.UserAgent} | {trackRequest.IpAddress}";
            Assert.Contains(expectedLine, lines.LastOrDefault());
        }

        [Fact]
        public async Task Consume_Invokes_StoreInformationAsync()
        {
            // Arrange
            var trackRequest = new TrackRequest
            {
                Referrer = "http://example.com",
                UserAgent = $"Test User Agent {DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")}",
                IpAddress = "127.0.0.1"
            };
            var context = Mock.Of<ConsumeContext<TrackRequest>>(x => x.Message == trackRequest);

            // Act
            await _storageService.Consume(context);

            // Assert
            var lines = await File.ReadAllLinesAsync(_filePath);
            var expectedLine = $"{DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")} | {trackRequest.Referrer} | {trackRequest.UserAgent} | {trackRequest.IpAddress}";
            Assert.Contains(expectedLine, lines.LastOrDefault());
        }
    }
}
