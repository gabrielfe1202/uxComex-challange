namespace UXComex_challenge.Domain.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime RegisteredAt { get; set; }

        public Client()
        {
            this.Id = 0;
            this.Name = "";
            this.Email = "";
            this.PhoneNumber = "";
        }
    }
}
