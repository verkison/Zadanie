namespace Zadanie.Models
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class Customer
    {
        public Customer()
        {
            this.Email = new HashSet<Email>();
            this.Phone = new HashSet<Phone>();
        }

        public int ID { get; set; }
        [Required(ErrorMessage = "To pole nie mo�e by� puste")]
        [DisplayName("Imi�")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "To pole nie mo�e by� puste")]
        [DisplayName("Nazwisko")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "To pole nie mo�e by� puste")]
        [DisplayName("Adres zamieszkania")]
        public string Address { get; set; }
        [DisplayName("Status klienta")]
        public int StatusID { get; set; }
        [DisplayName("Ilo�� emaili")]
        public int EmailQuantity { get; set; }
        [DisplayName("Ilo�� numer�w telefonu")]
        public int PhoneQuantity { get; set; }

        public virtual CustomerStatus CustomerStatus { get; set; }
        public virtual ICollection<Email> Email { get; set; }
        public virtual ICollection<Phone> Phone { get; set; }
    }
}