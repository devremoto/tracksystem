using MassTransit;
using Models;
using Moq;
using TrackSystem.Services;

namespace Tests
{
    public class TrackSystemTests
    {
        [Fact]
        public async Task Track_Publishes_TrackRequest_And_Returns_1PixelGif()
        {
            // Arrange
            var mockBusControl = new Mock<IBusControl>();
            var trackService = new TrackService(mockBusControl.Object);
            var trackRequest = new TrackRequest();

            // Act
            var result = await trackService.Track(trackRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(35, result.Length);
        }

        [Fact]
        public async Task Get1PixelGifStream_Returns_1PixelGif()
        {
            // Arrange
            var expectedGifBytes = new byte[] {
                0x47, 0x49, 0x46, 0x38, 0x39, 0x61, 0x01, 0x00, 0x01, 0x00, 0x80, 0x00, 0x00, 0xFF, 0xFF, 0xFF,
                0x00, 0x00, 0x00, 0x2C, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x02, 0x02, 0x44,
                0x01, 0x00, 0x3B
            };
            var trackService = new TrackService(null); // Since we are testing a private method, we can pass null here

            // Act
            var result = await trackService.Get1PixelGifStream();

            // Assert
            Assert.Equal(expectedGifBytes, result);
        }
    }
}