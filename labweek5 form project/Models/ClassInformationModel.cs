using System.ComponentModel.DataAnnotations;

namespace labweek5_form_project.Models
{
    public class ClassInformationModel
    {
        // Statik sayacı kaldırdık

        public ClassInformationModel()
        {
            // Boş constructor - ID için özel bir atama yapmıyoruz
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Class Name is required")]
        public string ClassName { get; set; }

        [Required(ErrorMessage = "Student Count is required")]
        [Range(1, 1000, ErrorMessage = "Student Count must be between 1 and 1000")]
        public int StudentCount { get; set; }

        public string Description { get; set; }
    }
}