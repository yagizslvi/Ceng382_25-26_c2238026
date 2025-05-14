using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using labweek5_form_project.Helpers;
using labweek5_form_project.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace labweek5_form_project.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public ClassInformationModel NewClass { get; set; } = new ClassInformationModel();

        public static List<ClassInformationModel> ClassList { get; set; } = new List<ClassInformationModel>();

        // Filtreleme için kullanılacak propertyler
        [BindProperty(SupportsGet = true)]
        public string ClassNameFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? MinStudentCount { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? MaxStudentCount { get; set; }

        [BindProperty(SupportsGet = true)]
        public string DescriptionFilter { get; set; }

        // Sayfalama için kullanılacak propertyler
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        public int PageSize { get; set; } = 10; // Her sayfada gösterilecek kayıt sayısı
        public int TotalRecords { get; set; } // Toplam kayıt sayısı
        public int TotalPages { get; set; } // Toplam sayfa sayısı

        // Filtreleme ve sayfalama yapılmış liste - frontend için
        public List<ClassInformationTable> ClassTableView { get; set; }

        public bool IsEditing { get; set; } = false;
        public int EditingId { get; set; }

        // Statik constructor - 100 adet örnek veri oluşturmak için
        static IndexModel()
        {
            // Eğer liste boşsa, verileri doldur
            if (ClassList.Count == 0)
            {
                GenerateSampleData(100);
            }
        }

        // Örnek veri oluşturmak için yardımcı metot
        private static void GenerateSampleData(int count)
        {
            string[] classNames = { "Math", "Physics", "Chemistry", "Biology", "Computer Science",
                                    "Literature", "History", "Geography", "Art", "Music" };

            string[] descriptions = { "Beginner level class", "Advanced topics", "Research and applications",
                                    "Theoretical foundations", "Practical experiments", "Fundamental concepts" };

            Random random = new Random();

            for (int i = 1; i <= count; i++)
            {
                ClassList.Add(new ClassInformationModel
                {
                    Id = i,
                    ClassName = $"{classNames[random.Next(classNames.Length)]} {random.Next(101, 110)}",
                    StudentCount = random.Next(10, 101),
                    Description = descriptions[random.Next(descriptions.Length)]
                });
            }
        }

        // Yeni bir ID oluşturmak için yardımcı metot
        private int GetNextId()
        {
            // Eğer liste boşsa, 1'den başla
            if (ClassList.Count == 0)
                return 1;

            // Listenin en büyük ID'sini bul ve 1 ekle
            return ClassList.Max(x => x.Id) + 1;
        }

        public IActionResult OnGet()
        {
            var usernameCookie = Request.Cookies["username"];
            var tokenCookie = Request.Cookies["token"];
            var sessionIdCookie = Request.Cookies["session_id"];

            var usernameSession = HttpContext.Session.GetString("username");
            var tokenSession = HttpContext.Session.GetString("token");
            var sessionIdSession = HttpContext.Session.GetString("session_id");

            bool isAuthenticated =
                !string.IsNullOrEmpty(usernameCookie) &&
                !string.IsNullOrEmpty(tokenCookie) &&
                !string.IsNullOrEmpty(sessionIdCookie) &&
                usernameCookie == usernameSession &&
                tokenCookie == tokenSession &&
                sessionIdCookie == sessionIdSession;

            if (!isAuthenticated)
            {
                return Redirect("/Login"); // burada return gerekli!
            }

            IsEditing = false;
            NewClass = new ClassInformationModel();
            FilterAndPaginateResults();
            return Page(); // sayfanın devamı burada biter
        }


        private void FilterAndPaginateResults()
        {
            // Filtreleme işlemi
            var query = ClassList.AsQueryable();

            // Class Name filtresi
            if (!string.IsNullOrWhiteSpace(ClassNameFilter))
            {
                query = query.Where(c => c.ClassName.Contains(ClassNameFilter, StringComparison.OrdinalIgnoreCase));
            }

            // Minimum öğrenci sayısı filtresi
            if (MinStudentCount.HasValue)
            {
                query = query.Where(c => c.StudentCount >= MinStudentCount);
            }

            // Maximum öğrenci sayısı filtresi
            if (MaxStudentCount.HasValue)
            {
                query = query.Where(c => c.StudentCount <= MaxStudentCount);
            }

            // Açıklama filtresi
            if (!string.IsNullOrWhiteSpace(DescriptionFilter))
            {
                query = query.Where(c => c.Description.Contains(DescriptionFilter, StringComparison.OrdinalIgnoreCase));
            }

            // Filtreleme sonrası toplam kayıt sayısı
            TotalRecords = query.Count();

            // Toplam sayfa sayısı hesaplama
            TotalPages = (int)Math.Ceiling((double)TotalRecords / PageSize);

            // Geçersiz sayfa numarası kontrolü
            if (CurrentPage < 1)
                CurrentPage = 1;
            else if (CurrentPage > TotalPages && TotalPages > 0)
                CurrentPage = TotalPages;

            // Sayfalama işlemi
            var paginatedList = query
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            // ClassInformationTable modeline dönüştürme
            ClassTableView = paginatedList.Select(item => new ClassInformationTable
            {
                Id = item.Id, // ID arka planda kullanılacak
                ClassName = item.ClassName,
                StudentCount = item.StudentCount,
                Description = item.Description
            }).ToList();
        }

        public IActionResult OnPostAdd()
        {
            if (!ModelState.IsValid)
            {
                FilterAndPaginateResults();
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

            // Filtreleme ve sayfalama yap
            FilterAndPaginateResults();
            return Page();
        }

        public IActionResult OnPostUpdate()
        {
            if (!ModelState.IsValid)
            {
                FilterAndPaginateResults();
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


        public IActionResult OnPostExportJson(bool isFiltered, List<string> selectedColumns)
        {
            List<ClassInformationTable> exportData;

            if (isFiltered)
            {
                // Yalnızca filtrelenmiş veriyi dışa aktar
                FilterAndPaginateResults(); // ClassTableView doldurulur
                exportData = ClassTableView;
            }
            else
            {
                // Tüm veriyi dışa aktar
                exportData = ClassList.Select(item => new ClassInformationTable
                {
                    Id = item.Id,
                    ClassName = item.ClassName,
                    StudentCount = item.StudentCount,
                    Description = item.Description
                }).ToList();
            }

            // JSON formatında string elde et
            var jsonString = Helpers.Utils.Instance.ToJson(exportData, selectedColumns);

            // JSON dosyasını kullanıcıya döndür
            var byteArray = System.Text.Encoding.UTF8.GetBytes(jsonString);
            return File(byteArray, "application/json", "export.json");
        }

    }
}