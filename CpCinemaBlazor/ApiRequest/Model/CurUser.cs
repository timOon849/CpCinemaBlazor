namespace CpCinemaBlazor.ApiRequest.Model
{
    public class CurUser
    {
        public int id_User { get; set; }
        public string Name { get; set; }
        public string AboutMe { get; set; }
        public bool isAdmin { get; set; }
        public string Token { get; set; }
    }
}
