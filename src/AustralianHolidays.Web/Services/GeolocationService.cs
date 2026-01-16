using Microsoft.JSInterop;

namespace AustralianHolidays.Web.Services;

public class GeolocationService(IJSRuntime jsRuntime)
{
    public async Task<GeolocationResult> GetCurrentPositionAsync()
    {
        try
        {
            var result = await jsRuntime.InvokeAsync<GeolocationPosition>("geolocation.getCurrentPosition");
            return new GeolocationResult(true, result.Latitude, result.Longitude, null);
        }
        catch (JSException ex)
        {
            return new GeolocationResult(false, 0, 0, ex.Message);
        }
    }

    public static State? MapCoordinatesToState(double latitude, double longitude)
    {
        // Australian state bounding boxes (approximate)
        // Format: (minLat, maxLat, minLon, maxLon)

        // ACT - Australian Capital Territory
        if (latitude is >= (-35.95) and <= (-35.12) &&
            longitude is >= 148.76 and <= 149.40)
        {
            return State.ACT;
        }

        // TAS - Tasmania (check before VIC due to latitude overlap)
        if (latitude is >= (-43.64) and <= (-39.58) &&
            longitude is >= 143.82 and <= 148.52)
        {
            return State.TAS;
        }

        // WA - Western Australia
        if (latitude is >= (-35.13) and <= (-13.69) &&
            longitude is >= 112.92 and <= 129.00)
        {
            return State.WA;
        }

        // NT - Northern Territory
        if (latitude is >= (-26.00) and <= (-10.97) &&
            longitude is >= 129.00 and <= 138.00)
        {
            return State.NT;
        }

        // SA - South Australia
        if (latitude is >= (-38.06) and <= (-26.00) &&
            longitude is >= 129.00 and <= 141.00)
        {
            return State.SA;
        }

        // QLD - Queensland
        if (latitude is >= (-29.18) and <= (-10.68) &&
            longitude is >= 138.00 and <= 153.55)
        {
            return State.QLD;
        }

        // NSW - New South Wales (excluding ACT which is checked first)
        if (latitude is >= (-37.51) and <= (-28.16) &&
            longitude is >= 141.00 and <= 153.64)
        {
            return State.NSW;
        }

        // VIC - Victoria
        if (latitude is >= (-39.16) and <= (-33.98) &&
            longitude is >= 140.96 and <= 150.03)
        {
            return State.VIC;
        }

        return null;
    }
}

public record GeolocationPosition(double Latitude, double Longitude);

public record GeolocationResult(bool Success, double Latitude, double Longitude, string? Error);
