using System;

namespace API.Extensions;

public static class DateTimeExtensions
{
  public static int CalculateAge(this DateOnly dob)
  {
    var today = DateOnly.FromDateTime(DateTime.Now);

    var age = today.Year - dob.Year;

    // If the birthday for the current year hasn't occurred yet,
    // the age is adjusted by subtracting one.
    if (dob > today.AddYears(-age)) age--;

    return age;
  }
}
