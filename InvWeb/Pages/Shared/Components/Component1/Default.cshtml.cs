using CoreLib.Models.Inventory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace InvWeb.Pages.Shared.Components.Component1
{
    [ViewComponent(Name = "Component1")]
    public class Component1ViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public Component1ViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            //var data = await _context.InvStores.ToListAsync();
            return View();
        }
    }
}
