using System;
using SQLite;

namespace HelloWorld.Model;

[Table("Activity")]
public class ActivityDay
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Intensity { get; set; }
}
