namespace Gr8FoodSystem
{
    public class Manager
    {
        public int UserID { get; set; }
        public string LoginID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string Role { get; set; }

        public Manager()
        {
            Role = "Manager";
        }

        public Manager(int userID, string loginID, string firstName,
                       string lastName, string email, string phoneNo)
        {
            UserID = userID;
            LoginID = loginID;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNo = phoneNo;
            Role = "Manager";
        }

        public string GetFullName()
        {
            return FirstName + " " + LastName;
        }

        public string GetWelcomeMessage()
        {
            return "Welcome, " + GetFullName() + "!";
        }

        public bool HasReplied(string managerReply)
        {
            return !string.IsNullOrWhiteSpace(managerReply);
        }

        public override string ToString()
        {
            return "[Manager] " + GetFullName() + " (" + LoginID + ")";
        }
    }
}