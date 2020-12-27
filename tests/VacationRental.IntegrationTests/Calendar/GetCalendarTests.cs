using System;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Core.Dtos.Booking;
using VacationRental.Core.Dtos.Calendar;
using VacationRental.Core.Dtos.Rental;
using VacationRental.Core.Dtos.Shared;
using Xunit;

namespace VacationRental.IntegrationTests.Calendar
{
    [Collection("Integration")]
    public class GetCalendarTests
    {
        private readonly HttpClient _client;

        public GetCalendarTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenGetCalendar_ThenAGetReturnsTheCalculatedCalendar()
        {
            DateTime dtStart = DateTime.UtcNow.Date;

            RentalBindingDto postRentalRequest = new RentalBindingDto
            {
                Units = 2,
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
                Nights = 2,
                Start = DateTime.UtcNow.AddDays(1).Date
            };

            ResourceIdDto postBooking1Result;
            using (HttpResponseMessage postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
                postBooking1Result = await postBooking1Response.Content.ReadAsAsync<ResourceIdDto>();
            }

            BookingBindingDto postBooking2Request = new BookingBindingDto
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = DateTime.UtcNow.AddDays(2).Date
            };

            ResourceIdDto postBooking2Result;
            using (HttpResponseMessage postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.True(postBooking2Response.IsSuccessStatusCode);
                postBooking2Result = await postBooking2Response.Content.ReadAsAsync<ResourceIdDto>();
            }

            using (HttpResponseMessage getCalendarResponse = await _client.GetAsync($"/api/v1/calendar?rentalId={postRentalResult.Id}&start={dtStart:yyyy-MM-dd}&nights=5"))
            {
                Assert.True(getCalendarResponse.IsSuccessStatusCode);

                CalendarDto getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<CalendarDto>();

                Assert.Equal(postRentalResult.Id, getCalendarResult.RentalId);
                Assert.Equal(5, getCalendarResult.Dates.Count);

                Assert.Equal(dtStart, getCalendarResult.Dates[0].Date);
                Assert.Empty(getCalendarResult.Dates[0].Bookings);
                Assert.Empty(getCalendarResult.Dates[0].PreparationTimes);

                Assert.Equal(dtStart.AddDays(1), getCalendarResult.Dates[1].Date);
                Assert.Single(getCalendarResult.Dates[1].Bookings);
                Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Id == postBooking1Result.Id);
                Assert.Empty(getCalendarResult.Dates[1].PreparationTimes);

                Assert.Equal(dtStart.AddDays(2), getCalendarResult.Dates[2].Date);
                Assert.Equal(2, getCalendarResult.Dates[2].Bookings.Count);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking1Result.Id);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking2Result.Id);
                Assert.Empty(getCalendarResult.Dates[2].PreparationTimes);

                Assert.Equal(dtStart.AddDays(3), getCalendarResult.Dates[3].Date);
                Assert.Single(getCalendarResult.Dates[3].Bookings);
                Assert.Contains(getCalendarResult.Dates[3].Bookings, x => x.Id == postBooking2Result.Id);
                Assert.Contains(getCalendarResult.Dates[3].PreparationTimes, x => x.Unit == 6);

                Assert.Equal(dtStart.AddDays(4), getCalendarResult.Dates[4].Date);
                Assert.Empty(getCalendarResult.Dates[4].Bookings);
                Assert.Contains(getCalendarResult.Dates[4].PreparationTimes, x => x.Unit == 7);
            }
        }
    }
}