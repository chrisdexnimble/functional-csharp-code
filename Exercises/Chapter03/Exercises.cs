using System;
using System.Collections;
using System.Collections.Specialized;
//using System.Configuration;
using LaYumba.Functional;
using LaYumba.Functional.Option;
using Enum = LaYumba.Functional.Enum;
using static LaYumba.Functional.F;
using System.Collections.Generic;
using System.Linq;

namespace Exercises.Chapter3
{
    public static class Exercises
    {
        // 1 Write a generic function that takes a string and parses it as a value of an enum. It
        // should be usable as follows:
        // Enum.Parse<DayOfWeek>("Friday") // => Some(DayOfWeek.Friday)
        // Enum.Parse<DayOfWeek>("Freeday") // => None

        public static Option<T> ChrisParse<T>(this string value) where T : struct =>
            System.Enum.TryParse<T>(value, false, out var result) ? Some(result) : F.None;


        // 2 Write a Lookup function that will take an IEnumerable and a predicate, and
        // return the first element in the IEnumerable that matches the predicate, or None
        // if no matching element is found. Write its signature in arrow notation:
        // bool isOdd(int i) => i % 2 == 1;
        // new List<int>().Lookup(isOdd) // => None
        // new List<int> { 1 }.Lookup(isOdd) // => Some(1)


        public static Option<T> ChrisLookup<T>(this IEnumerable<T> t, Func<T, bool> predicate)
        {
            if (!t.Any()) return F.None;

            var r = t.FirstOrDefault(x => predicate(x) == true);
            return r != null ? Some(r) : F.None;
        }

    // 3 Write a type Email that wraps an underlying string, enforcing that it’s in a valid
      // format. Ensure that you include the following:
      // - A smart constructor
      // - Implicit conversion to string, so that it can easily be used with the typical API
      // for sending emails
      public readonly struct ChrisEmail
      {
          private readonly string _value;

          private ChrisEmail(string value)
          {
              if (!IsValid(value))
                  throw new ArgumentException("Invalid email address");

              _value = value;
          }

          public override string ToString() => _value;

          private static bool IsValid(string value) =>
              (!string.IsNullOrWhiteSpace(value) && value.Contains("@"));

          public static Option<ChrisEmail> Of(string value) =>
              IsValid(value) ? Some(new ChrisEmail(value)) : F.None;

          public static implicit operator string(ChrisEmail email)
              => email._value.ToString();
      }

      // 4 Take a look at the extension methods defined on IEnumerable inSystem.LINQ.Enumerable.
      // Which ones could potentially return nothing, or throw some
      // kind of not-found exception, and would therefore be good candidates for
      // returning an Option<T> instead?
   }

   // 5.  Write implementations for the methods in the `AppConfig` class
   // below. (For both methods, a reasonable one-line method body is possible.
   // Assume settings are of type string, numeric or date.) Can this
   // implementation help you to test code that relies on settings in a
   // `.config` file?
   public class AppConfig
   {
      NameValueCollection source;

      //public AppConfig() : this(ConfigurationManager.AppSettings) { }

      public AppConfig(NameValueCollection source)
      {
         this.source = source;
      }

      public Option<T> Get<T>(string name)
      {
         throw new NotImplementedException("your implementation here...");
      }

      public T Get<T>(string name, T defaultValue)
      {
         throw new NotImplementedException("your implementation here...");
      }
   }
}
