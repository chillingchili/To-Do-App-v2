using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MauiApp1
{
    public class ToDoClass : INotifyPropertyChanged
    {
        public ToDoClass() { }

        int _item_id;
        string _item_name = string.Empty;
        string _item_description = string.Empty;
        string _status = string.Empty;
        int _user_id;

        public int item_id
        {
            get { return _item_id; }
            set { _item_id = value; OnPropertyChanged(nameof(item_id)); }
        }

        public int id
        {
            get { return _item_id; }
            set { _item_id = value; OnPropertyChanged(nameof(id)); }
        }

        public string item_name
        {
            get { return _item_name; }
            set { _item_name = value; OnPropertyChanged(nameof(item_name)); }
        }

        public string item_description
        {
            get { return _item_description; }
            set { _item_description = value; OnPropertyChanged(nameof(item_description)); }
        }

        public string status
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged(nameof(status)); }
        }

        public int user_id
        {
            get { return _user_id; }
            set { _user_id = value; OnPropertyChanged(nameof(user_id)); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
