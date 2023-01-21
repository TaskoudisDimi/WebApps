using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pizza.Models;

namespace Pizza.Pages
{
    public class PizzaModel : PageModel
    {
        public List<PizzasModel> fakePizzaDB = new List<PizzasModel>
        {
            new PizzasModel(){ImageTitle="Margerita",PizzaName="Margerita", BasePrice=2,TomatoSause=true,Cheese=true,FinalPrice=5 },
            new PizzasModel(){ImageTitle="Peperoni",PizzaName="Peperoni", BasePrice=6,TomatoSause=true,Cheese=true, Peperoni=true,FinalPrice=5 }

        };
        public void OnGet()
        {
        }
    }
}
