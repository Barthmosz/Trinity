using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Trinity.API.Controllers;

namespace Trinity.Test.API.Controllers
{
    [TestFixture]
    public class HealthControllerTest
    {
        [Test]
        public void Ensure_HealthCheck_Returns200()
        {
            HealthController healthController = new();
            OkResult? result = healthController.CheckHealthAsync() as OkResult;
            Assert.That(result!.StatusCode, Is.EqualTo(200));
        }
    }
}
