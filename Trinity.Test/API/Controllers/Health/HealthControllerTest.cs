using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Trinity.API.Controllers.Health;

namespace Trinity.Test.API.Controllers.Health
{
    [TestFixture]
    public class HealthControllerTest
    {
        [Test]
        public void Ensure_HealthCheck_Returns200()
        {
            HealthController sut = new();
            OkResult? result = sut.CheckHealthAsync() as OkResult;
            Assert.That(result!.StatusCode, Is.EqualTo(200));
        }
    }
}
