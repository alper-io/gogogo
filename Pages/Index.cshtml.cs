using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webapp.Models;
using System.Collections.Generic;

namespace webapp.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public ClassInformationModel ClassInfo { get; set; } = new();

        public static List<ClassInformationModel> ClassInformationList { get; set; } = new();

        [BindProperty]
        public int? EditId { get; set; } // Used to determine editing state

        public void OnGet(int? editId = null)
        {
            EditId = editId;

            if (EditId.HasValue)
            {
                // Prefill form with data for editing
                ClassInfo = ClassInformationList.Find(c => c.Id == EditId.Value) ?? new();
            }
        }

        public IActionResult OnPostAdd()
        {
            if (!ModelState.IsValid)
                return Page();

            ClassInformationList.Add(ClassInfo); // Add new item
            return RedirectToPage(); // Refresh the page
        }

        public IActionResult OnPostEdit()
        {
            if (!ModelState.IsValid || !EditId.HasValue)
                return Page();

            var existingItem = ClassInformationList.Find(c => c.Id == EditId.Value);
            if (existingItem != null)
            {
                existingItem.ClassName = ClassInfo.ClassName;
                existingItem.StudentCount = ClassInfo.StudentCount;
                existingItem.Description = ClassInfo.Description;
            }

            return RedirectToPage(); // Refresh the page
        }

        public IActionResult OnPostDelete(int deleteId)
        {
            ClassInformationList.RemoveAll(c => c.Id == deleteId); // Delete item
            return RedirectToPage(); // Refresh the page
        }
    }
}
