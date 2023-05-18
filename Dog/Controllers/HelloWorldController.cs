using Microsoft.AspNetCore.Mvc;
using Dog.Models;



namespace Dog.Controllers
{
    public class HelloWorldController : Controller
    {
        private static List<DogViewModel> dogs = new List<DogViewModel>();

        public IActionResult Index()
        {
            return View(dogs);
        }

        public IActionResult Create()
        {
            var dog = new DogViewModel();
            return View(dog);    
        }

        public IActionResult CreateDog(DogViewModel dogVieModel)
        {
            dogs.Add(dogVieModel);
            return RedirectToAction(nameof(Index));
            //return View("Index");
        }

    }
}
