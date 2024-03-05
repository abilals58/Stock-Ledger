using System.ComponentModel.DataAnnotations;

namespace Ledger.Ledger.Web.Models
{
    public class User // User is the main component of the ledger who refers to the people trading in the scope of ledger system. A user has UserId (primary key), Name,
                      // Surname, Username, Email, Password, Phone fields.
    {
        [Key]
        public  int UserId { set; get; } // a randomly generated integer 
        public  string Name { set; get; }
        public  string Surname { set; get; }
        public  string UserName { set; get; }
        public  string Email { set; get; }
        public  string Password { set; get; }
        public  string Phone { set; get; }
        
        public double Budget { set; get; }

        public User()
        {
        }

        public User(int userId, string name, string surname, string userName, string email, string password, string phone, double budget)
        {
            UserId = userId;
            Name = name;
            Surname = surname;
            UserName = userName;
            Email = email;
            Password = password;
            Phone = phone;
            Budget = budget;
        }
        
        public override string ToString()
        {
            return $"User ID: {UserId}, Name: {Name}, Surname: {Surname}, UserName: {UserName}, Email: {Email}, Phone: {Phone}, Budget: {Budget}";
        }
    }
}