using CpCinemaBlazor.ApiRequest.Model;



namespace CpCinemaBlazor.ApiRequest
{
    public class SingletoneUser
    {
        private CurUser _currentUser;

        public event Action OnAuthStateChanged;

        public CurUser CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                NotifyAuthStateChanged();
            }
        }

        public bool IsAuthenticated => _currentUser != null;

        public void Login(CurUser user)
        {
            CurrentUser = user;
        }

        public void Logout()
        {
            CurrentUser = null;
        }

        private void NotifyAuthStateChanged()
        {
            OnAuthStateChanged?.Invoke();
        }
    }
}
