using System.ComponentModel.DataAnnotations;

namespace labweek5_form_project.Models
{
    public class ClassInformationTable
    {
        // Filtrelenmiş verileri göstermek için kullanılacak sınıf
        public int Id { get; set; } // Arka planda kullanılacak ama tabloda görünmeyecek
        public string ClassName { get; set; }
        public int StudentCount { get; set; }
        public string Description { get; set; }
    }
}