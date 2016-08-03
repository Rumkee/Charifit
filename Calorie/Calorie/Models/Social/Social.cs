using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calorie.Models.Social
{
    public class SocialVM
    {
        public enum SocialType
        {
            OffSet,
            Pledge,
            Team,
            Corporate,
            Charity,
            Site,
            PledgeContribution
        }

        public string Blurb { get; set; }
        public string ShareURL { get; set; }
        public SocialType Type { get; set; }

        public string LinkID { get; set; }


    }
}