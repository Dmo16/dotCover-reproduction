namespace TUnit_POC;

public class WeatherForecastTests
{
    [Test]
    public async Task Constructor_InitializesPropertiesCorrectly()
    {
        var date = new DateOnly(2026, 9, 1);
        var temperatureC = 25;
        var summary = "Warm";

        var forecast = new WeatherForecast(date, temperatureC, summary);

        await Assert.That(forecast.Date).IsEqualTo(date);
        await Assert.That(forecast.TemperatureC).IsEqualTo(25);
        await Assert.That(forecast.Summary).IsEqualTo("Warm");
    }

    [Test]
    [Arguments(0, 32)]
    [Arguments(-20, -3)]
    [Arguments(55, 130)]
    [Arguments(20, 67)]
    [Arguments(25, 76)]
    [Arguments(-40, -39)]
    [Arguments(100, 211)]
    [Arguments(-10, 15)]
    [Arguments(37, 98)]
    public async Task TemperatureF_CalculatesExpectedFahrenheit(int tempC, int expectedTempF)
    {
        var forecast = new WeatherForecast(DateOnly.FromDateTime(DateTime.Today), tempC, "Test");

        await Assert.That(forecast.TemperatureF).IsEqualTo(expectedTempF);
    }

    [Test]
    public async Task TemperatureF_FormulaMatchesExpectedCalculation()
    {
        var tempC = 18;
        var expectedTempF = 32 + (int)(tempC / 0.5556);

        var forecast = new WeatherForecast(DateOnly.FromDateTime(DateTime.Today), tempC, "Mild");

        await Assert.That(forecast.TemperatureF).IsEqualTo(expectedTempF);
    }

    [Test]
    public async Task Record_Equality_WhenValuesAreEqual()
    {
        var date = new DateOnly(2026, 1, 1);
        var forecast1 = new WeatherForecast(date, 20, "Mild");
        var forecast2 = new WeatherForecast(date, 20, "Mild");

        await Assert.That(forecast1).IsEqualTo(forecast2);
        await Assert.That(forecast1 == forecast2).IsTrue();
        await Assert.That(forecast1.GetHashCode()).IsEqualTo(forecast2.GetHashCode());
    }

    [Test]
    public async Task Record_Inequality_WhenValuesDiffer()
    {
        var date1 = new DateOnly(2026, 1, 1);
        var date2 = new DateOnly(2026, 1, 2);
        var forecast1 = new WeatherForecast(date1, 20, "Mild");
        var forecast2 = new WeatherForecast(date2, 20, "Mild");
        var forecast3 = new WeatherForecast(date1, 21, "Mild");
        var forecast4 = new WeatherForecast(date1, 20, "Warm");

        await Assert.That(forecast1 == forecast2).IsFalse();
        await Assert.That(forecast1 == forecast3).IsFalse();
        await Assert.That(forecast1 == forecast4).IsFalse();
    }

    [Test]
    public async Task Record_WithExpression_CreatesModifiedCopy()
    {
        var original = new WeatherForecast(new DateOnly(2026, 5, 10), 15, "Cool");
        var modified = original with { TemperatureC = 25, Summary = "Warm" };

        await Assert.That(original.TemperatureC).IsEqualTo(15);
        await Assert.That(original.Summary).IsEqualTo("Cool");
        await Assert.That(modified.Date).IsEqualTo(original.Date);
        await Assert.That(modified.TemperatureC).IsEqualTo(25);
        await Assert.That(modified.Summary).IsEqualTo("Warm");
    }

    [Test]
    public async Task Constructor_AllowsNullSummary()
    {
        var forecast = new WeatherForecast(new DateOnly(2026, 1, 1), 0, null);

        await Assert.That(forecast.Summary).IsNull();
        await Assert.That(forecast.TemperatureC).IsEqualTo(0);
        await Assert.That(forecast.TemperatureF).IsEqualTo(32);
    }

    [Test]
    public async Task Record_ToString_ContainsProperties()
    {
        var date = new DateOnly(2026, 9, 1);
        var forecast = new WeatherForecast(date, 22, "Balmy");
        var str = forecast.ToString();

        await Assert.That(str).Contains(date.ToString());
        await Assert.That(str).Contains("22");
        await Assert.That(str).Contains("Balmy");
        await Assert.That(str).Contains("71");
    }
}
