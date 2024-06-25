using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IWema.Domain.Entity;

public class UpcomingEventEntity : EntityBase
{
    public string NameOfEvent { get; private set; }
    public string Date { get; private set; }
    public string ImageLocation { get; private set; }

    public UpcomingEventEntity() { }
    public UpcomingEventEntity(string nameOfEvent, string date, string imageLocation)
    {
        NameOfEvent = nameOfEvent;
        Date = date;
        ImageLocation = imageLocation;

    }

    public static UpcomingEventEntity Create(string nameOfEvent, string date, string imageLocation)
    {
        return new UpcomingEventEntity(nameOfEvent, date, imageLocation);
    }

    public void Update(string nameOfEvent, string date, string imageLocation)
    {
        NameOfEvent = nameOfEvent;
        Date = date;
        ImageLocation = imageLocation;
    }

    public void UpdateNameOfEvent(string nameOfEvent)
    {
        NameOfEvent = nameOfEvent;
    }

    public void UpdateDate(string date)
    {
        Date = date;
    }

    public void UpdateImageLocation(string imageLocation)
    {
        ImageLocation = imageLocation;
    }
}

