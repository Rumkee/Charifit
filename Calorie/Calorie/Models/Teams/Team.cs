using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Calorie.Models.Teams
{
    public class Team
    {

        public enum Access
        {
            @Public,
            @Private,
        }

        public int ID{ get; set; }

        public string Name { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [ForeignKey("ImageID")]
        public virtual CalorieImage Image { get; set; }
        public int ImageID { get; set; }

        public virtual List<ApplicationUser> Members { get; set; }

        public Access Availability { get; set; }

        public virtual ICollection<Pledges.Pledge> Pledges { get; set; }

        public virtual ICollection<TeamJoinRequests> JoinRequests { get; set; }

    }

    public class EditTeamVM
    {
        public int TeamID { get; set; }
        public Team Team { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public string Name { get; set; }
        public int TeamImageID { get; set; }
        public Team.Access Availability { get; set; }
    }
    public class TeamIndexVM
    {
        
        public List<Team> AllTeams { get; set; }

    }

    public class TeamVM
    {
        public TeamVM()
        {
            ShowPanel = true;
        }

        public Team Team { get; set; }
        public bool ShowSelector { get; set; }
        public bool? ShowHyperLinks { get; set; }

        public bool ShowSocial { get; set; }
        public bool ShowPanel { get; set; }
    }


    public class TeamDetailsVM
    {
        public Team Team { get; set; }
        public bool IsAdmin { get; set; }
        public ApplicationUser AdminUser { get; set; }

        public List<Pledges.Pledge> CurrentSponsorships{ get; set; }
        public List<Pledges.Pledge> ArchivedSponsorships { get; set; }

    }

    public class TeamSearchResultsVM
    {
        public List<Team> TeamResults{ get; set; }

        public bool ShowSelector { get; set; }
    }

}