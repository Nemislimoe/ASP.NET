using System.ComponentModel.DataAnnotations;

namespace CampusActivityHub.Helpers;
public class FutureDateAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is DateTime dt)
        {
            return dt > DateTime.UtcNow;
        }
        return false;
    }
}

public class TimeRangeAttribute : ValidationAttribute
{
    private readonly int _minHour;
    private readonly int _maxHour;
    public TimeRangeAttribute(int minHour, int maxHour)
    {
        _minHour = minHour;
        _maxHour = maxHour;
    }

    public override bool IsValid(object value)
    {
        if (value is DateTime dt)
        {
            var hour = dt.ToLocalTime().Hour;
            return hour >= _minHour && hour < _maxHour;
        }
        return false;
    }
}
