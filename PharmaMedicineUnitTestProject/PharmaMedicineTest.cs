using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Pharma.Medicine.API;
using Pharma.Medicine.CoreEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PharmaMedicineUnitTestProject
{
    [Collection("Integration Tests")]
    public class PharmaMedicineTest
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public PharmaMedicineTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }


        [Fact]
        public async Task verify_get_medicines()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("api/v1/PharmaMedicine");

            response.EnsureSuccessStatusCode();

            Assert.NotNull(response.Content);

            string medicine_response = response.Content.ReadAsStringAsync().Result;

            List<MedicineModel> medicines = JsonConvert.DeserializeObject<List<MedicineModel>>(medicine_response);

            medicines.Should().NotBeEmpty();

            Assert.True(medicines.Count >= 1);

            Assert.Contains(medicines, (me) => me.Name.Equals("Dummy Cetzine"));

            Assert.Contains(medicines, (me) => me.Brand.Equals("Dummy Cipla"));
        }

        [Fact]
        public async Task verify_create_medicine()
        {
            var client = _factory.CreateClient();

            var medicine_model = new MedicineModel();
            medicine_model.Brand = "Ashu Brand";
            medicine_model.Expiry = DateTime.Now.AddDays(365);
            medicine_model.Name = "Ashutosh Medicine";
            medicine_model.Quantity = 200;
            medicine_model.Price = 3000.58f;
            medicine_model.Notes = "This is dummy medicine for test";

            string medicinepayload = JsonConvert.SerializeObject(medicine_model);

            var response = await client.PostAsync("api/v1/PharmaMedicine",
                                                  new StringContent(medicinepayload, Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();
            Assert.NotNull(response.Content);

            string create_response = response.Content.ReadAsStringAsync().Result;

            MedicineModel created_medicine = JsonConvert.DeserializeObject<MedicineModel>(create_response);

            created_medicine.Should().NotBeNull();

            Guid created_id = created_medicine.Id;
            
            created_medicine.Name.Should().Be("Ashutosh Medicine");
            created_medicine.Brand.Should().Be("Ashu Brand");
            created_medicine.Quantity.Should().Be(200);
            created_medicine.Price.Should().Be(3000.58f);

            //now delete the created medicine.

            var deleteclient = _factory.CreateClient();
            var delete_uri = string.Format("api/v1/PharmaMedicine?id={0}",
                created_id.ToString());

            var deletresp = await deleteclient.DeleteAsync(delete_uri);

            deletresp.EnsureSuccessStatusCode();
            Assert.NotNull(deletresp.Content);

            string delete_response = deletresp.Content.ReadAsStringAsync().Result;

            delete_response.Should().NotBeNullOrEmpty();
            delete_response.Should().Be("true");
        }

        [Theory]
        [InlineData("f64d6751-3045-4106-b3c7-5323055f5089")]
        public async Task verify_get_medicine_by_id(string id)
        {
            var client = _factory.CreateClient();
            var get_uri = string.Format("api/v1/PharmaMedicine/GetMedicineById/{0}", id);

            var getresp = await client.GetAsync(get_uri);

            getresp.EnsureSuccessStatusCode();
            Assert.NotNull(getresp.Content);

            string get_response = getresp.Content.ReadAsStringAsync().Result;
            get_response.Should().NotBeNullOrEmpty();
            MedicineModel medicine = JsonConvert.DeserializeObject<MedicineModel>(get_response);
            medicine.Should().NotBeNull();
            medicine.Id.Should().Be(new Guid(id));
            medicine.Name.Should().Be("Dummy Cetzine");
            medicine.Brand.Should().Be("Dummy Cipla");
            medicine.Quantity.Should().Be(100);
            medicine.Price.Should().Be(100.98f);
        }

        [Theory]
        [InlineData("Dummy Cetzine")]
        public async Task verify_get_medicine_by_name(string name)
        {
            var client = _factory.CreateClient();
            var get_uri = string.Format("api/v1/PharmaMedicine/GetMedicineByName/{0}", name);

            var getresp = await client.GetAsync(get_uri);

            getresp.EnsureSuccessStatusCode();
            Assert.NotNull(getresp.Content);

            string get_response = getresp.Content.ReadAsStringAsync().Result;
            get_response.Should().NotBeNullOrEmpty();
            List<MedicineModel> medicines = JsonConvert.DeserializeObject<List<MedicineModel>>(get_response);

            medicines.Should().NotBeEmpty();

            Assert.True(medicines.Count >= 1);

            Assert.Contains(medicines, (me) => me.Name.Equals("Dummy Cetzine"));

            Assert.Contains(medicines, (me) => me.Brand.Equals("Dummy Cipla"));

            var medicine = (from m in medicines
                            where m.Id.ToString().Equals("f64d6751-3045-4106-b3c7-5323055f5089")
                            select m).FirstOrDefault();

            medicine.Should().NotBeNull();
            medicine.Name.Should().Be("Dummy Cetzine");
            medicine.Brand.Should().Be("Dummy Cipla");
            medicine.Quantity.Should().Be(100);
            medicine.Price.Should().Be(100.98f);
        }

        [Theory]
        [InlineData("Dummy Cipla")]
        public async Task verify_get_medicine_by_brand(string brand)
        {
            var client = _factory.CreateClient();
            var get_uri = string.Format("api/v1/PharmaMedicine/GetMedicineByBrand/{0}", brand);

            var getresp = await client.GetAsync(get_uri);

            getresp.EnsureSuccessStatusCode();
            Assert.NotNull(getresp.Content);

            string get_response = getresp.Content.ReadAsStringAsync().Result;
            get_response.Should().NotBeNullOrEmpty();
            List<MedicineModel> medicines = JsonConvert.DeserializeObject<List<MedicineModel>>(get_response);

            medicines.Should().NotBeEmpty();

            Assert.True(medicines.Count >= 1);

            Assert.Contains(medicines, (me) => me.Name.Equals("Dummy Cetzine"));

            Assert.Contains(medicines, (me) => me.Brand.Equals("Dummy Cipla"));

            var medicine = (from m in medicines
                            where m.Id.ToString().Equals("f64d6751-3045-4106-b3c7-5323055f5089")
                            select m).FirstOrDefault();

            medicine.Should().NotBeNull();
            medicine.Name.Should().Be("Dummy Cetzine");
            medicine.Brand.Should().Be("Dummy Cipla");
            medicine.Quantity.Should().Be(100);
            medicine.Price.Should().Be(100.98f);
        }

        [Theory]
        [InlineData("f64d6751-3045-4106-b3c7-5323055f5089")]
        public async Task verify_update_medicine_notes(string id)
        {
            var client = _factory.CreateClient();
            var get_uri = string.Format("api/v1/PharmaMedicine/GetMedicineById/{0}", id);

            var getresp = await client.GetAsync(get_uri);

            getresp.EnsureSuccessStatusCode();
            Assert.NotNull(getresp.Content);

            string get_response = getresp.Content.ReadAsStringAsync().Result;

            MedicineModel medicine = JsonConvert.DeserializeObject<MedicineModel>(get_response);

            string oldnotes = medicine.Notes;

            string newnotes = "Updates Notes For Unit Test";

            medicine.Notes = newnotes;

            var update_client = _factory.CreateClient();

            string medicinepayload = JsonConvert.SerializeObject(medicine);

            var response = await client.PutAsync("api/v1/PharmaMedicine",
                                                  new StringContent(medicinepayload, Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();
            Assert.NotNull(response.Content);

            string update_response = response.Content.ReadAsStringAsync().Result;

            MedicineModel updated_medicine = JsonConvert.DeserializeObject<MedicineModel>(update_response);

            updated_medicine.Should().NotBeNull();
            updated_medicine.Notes.Should().Be("Updates Notes For Unit Test");

            //update the value back to original value again.
            medicine.Notes = oldnotes;

            var orignal_update_client = _factory.CreateClient();

            string orignal_medicinepayload = JsonConvert.SerializeObject(medicine);

            var org_update_response = await client.PutAsync("api/v1/PharmaMedicine",
                                                  new StringContent(orignal_medicinepayload, Encoding.UTF8, "application/json"));

            org_update_response.EnsureSuccessStatusCode();
            Assert.NotNull(org_update_response.Content);

            string back_to_orgupdate_response = org_update_response.Content.ReadAsStringAsync().Result;

            MedicineModel org_updated_medicine_ml = JsonConvert.DeserializeObject<MedicineModel>(back_to_orgupdate_response);

            org_updated_medicine_ml.Should().NotBeNull();
            org_updated_medicine_ml.Should().Be(oldnotes);
            
        }
    }

}
