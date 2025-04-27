using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using labweek5_form_project.Models;
using System.Collections.Generic;
using System.Linq;

namespace labweek5_form_project.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public ClassInformationModel NewClass { get; set; } = new ClassInformationModel();

        public static List<ClassInformationModel> ClassList { get; set; } = new List<ClassInformationModel>();

        public bool IsEditing { get; set; } = false;
        public int EditingId { get; set; }

        // Yeni bir ID oluşturmak için yardımcı metot
        private int GetNextId()
        {
            // Eğer liste boşsa, 1'den başla
            if (ClassList.Count == 0)
                return 1;

            // Listenin en büyük ID'sini bul ve 1 ekle
            return ClassList.Max(x => x.Id) + 1;
        }

        public void OnGet()
        {
            // Sayfa yüklendiğinde yeni bir sınıf oluştur
            IsEditing = false;
            NewClass = new ClassInformationModel();
        }

        public IActionResult OnPostAdd()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Yeni bir nesne oluştur ve sıradaki ID'yi ata
            var classToAdd = new ClassInformationModel
            {
                Id = GetNextId(),
                ClassName = NewClass.ClassName,
                StudentCount = NewClass.StudentCount,
                Description = NewClass.Description
            };

            ClassList.Add(classToAdd);

            // Formu temizle
            NewClass = new ClassInformationModel();
            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            var item = ClassList.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                ClassList.Remove(item);
            }
            return RedirectToPage();
        }

        public IActionResult OnPostEdit(int id)
        {
            var classToEdit = ClassList.FirstOrDefault(x => x.Id == id);
            if (classToEdit != null)
            {
                // Form doldurmak için bilgileri sakla
                IsEditing = true;
                EditingId = id;

                // Var olan nesnenin değerlerini kopyala
                NewClass = new ClassInformationModel
                {
                    Id = classToEdit.Id,
                    ClassName = classToEdit.ClassName,
                    StudentCount = classToEdit.StudentCount,
                    Description = classToEdit.Description
                };
            }
            return Page();
        }

        public IActionResult OnPostUpdate()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var classToUpdate = ClassList.FirstOrDefault(x => x.Id == NewClass.Id);
            if (classToUpdate != null)
            {
                // Var olan nesneyi güncelle
                classToUpdate.ClassName = NewClass.ClassName;
                classToUpdate.StudentCount = NewClass.StudentCount;
                classToUpdate.Description = NewClass.Description;
                IsEditing = false;
            }

            // Formu temizle
            NewClass = new ClassInformationModel();
            return RedirectToPage();
        }
    }
}