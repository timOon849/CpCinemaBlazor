namespace CpCinemaBlazor.ApiRequest.Services
{
    public class UserService
    {
        public User CurrentUser { get; private set; }

        public async Task LoadUserAsync()
        {
            CurrentUser = await GetUserFromApiAsync();
        }

        private async Task<User> GetUserFromApiAsync()
        {
            await Task.Delay(1000);
            return new User
            {
                id_User = 1,
                Name = "Test",
                AboutMe = "Test",
                Login = "Test",
                Password = "Test"
            };
        }
    }

    public class User
    {
        public int id_User { get; set; }
        public string Name { get; set; }
        public string AboutMe { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

    }
}
