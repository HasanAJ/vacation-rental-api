using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Core.Models.Dtos.Rental;
using VacationRental.Core.Models.Dtos.Shared;
using Xunit;

namespace VacationRental.IntegrationTests.Rental
{
    [Collection("Integration")]
    public class PostRentalTests
    {
        private readonly HttpClient _client;

        public PostRentalTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostRental_ThenAGetReturnsTheCreatedRental()
        {
            RentalBindingDto request = new RentalBindingDto
            {
                Units = 25,
                PreparationTimeInDays = 1
            };

            ResourceIdDto postResult;
            using (HttpResponseMessage postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdDto>();
            }

            using (HttpResponseMessage getResponse = await _client.GetAsync($"/api/v1/rentals/{postResult.Id}"))
            {
                Assert.True(getResponse.IsSuccessStatusCode);

                RentalDto getResult = await getResponse.Content.ReadAsAsync<RentalDto>();
                Assert.Equal(request.Units, getResult.Units);
                Assert.Equal(request.PreparationTimeInDays, getResult.PreparationTimeInDays);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostRental_ThenAPutThenAGetReturnsTheCreatedRental()
        {
            RentalBindingDto request = new RentalBindingDto
            {
                Units = 25,
                PreparationTimeInDays = 1
            };

            ResourceIdDto postResult;
            using (HttpResponseMessage postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdDto>();
            }

            RentalBindingDto putRequest = new RentalBindingDto
            {
                Units = 20,
                PreparationTimeInDays = 2
            };

            using (HttpResponseMessage putResponse = await _client.PutAsJsonAsync($"/api/v1/rentals/{postResult.Id}", putRequest))
            {
                Assert.True(putResponse.IsSuccessStatusCode);
            }

            using (HttpResponseMessage getResponse = await _client.GetAsync($"/api/v1/rentals/{postResult.Id}"))
            {
                Assert.True(getResponse.IsSuccessStatusCode);

                RentalDto getResult = await getResponse.Content.ReadAsAsync<RentalDto>();
                Assert.Equal(putRequest.Units, getResult.Units);
                Assert.Equal(putRequest.PreparationTimeInDays, getResult.PreparationTimeInDays);
            }
        }
    }
}