using System;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Common.Constants;
using VacationRental.Core.Dtos.Booking;
using VacationRental.Core.Dtos.Rental;
using VacationRental.Core.Dtos.Shared;
using Xunit;

namespace VacationRental.IntegrationTests.Booking
{
    [Collection("Integration")]
    public class PostBookingTests
    {
        private readonly HttpClient _client;

        public PostBookingTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAGetReturnsTheCreatedBooking()
        {
            RentalBindingDto postRentalRequest = new RentalBindingDto
            {
                Units = 4,
                PreparationTimeInDays = 1
            };

            ResourceIdDto postRentalResult;
            using (HttpResponseMessage postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdDto>();
            }

            BookingBindingDto postBookingRequest = new BookingBindingDto
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = DateTime.UtcNow.AddDays(2)
            };

            ResourceIdDto postBookingResult;
            using (HttpResponseMessage postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
                postBookingResult = await postBookingResponse.Content.ReadAsAsync<ResourceIdDto>();
            }

            using (HttpResponseMessage getBookingResponse = await _client.GetAsync($"/api/v1/bookings/{postBookingResult.Id}"))
            {
                Assert.True(getBookingResponse.IsSuccessStatusCode);

                BookingDto getBookingResult = await getBookingResponse.Content.ReadAsAsync<BookingDto>();
                Assert.Equal(postBookingRequest.RentalId, getBookingResult.RentalId);
                Assert.Equal(postBookingRequest.Nights, getBookingResult.Nights);
                Assert.Equal(postBookingRequest.Start, getBookingResult.Start);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenThereIsOverbooking()
        {
            RentalBindingDto postRentalRequest = new RentalBindingDto
            {
                Units = 1,
                PreparationTimeInDays = 1
            };

            ResourceIdDto postRentalResult;
            using (HttpResponseMessage postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdDto>();
            }

            BookingBindingDto postBooking1Request = new BookingBindingDto
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = DateTime.UtcNow.AddDays(1)
            };

            using (HttpResponseMessage postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            BookingBindingDto postBooking2Request = new BookingBindingDto
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = DateTime.UtcNow.AddDays(2)
            };

            using (HttpResponseMessage postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.False(postBooking1Response.IsSuccessStatusCode);

                ApiErrorDto getBookingResult = await postBooking1Response.Content.ReadAsAsync<ApiErrorDto>();
                Assert.Equal(ApiCodeConstants.NOT_AVAILABLE, getBookingResult.Code);
            }
        }
    }
}