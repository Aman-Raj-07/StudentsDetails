using First.Data;
using First.Models;
using First.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace First.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudent<Student> g;//g is the variable name of IStudent<Student> interface.

        public StudentController(IStudent<Student> genericRepository)
        {
            g = genericRepository;//We are assigning the value of genericRepository to g.  
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Student> s =await g.GetAllStudentsAsync();//calling gettallasync method and storing the result in s.
            return View(s);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Student s)
        {
            if (ModelState.IsValid)
            {
                await g.CreateAsync(s);//calling createasync method and passing s as parameter.
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Student s = await g.GetByIDAsync(id);//calling getbyidasync method and storing the result in s.
            return View(s);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Student s)
        {
            if (ModelState.IsValid)
            {
                await g.UpdateAsync(s);//calling updateasync method and passing s as parameter.
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Student s = await g.GetByIDAsync(id);//calling getbyidasync method and storing the result in s.
            return View(s);
        }
        [HttpPost,ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmation(int id) {
            bool check = await g.DeleteAsync(id);//calling deleteasync method and passing id as parameter.
            if (check)
            { 
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
//public async Task<IActionResult> Details(int id)
//{
//    Student s = await g.GetByIdAsync(id);//calling getbyidasync method and storing the result in s.
//    return View(s);
//}

/*public IActionResult Create()
{
    return View();
}
[HttpPost]
public async Task<IActionResult> Create(Student s)
{
    if (ModelState.IsValid)
    {
        await g.CreateAsync(s);//calling createasync method and passing s as parameter.
        return RedirectToAction("Index");
    }
    return View();
}*/