namespace Zadanie.Models
{
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class CustomerStatus
    {
        public CustomerStatus()
        {
            this.Customer = new HashSet<Customer>();
        }
    
        public int ID { get; set; }
        [DisplayName("Status klienta")]
        public string Status { get; set; }
    
        public virtual ICollection<Customer> Customer { get; set; }
    }
}