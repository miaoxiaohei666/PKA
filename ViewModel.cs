using System.ComponentModel;

namespace PKA;

public class ViewModel : INotifyPropertyChanged
{
    private string _myText;

    public string MyText
    {
        get => _myText;
        set
        {
            _myText = value;
            OnPropertyChanged(nameof(MyText));
        }
    }

    public ViewModel(string myText)
    {
      _myText = myText;
    }

    public ViewModel()
    {
        _myText = "";
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
