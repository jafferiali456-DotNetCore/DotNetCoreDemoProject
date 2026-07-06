using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoProjectService.Entity
{
    public class GoalObjective
    {
        public int GoalObjectiveID { get; set; }
        public int GoalSettingID { get; set; }
        
        public string? GoalType { get; set; }

        
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public decimal? Weightage { get; set; }

        public int MaxGoals { get; set; }

        public int MinGoals { get; set; }

        public int NoOfGoals { get; set; }
        public decimal? CompletionPercentage { get; set; }

        public int Id { get; set; }
        public int rec_status { get; set; }
        public DateTime rec_modified_on { get; set; }
        public DateTime DurationStart { get; set; }
        public DateTime DurationEnd { get; set; }
        public int rec_modified_by { get; set; }
        public DateTime rec_time_stamp { get; set; }
        public string? rec_rnd_key { get; set; }
        public int rec_add_by { get; set; }
        public string? Mode { get; set; }
        public List<KPI> kPIs { get; set; }

    }
}