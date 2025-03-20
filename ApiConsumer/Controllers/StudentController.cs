using ApiConsumer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace ApiConsumer.Controllers
{
    public class StudentController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7026");
        private readonly HttpClient _client;

        public StudentController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await _client.GetAsync("/api/stud/get");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var students = JsonConvert.DeserializeObject<List<StudentViewModel>>(data);
                return View(students);
            }            
            return NotFound();
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(StudentViewModel student)
        {
            var json = JsonConvert.SerializeObject(student);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("/api/stud/post", data);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            HttpResponseMessage response = await _client.GetAsync($"/api/stud/get/{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var student = JsonConvert.DeserializeObject<StudentViewModel>(data);
                return View(student);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(StudentViewModel student)
        {
            var json = JsonConvert.SerializeObject(student);//Here the object student is serialized to json
            var data = new StringContent(json, Encoding.UTF8, "application/json");//Here the json is converted to string content to post in api
            HttpResponseMessage response = await _client.PutAsync("/api/stud/put", data);//Here the data is posted to api
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            StudentViewModel student = new StudentViewModel();
            HttpResponseMessage response =await _client.GetAsync($"/api/stud/get/{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                student = JsonConvert.DeserializeObject<StudentViewModel>(data);
                return View(student);
            }
            return View();
        }
        [HttpPost,ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(StudentViewModel student)
        {
            HttpResponseMessage response = await _client.DeleteAsync($"/api/stud/delete/{student.Id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
