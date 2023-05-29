using System.ComponentModel;

namespace ClassLibraryLogic.Models
{
    public class IsRoleAvailableModel: INotifyPropertyChanged
    {
        private bool _isAvaliable;
        public event PropertyChangedEventHandler PropertyChanged;
        public string RoleName { get; set; }
        public bool IsAvailable
        {
            get { return _isAvaliable; }
            set
            {
                if (_isAvaliable != value)
                {
                    _isAvaliable = value;
                    OnPropertyChanged(nameof(IsAvailable));
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
