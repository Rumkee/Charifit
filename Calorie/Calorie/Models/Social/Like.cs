using System;
using System.Collections.Generic;

namespace Calorie.Models.Social
{

   

    public class Like
    {

        public int ID { get; set; }
        public string LinkType { get; set; }
        public string LinkID { get; set; }

        public string UserID { get; set; }
                
    }

    //public class LikeVM{

      


      
    //    public int LinkID { get; set; }
    //    public List<Like> Likes { get; set; }
        
    //}
}