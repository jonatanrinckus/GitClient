namespace GitClient.ViewModels
{
	public class MainViewModel : BaseViewModel
    {
        private string _status;

        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

	    private string _clock;

	    public string Clock
	    {
		    get { return _clock; }
		    set { SetProperty(ref _clock, value); }
	    }

	    
    }
}
