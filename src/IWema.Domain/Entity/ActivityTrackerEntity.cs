//namespace IWema.Domain.Entity;

//public class ActivityTrackerEntity
//{
//    public int Id { get; set; }
//    public string Feature { get; set; }
//    public DateTime Date { get; set; }
//    public int VisitCount { get; set; }
//}


using System;
using System.ComponentModel.DataAnnotations;

namespace IWema.Domain.Entity
{
    public class ActivityTrackerEntity
    {
        [Key]
        public int Id { get; private set; }
        public string Feature { get; private set; }
        public DateTime Date { get; private set; }
        public int VisitCount { get; private set; }

        public ActivityTrackerEntity() { }

        public ActivityTrackerEntity(int id, string feature, DateTime date, int visitCount)
        {
            Id = id;
            Feature = feature;
            Date = date;
            VisitCount = visitCount;
        }

        public static ActivityTrackerEntity Create(int id, string feature, DateTime date, int visitCount)
        {
            return new ActivityTrackerEntity(id, feature, date, visitCount);
        }

        public void Update(string feature, DateTime date, int visitCount)
        {
            Feature = feature;
            Date = date;
            VisitCount = visitCount;
        }

        public void UpdateFeature(string feature)
        {
            Feature = feature;
        }

        public void UpdateDate(DateTime date)
        {
            Date = date;
        }

        public void UpdateVisitCount(int visitCount)
        {
            VisitCount = visitCount;
        }
    }
}
