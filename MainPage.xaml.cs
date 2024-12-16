using HelloWorld.Model;
using HelloWorld.ViewModels;
using SQLite;

namespace HelloWorld;

public partial class MainPage : ContentPage
{

	private SQLiteConnection db;
	private MainPageViewModel viewModel;

	public MainPage()
	{

		string dbPath = Path.Combine(FileSystem.AppDataDirectory, "activity.db");
		db = new SQLiteConnection(dbPath);

		if(!DoesTableExist("Activity"))
		{
			db.CreateTable<ActivityDay>();
		}

		//DropTable("Activity");

		InitializeComponent();
		DrawActivityGrid();
		
	}

	private void DrawActivityGrid()
	{

		viewModel = new MainPageViewModel();
		BindingContext = viewModel;

		var tileWidth = 380 / 26;

		ActivityCollection.ItemsSource = viewModel.ActivityDays;
		ActivityCollection.HeightRequest = tileWidth * 7;
		ActivityCollection.ItemTemplate = new DataTemplate(() =>
		{
			var frame = new Frame
			{
				CornerRadius = 0,
				Padding = 0,
				Margin = 0,
				BackgroundColor = Colors.Transparent,
				BorderColor = Colors.White,
				HeightRequest = tileWidth,
				WidthRequest = tileWidth
			};

			frame.SetBinding(BackgroundColorProperty, new Binding(
				"Intensity", 
				converter: new IntensityToColorConverter()
			));

			return frame;

		});


	}

	public void OnTextChanged(object sender, TextChangedEventArgs e)
	{
		if(e.NewTextValue != "")
			TextAreaOverlayLabel.Text = "";
		else
			TextAreaOverlayLabel.Text = "TODAY'S NOTE";
	}

	public void ClearStats(object sender, EventArgs args)
	{
		DropTable("Activity");
		db.CreateTable<ActivityDay>();

		viewModel.GenerateActivityData(db);
		DrawActivityGrid();
	}

	public void OnSubmit()
	{

		TextArea.Text = "";

		if(!DoesTupleExist("Activity", DateTime.Today))
		{
			db.Insert(new ActivityDay{
				Date = DateTime.Today,
				Intensity = 0,
			});
		}
		else
		{
			var itemInDb = db.Table<ActivityDay>().FirstOrDefault(i => i.Date == DateTime.Today);
			if(itemInDb != null)
			{
				itemInDb.Intensity ++;
				db.Update(itemInDb);
			}
		}

		viewModel.GenerateActivityData(db);
		DrawActivityGrid();
	}

	private bool DoesTableExist(string tableName)
	{
		var query = "SELECT name FROM sqlite_master WHERE type='table' AND name=?;";
		var result = db.ExecuteScalar<string>(query, tableName);
		return !string.IsNullOrEmpty(result);
	}

	private bool DoesTupleExist(string tableName, DateTime day)
	{
		var query = $"SELECT Id FROM {tableName} WHERE Date = ?";
		var result = db.ExecuteScalar<string>(query, day);
		return !string.IsNullOrEmpty(result);
	}

	private void DropTable(string tableName)
	{
		var query = $"DROP TABLE IF EXISTS {tableName};";
		db.ExecuteScalar<string>(query);	
	}

}

