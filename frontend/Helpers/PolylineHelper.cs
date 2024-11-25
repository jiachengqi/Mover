using GoogleMapsComponents.Maps;

namespace frontend.Helpers;

public static class PolylineHelper
{
    public static List<LatLngLiteral> DecodePolylinePoints(string encodedPoints)
    {
        if (string.IsNullOrEmpty(encodedPoints))
            return null;

        var poly = new List<LatLngLiteral>();
        char[] polylineChars = encodedPoints.ToCharArray();
        int index = 0;

        int currentLat = 0;
        int currentLng = 0;

        while (index < polylineChars.Length)
        {
            int sum = 0;
            int shifter = 0;
            int next5Bits;

            do
            {
                next5Bits = polylineChars[index++] - 63;
                sum |= (next5Bits & 31) << shifter;
                shifter += 5;
            } while (next5Bits >= 32 && index < polylineChars.Length);

            if (index >= polylineChars.Length)
                break;

            currentLat += (sum & 1) != 0 ? ~(sum >> 1) : (sum >> 1);
            
            sum = 0;
            shifter = 0;

            do
            {
                next5Bits = polylineChars[index++] - 63;
                sum |= (next5Bits & 31) << shifter;
                shifter += 5;
            } while (next5Bits >= 32 && index < polylineChars.Length);

            if (index >= polylineChars.Length && next5Bits >= 32)
                break;

            currentLng += (sum & 1) != 0 ? ~(sum >> 1) : (sum >> 1);

            var point = new LatLngLiteral
            {
                Lat = currentLat / 1E5,
                Lng = currentLng / 1E5
            };
            poly.Add(point);
        }

        return poly;
    }
}