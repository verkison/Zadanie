namespace Zadanie.Models
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class Email
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "To pole nie mo¿e byæ puste")]
        [DisplayName("Adres email")]
        public string EmailContent { get; set; }
        [DisplayName("Dane klienta")]
        public int CustomerID { get; set; }

        [DisplayName("Dane klienta")]
        public virtual Customer Customer { get; set; }
    }
}