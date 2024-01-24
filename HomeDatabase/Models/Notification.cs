namespace HomeDatabase.Models
{
    public class Notification
    {

        public int UserId { get; set; }
        public string EventName { get; set; }
        public DateTime EventTime { get; set; }

    }
}
