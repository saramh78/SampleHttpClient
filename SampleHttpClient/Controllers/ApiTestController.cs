using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SampleHttpClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiTestController : ControllerBase
    {
        public static string Token { get; set; }

        [HttpGet("TestLogin")]
        public async Task<ActionResult<Dto>> TestLogin()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:51663");

                var data = new Model()
                {
                    Password = "aA123456&",
                    Username = "Admin"
                };

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "Token");

                StringContent httpContent = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");


                var response = await client.PostAsync("/api/Authenticate/Login", httpContent);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.StatusCode.ToString());
                }

                var result = await response.Content.ReadAsStringAsync();

                var resultDto = JsonSerializer.Deserialize<Dto>(result);

                Token = resultDto.Token;

                return resultDto;


            }

        }


        [HttpGet("TestCategory")]
        public async Task<ActionResult<List<CategoryDto>>> TestCategory()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:51663");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
                client.DefaultRequestHeaders.Add("termianlId", "majid");
                client.DefaultRequestHeaders.Add("ccc", "fff");


                //    StringContent httpContent = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");


                var response = await client.GetAsync("/api/Category");

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.StatusCode.ToString());
                }

                var result = await response.Content.ReadAsStringAsync();

                var resultDto = JsonSerializer.Deserialize<List<CategoryDto>>(result);

                return resultDto;


            }

        }

    }

    public class Model
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class Dto
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("expiration")]
        public string Expiration { get; set; }
    }


    public class CategoryDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("parentId")]
        public int? ParentId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

}
