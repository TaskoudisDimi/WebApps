using System.ComponentModel.DataAnnotations;

namespace HomeDatabase.Models
{
    public class TODOViewModel
    {

        public int ID { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DeliveryDate { get; set; }

        public bool Done { get; set; }

        public int userID { get; set; }



    }
}
