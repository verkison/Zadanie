namespace Zadanie.Models
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class Phone
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "To pole nie mo¿e byæ puste")]
        [DisplayName("Numer telefonu")]
        [Range(0, 999999999)]
        public string PhoneContent { get; set; }
        [DisplayName("Dane klienta")]
        public int CustomerID { get; set; }

        [DisplayName("Dane klienta")]
        public virtual Customer Customer { get; set; }
    }
}