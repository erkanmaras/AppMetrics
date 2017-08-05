// <copyright file="CounterFormattingTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using App.Metrics.Formatters.Json.Facts.Helpers;
using App.Metrics.Formatters.Json.Facts.TestFixtures;
using FluentAssertions;
using FluentAssertions.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace App.Metrics.Formatters.Json.Facts
{
    public class CounterFormattingTests : IClassFixture<MetricProviderTestFixture>
    {
        private readonly MetricsDataValueSource _metrics;
        private readonly ITestOutputHelper _output;
        private readonly IMetricsOutputFormatter _formatter;

        public CounterFormattingTests(ITestOutputHelper output, MetricProviderTestFixture fixture)
        {
            _output = output;
            _formatter = new JsonMetricsOutputFormatter();
            _metrics = fixture.CounterContext;
        }

        [Fact]
        public async Task Produces_expected_json()
        {
            // Arrange
            JToken result;
            var expected = MetricType.Counter.SampleJson();

            // Act
            using (var stream = new MemoryStream())
            {
                await _formatter.WriteAsync(stream, _metrics, Encoding.UTF8);

                result = Encoding.UTF8.GetString(stream.ToArray()).ParseAsJson();
            }

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public async Task Produces_valid_Json()
        {
            // Arrange
            string result;

            // Act
            using (var stream = new MemoryStream())
            {
                await _formatter.WriteAsync(stream, _metrics, Encoding.UTF8);

                result = Encoding.UTF8.GetString(stream.ToArray());
            }

            _output.WriteLine("Json Metrics Data: {0}", result);

            // Assert
            Action action = () => JToken.Parse(result);
            action.ShouldNotThrow<Exception>();
        }
    }
}