using Calorie.Models;
using System.Threading.Tasks;
using Calorie.Models.Trackers;

namespace Calorie.BusinessLogic.Trackers
{
    public interface ITracker
    {
        
        string name { get; }

        Tracker.TrackerType Type { get; }

        string AuthenticateActionName { get; }

        //string AuthenticateStart();              
        //Task<bool> AuthenticateComplete(string userID, string code,ApplicationDbContext db);

        Task<string> GetAccessCode(ApplicationDbContext db,Tracker t);
        
        string DisassociateStart();

        string DisassociateComplete();

        string DisassociateActionName { get; }

        string LogoURL { get; }

        string ManageURL { get; }



    }
}
