using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography.X509Certificates;
using Exercises.Chapter3;
using FluentAssertions;
using LaYumba.Functional;
using LaYumba.Functional.Option;
using Xunit;
using static LaYumba.Functional.F;
using static Exercises.Chapter3.Exercises;

namespace Exercises.Chapter03
{
    public class Ex3Tests
    {
        [Fact]
        public void Exercise3()
        {
            // 1 Write a generic function that takes a string and parses it as a value of an enum. It
            // should be usable as follows:
            // Enum.Parse<DayOfWeek>("Friday") // => Some(DayOfWeek.Friday)
            // Enum.Parse<DayOfWeek>("Freeday") // => None

            Option<DayOfWeek> s = "Friday".ChrisParse<DayOfWeek>();
            s.Should().Be(Some(DayOfWeek.Friday)); 

            Option<DayOfWeek> n = "Freeday".ChrisParse<DayOfWeek>();
            // why can't i assert this as none?
            //n.Should().Be(F.None);
            n.isNone.Should().BeTrue();
        }

        [Fact]
        public void Lookup()
        {
            // 2 Write a Lookup function that will take an IEnumerable and a predicate, and
            // return the first element in the IEnumerable that matches the predicate, or None
            // if no matching element is found. Write its signature in arrow notation:
            // bool isOdd(int i) => i % 2 == 1;
            // new List<int>().Lookup(isOdd) // => None
            // new List<int> { 1 }.Lookup(isOdd) // => Some(1)
            var nums = new[] { 1, 2, 3, 4, 5 };
            nums.ChrisLookup((x) => x == 3)
                .Should().Be(Some(3));

            bool isOdd(int i) => i % 2 == 1;
            
            new List<int>().ChrisLookup(isOdd)
                .isNone.Should().BeTrue();

            new List<int> { 1 }.ChrisLookup(isOdd)
                .Should().Be(Some(1));
        }

        [Fact]
        public void SmartConstructor()
        {
            //3 Write a type Email that wraps an underlying string, enforcing that it’s in a valid
            // format. Ensure that you include the following:
            // - A smart constructor
            // - Implicit conversion to string, so that it can easily be used with the typical API
            // for sending emails

            ChrisEmail.Of("Chris@Here.com").Should().BeOfType<Option<ChrisEmail>>();
            ChrisEmail.Of("Chris@Here.com").Match(() => Assert.False(true), x => (x == "Chris@Here.com").Should().BeTrue());

            ChrisEmail.Of("ChrisNotHere.com").isNone.Should().BeTrue();
        }

        [Fact]
        public void AppConfig()
        {
            var configValues = new NameValueCollection(3);
            configValues.Add("FeatureName", "MyFeature");
            configValues.Add("FeatureOn", "false");
            configValues.Add("FeatureValue", "3");
            configValues.Add("FeatureDate", "1-Oct-2020");
            var conf = new ChrisAppConfig(configValues);
            
            conf.Get<string>("FeatureName").Match(() => Assert.False(true, "Should match"), s => s.Should().Be("MyFeature"));
            conf.Get<string>("NotFound").Match(() => Assert.True(true, "Should be none"), s=> Assert.False(true, "Should not match"));

            conf.Get<bool>("FeatureOn").Match(() => Assert.False(true, "Should match"), b => b.Should().BeFalse());
            conf.Get<int>("FeatureValue").Match(() => Assert.False(true, "Should match"), n => n.Should().Be(3));
            conf.Get<DateTime>("FeatureDate").Match(() => Assert.False(true, "Should match"), d => d.Should().Be(DateTime.Parse("1-Oct-2020")));
        }
    }
}
