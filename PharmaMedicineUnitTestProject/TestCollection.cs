using Microsoft.AspNetCore.Mvc.Testing;
using Pharma.Medicine.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PharmaMedicineUnitTestProject
{
    [CollectionDefinition("Integration Tests")]
    public class TestCollection: ICollectionFixture<WebApplicationFactory<Startup>>
    {
    }
}
