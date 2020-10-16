using System;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Core.Models.Dtos.Booking;
using VacationRental.Core.Models.Dtos.Calendar;
using VacationRental.Core.Models.Dtos.Rental;
using VacationRental.Core.Models.Dtos.Shared;
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
            DateTime dt1 = DateTime.UtcNow.AddDays(1).Date;
            DateTime dt2 = DateTime.UtcNow.AddDays(2).Date;
            DateTime dt3 = DateTime.UtcNow.AddDays(3).Date;
            DateTime dt4 = DateTime.UtcNow.AddDays(4).Date;

            RentalBindingDto postRentalRequest = new RentalBindingDto
            {
                Units = 2,
                PreparationTimeInDays = 0
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
                Start = dt1
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
                Start = dt2
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

                Assert.Equal(dt1, getCalendarResult.Dates[1].Date);
                Assert.Single(getCalendarResult.Dates[1].Bookings);
                Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Id == postBooking1Result.Id);

                Assert.Equal(dt2, getCalendarResult.Dates[2].Date);
                Assert.Equal(2, getCalendarResult.Dates[2].Bookings.Count);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking1Result.Id);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking2Result.Id);

                Assert.Equal(dt3, getCalendarResult.Dates[3].Date);
                Assert.Single(getCalendarResult.Dates[3].Bookings);
                Assert.Contains(getCalendarResult.Dates[3].Bookings, x => x.Id == postBooking2Result.Id);

                Assert.Equal(dt4, getCalendarResult.Dates[4].Date);
                Assert.Empty(getCalendarResult.Dates[4].Bookings);
            }
        }
    }
}