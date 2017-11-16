using System.ComponentModel.DataAnnotations;

namespace MachineLearing.Models
{
    public class UserQuery
    {

        [Required(ErrorMessage = "ThirdParty is required")]
        [Display(Name = "ThirdParty :")]
        public ThirdParty ThirdParty { get; set; }

        [Required(ErrorMessage = "Integration is required")]
        [Display(Name = "Integration :")]
        public Type Integration { get; set; }

        [Required(ErrorMessage = "Query is required")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Query:")]
        public string Query { get; set; }

        [Display(Name = "Possible Solution:")]
        public string PossibleSolution { get; set; }

        public double Score { get; set; }

        public int Id { get; set; }
    }
    public enum ThirdParty
    {
        Gallagher,
        Jacques,
        CCure
    }
    public enum Type
    {
        Video,
        Alarm
    }
}