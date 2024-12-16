
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using HelloWorld.Model;
using SQLite;

namespace HelloWorld.ViewModels;

public class MainPageViewModel : BindableObject
{

    public ObservableCollection<ActivityDay> ActivityDays { get; set; }

    public MainPageViewModel()
    {
        ActivityDays = new ObservableCollection<ActivityDay>(GenerateActivityData(null));
    }

    public List<ActivityDay> GenerateActivityData(SQLiteConnection? db)
    {
        var data = new List<ActivityDay>();
        var startDate = DateTime.Today.AddDays(-365/2);

        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "activity.db");
		
        if(db == null) 
        {
            Console.WriteLine("Got a null sqlite connection, connecting again...");
            db = new SQLiteConnection(dbPath);
        }

        var random = new Random();
        for(int i=0; i<(365/2); i++)
        {
            DateTime day = startDate.AddDays(i+1);

            var allItems = db.Table<ActivityDay>().ToList();

            var foundActivityThisDay = false;
            foreach(var item in allItems)
            {
                if(item.Date == day)
                {
                    data.Add(item);
                    foundActivityThisDay = true;
                }
            }

            if(!foundActivityThisDay)
            {
                data.Add(new ActivityDay{Date = DateTime.Now, Intensity=-1});
            }

        }

        OnPropertyChanged(nameof(ActivityDays));
        return data;
    }

}
