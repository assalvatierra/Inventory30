using CoreLib.DTO.Common.Dialog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InvWeb.Pages.Shared.Components.Dialog
{
    [ViewComponent(Name = "Dialog")]
    public class DefaultViewComponent : ViewComponent
    {
        public DefaultViewComponent()
        {
        }

        public List<DialogItems> DialogItemList { get; set; }

        public async Task<IViewComponentResult> InvokeAsync(List<DialogItems> items, string selector)
        {

            DialogItemList = new List<DialogItems>();

            DialogItemList.Add(new DialogItems()
            {
                Id = 1,
                Name = "Item 1",
                Description = "Item Description"
            });

            DialogItemList.Add(new DialogItems()
            {
                Id = 2,
                Name = "Item 2",
                Description = "Item Description 2"
            });

            DialogItemList.Add(new DialogItems()
            {
                Id = 3,
                Name = "Item 3",
                Description = "Item Description 3"
            });

            DialogItemList = items;

            ViewBag.Selector = selector;

            return View(DialogItemList);
        }
    }

}