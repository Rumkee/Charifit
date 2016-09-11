using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Calorie.Models;
using Facebook;
using System.Net;
using System.Net.Mime;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Text;
using Recaptcha.Web;
using Recaptcha.Web.Mvc;

namespace Calorie.Controllers
{


    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();

        public ApplicationUser CurrentUser()
        {
            var id = User.Identity.GetUserId();
            return db.Users.FirstOrDefault(u => u.Id == id);
        }

        public AccountController()
        {
        }

        [AllowAnonymous]
        public ActionResult LoggedInUser()
        {

            var user = CurrentUser();

            if (user == null)
            {
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Content("<div id='loggedInUserData' data-userloggedin = 'false'></div>");
            }

            Response.StatusCode = (int)HttpStatusCode.OK;

            var ResponseStr =new System.Text.StringBuilder();

            ResponseStr.Append(" data-userloggedin='true'");
            ResponseStr.Append($" data-username='{user.UserName}'");
            ResponseStr.Append($" data-userid='{user.Id}'");
            ResponseStr.Append($" data-usercurrency='{user.Currency}'");
            ResponseStr.Append($" data-useremail='{user.Email}'");
            ResponseStr.Append($" data-useriscompany='{user.IsCompany}'");
            ResponseStr.Append($" data-userisexercisor='{user.IsExercisor}'");
            ResponseStr.Append($" data-userissponsor='{user.IsSponsor}'");                
            ResponseStr.Append($" data-userteamid='{user.TeamID}'");
            ResponseStr.Append($" data-userprofilepictureid='{user.ProfilePictureID}'");

            return Content($"<div id='loggedInUserData' {ResponseStr} ></div>");
        }


        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }



        [AllowAnonymous]
        public ActionResult GetPopupInfo(string UserID)
        {

          
            var user = db.Users.FirstOrDefault(u => u.Id == UserID);

            if (user == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest ;
                return Json("unknownUser", JsonRequestBehavior.AllowGet);
            }

            var thisUserStats = BusinessLogic.UserLogic.getStatsForUser(user);

            var record = new
            {
                name = user.UserName,
                status = BusinessLogic.UserLogic.getStarsHTMLForUser(user),
                divUniqueID = Guid.NewGuid(),
                calTotal = thisUserStats.calTotal ?? 0,
                hoursTotal = thisUserStats.HoursTotal ?? 0,
                milesTotal = thisUserStats.MilesTotal ?? 0 ,
                sessionsTotal = thisUserStats.SessionsTotal ?? 0,
                pledgedTotal = thisUserStats.PledgedTotal,

                calStars = BusinessLogic.UserLogic.GetStarsHTMLForActivity(Models.Pledges.PledgeActivity.ActivityUnits.Calories, thisUserStats.calTotal ?? 0m),
                hoursStars = BusinessLogic.UserLogic.GetStarsHTMLForActivity(Models.Pledges.PledgeActivity.ActivityUnits.Hours , thisUserStats.HoursTotal.Value),
                milesStars = BusinessLogic.UserLogic.GetStarsHTMLForActivity(Models.Pledges.PledgeActivity.ActivityUnits.Miles, thisUserStats.MilesTotal.Value),
                sessionsStars = BusinessLogic.UserLogic.GetStarsHTMLForActivity(Models.Pledges.PledgeActivity.ActivityUnits.Sessions, thisUserStats.SessionsTotal ?? 0m),
                pledgedStars = BusinessLogic.UserLogic.GetStarsHTMLForPledgedTotal(thisUserStats.PledgedTotal),

                teamImageID = user.Team?.ImageID,
                teamName = user.Team?.Name,
               // badges = Badges
            };
                        

            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(record, JsonRequestBehavior.AllowGet);

        }


        [AllowAnonymous]
        public ActionResult CheckUsername(string Username)
        {
            
            var Count = db.Users.Count(u => u.UserName == Username);
            var responsetext = "no";
            if (Count == 0 && (!string.IsNullOrWhiteSpace(Username)))
            {
                responsetext = "yes";
            }
            
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Content(responsetext , MediaTypeNames.Text.Plain);                                  
            
        }

        public class GenericJsonProxyData
        {
            public string URL { get; set; }
            public Dictionary<String,String> Headers { get; set; }
            public Dictionary<String, String> Params { get; set; }

        }

     
        [HttpPost]        
        public ActionResult KillAllAlerts()
        {
            var user = CurrentUser();
            if (user == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("user not found", MediaTypeNames.Text.Plain);
            }
            else
            {
                user.Alerts.RemoveAll(a => true);
                db.SaveChanges();
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Content("Done", MediaTypeNames.Text.Plain);
            }
            
        }

        [HttpPost]
        public ActionResult KillAlert(string ID)
        {

            var IDInt = BusinessLogic.GenericLogic.GetInt(ID);
            var user = CurrentUser();
            if (user == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("user not found", MediaTypeNames.Text.Plain);
            }

            var AlertToDelete = user.Alerts.FirstOrDefault(a => a.ID == IDInt);
            if (AlertToDelete == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("user does not have alert with that id", MediaTypeNames.Text.Plain);
            }
            user.Alerts.Remove(AlertToDelete);
            db.SaveChanges();
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Content("Done", MediaTypeNames.Text.Plain);
        }


        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            TempData.Remove("IsLogInView");
            TempData.Add("IsLogInView", true);
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LoginModal(LoginViewModel model)
        {

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Login Unsuccessful", MediaTypeNames.Text.Plain);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            

            var result = await SignInManager.PasswordSignInAsync(model.UsernameOrEmail , model.Password, model.RememberMe, shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    return Content("OK", MediaTypeNames.Text.Plain);
                    
                case SignInStatus.LockedOut:
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Content("Account Locked Out", MediaTypeNames.Text.Plain);
                case SignInStatus.RequiresVerification:
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Content("Account Requires Verification", MediaTypeNames.Text.Plain);
                case SignInStatus.Failure:
                default:
                    var user = UserManager.Users.FirstOrDefault(u => u.Email  == model.UsernameOrEmail);
                    if (user != null)
                    {
                        if (UserManager.CheckPassword(user, model.Password ))
                        {
                            SignInManager.SignIn(user, false , true);
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            return Content("OK", MediaTypeNames.Text.Plain);
                        }
                    }

                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Content("Login Unsuccessful", MediaTypeNames.Text.Plain);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {

            
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true


            var result = await SignInManager.PasswordSignInAsync(model.UsernameOrEmail, model.Password, true, shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                //case SignInStatus.RequiresVerification:
                //    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                //case SignInStatus.Failure:
                default:
                    var user = UserManager.Users.FirstOrDefault(u => u.Email == model.UsernameOrEmail);
                    if (user != null)
                    {
                        if (UserManager.CheckPassword(user, model.Password))
                        {
                            SignInManager.SignIn(user, false, true);
                            return RedirectToLocal(returnUrl);
                        }
                    }

                    ModelState.AddModelError("", "Nope, try again.");
                    return View(model);
            }
        }



        
        public ActionResult RegisterFurtherDetails() => View(new RegisterFurtherDetailsViewModel() {User = CurrentUser() });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterFurtherDetails(RegisterFurtherDetailsViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = CurrentUser();
                user.PreferredActivities.Clear();
                if (model.PreferredActivities!= null)
                {
                    foreach (var a in model.PreferredActivities)
                        user.PreferredActivities.Add(new Models.Pledges.PreferredActivity() { Activity = (Models.Pledges.PledgeActivity.ActivityTypes)Enum.Parse(typeof(Models.Pledges.PledgeActivity.ActivityTypes), a) });
                }
                
                                    
                if (user.IsSponsor)                
                    return RedirectToAction("RegisterJustGiving");

                return RedirectToAction("Index","Home");
            }

            model.User = CurrentUser();
            return View(model);
        }


        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View(new RegisterViewModel() {IsExercisor=true,IsSponsor=true, FirstPartyEmails = true , IsSuperAdmin= CurrentUser()!=null && CurrentUser().IsSuperAdmin} );
        }

        private async Task ReCapchaVerification()
        {
            RecaptchaVerificationHelper recaptchaHelper = this.GetRecaptchaVerificationHelper();
            if (string.IsNullOrEmpty(recaptchaHelper.Response))
            {
                ModelState.AddModelError("", "Click the thing that says 'I'm not a robot' (unless you are actually a robot, in which case go away)");
                return;
            }

            RecaptchaVerificationResult recaptchaResult = await recaptchaHelper.VerifyRecaptchaResponseTaskAsync();
            if (recaptchaResult != RecaptchaVerificationResult.Success)
                ModelState.AddModelError("", "Google says you're a bot. Sorry (notsorry)");
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {

            if (!model.AcceptTAndCs)
            {
                ModelState.AddModelError("AcceptTAndCs", "You Must Accept The Terms & Conditions");
                

            }

            await ReCapchaVerification();
            
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser {        UserName = model.Username ,
                                                        Email = model.Email,
                                                        ProfilePictureID = model.ProfilePictureImageID ,
                                                        RecieveThirdPartyEmails =model.ThirdPartyEmails ,
                                                        RecieveFirstPartyEmails=model.FirstPartyEmails,
                                                        IsCompany = model.IsCorporate,
                                                        IsExercisor = model.IsExercisor,
                                                        IsSponsor = model.IsSponsor
                                                        
                                                };

                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    string code = UserManager.GenerateEmailConfirmationToken(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                    var reloadedUser = db.Users.FirstOrDefault(u => u.Id == user.Id);

                    var WelcomeVM = new GenericEmailViewModel {RootURL = GetRootURL()};

                    await BusinessLogic.UserLogic.ConfigureNewUser(reloadedUser, WelcomeVM, callbackUrl);
                    db.SaveChanges();
                                 
                    if (reloadedUser.IsExercisor)
                    {
                        return RedirectToAction("RegisterFurtherDetails");
                    }

                    if (reloadedUser.IsSponsor)
                    {
                        return RedirectToAction("RegisterJustGiving");
                    }

                    return RedirectToAction("Index","Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #region Helpers

        private string GetRootURL()
        {
            return $"{Request.Url.Scheme}://{Request.Url.Authority}{Url.Content("~")}".TrimEnd("/".ToCharArray()[0]);
        }



        #endregion
        

        [Authorize]
        public ActionResult RegisterJustGiving() => View(new RegisterJustGiving() );

        //
        // POST: /Account/Register

       [Authorize]
        [HttpPost]        
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterJustGiving(RegisterJustGiving model)
        {

            //add email, password and accepttandc
            
            if (!ModelState.IsValid)
            {
                return View(model);
            }
                     

            var body = new
            {
                acceptTermsAndConditions = true,
                address = new
                {
                    country = model.country,
                    line1 = model.AddressLine1 ,
                    line2 = model.AddressLine2 ,
                    postcodeOrZipcode= model.postcodeOrZip,
                    townOrCity = model.AddressTownOrCity,            
                },
                email= CurrentUser()?.Email,
                firstName= model.firstname,
                lastName=model.lastname,
                password= Guid.NewGuid().ToString("d").Replace("-", "").Substring(1, 15),
                //reference="Your Reference",
                title= model.title
            };
                    
           var result = await BusinessLogic.GenericLogic.HttpPut(body, ConfigurationManager.AppSettings["JustGivingAPIURL"] + ConfigurationManager.AppSettings["JustGivingAppId"] + "/v1/account");
                        
           var resultStr = await result.Content.ReadAsStringAsync();

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var JustGivingSignUpEmailVM = new GenericEmailViewModel {RootURL = GetRootURL()};

                await BusinessLogic.UserLogic.ConfigureNewJustGivingAccount(CurrentUser(), JustGivingSignUpEmailVM, body.password);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
           if (result.StatusCode == HttpStatusCode.BadRequest)
           {
               dynamic jsonresponse = System.Web.Helpers.Json.Decode(resultStr);
               foreach (var item in jsonresponse)
               {

                   string value = item.Value;
                   string desc = string.Empty;

                   try
                   {
                       desc = item.desc;
                   }
                   catch (Exception)
                   {
                       // ignored
                   }

                   if (value == null)
                   {
                       ModelState.AddModelError("Registration Message", "JustGiving says.. " + desc);
                   }
                   else
                   {
                       ModelState.AddModelError("Registration Message", "JustGiving says.. " + value);
                   }

               }

           }
           else if (result.StatusCode == HttpStatusCode.InternalServerError)
           {
               ModelState.AddModelError("Registration Message", "JustGiving spazzed out. Try again later");
           }

           return View(model);

        }







        public ActionResult Edit()
        {
            var user = CurrentUser();
            return View(new EditViewModel() {
                User = user,
                PreferredActivities = user.PreferredActivities?.Select(pa => pa.Activity.ToString()).ToList(),
                PreferredjustGivingCharities = user.PreferredCharities?.Select(pc => pc.Charity.JustGivingCharityID).ToList(),
                TopTeams = db.Teams.OrderByDescending(t => t.Members.Count()).Take(5).ToList()                
                }
            );
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditViewModel model)
        {
            var user = CurrentUser();
            if (ModelState.IsValid)
            {
                
                if (BusinessLogic.UserLogic.MergeVewModelToUser(model,user,db))
                {
                    BusinessLogic.Messaging.Add(Message.LevelEnum.alert_success, "User settings updated.",Message.TypeEnum.StickyAlert,user);
                    db.SaveChanges();
                }
                
            }
            db = new ApplicationDbContext();
            user = CurrentUser();
            return View(new EditViewModel()
            {
                User = user,
                PreferredActivities = user.PreferredActivities?.Select(pa => pa.Activity.ToString()).ToList(),
                PreferredjustGivingCharities = user.PreferredCharities?.Select(pc => pc.Charity.JustGivingCharityID).ToList(),
                TopTeams = db.Teams.OrderByDescending(t => t.Members.Count()).Take(5).ToList()
            }
            );
        }
        

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            var User = db.Users.FirstOrDefault(u => u.Id == userId);

            if (User == null || code == null)
            {
                return View("Error");
            }                 

            var result = await UserManager.ConfirmEmailAsync(userId, code);
            
            if (CurrentUser() != null)
            {
                if (!result.Succeeded)
                {
                    BusinessLogic.Messaging.Add(Message.LevelEnum.alert_danger, "There was a problem verifying your email address.", Message.TypeEnum.TemporaryAlert, User);
                }
                else
                {
                    BusinessLogic.Messaging.Add(Message.LevelEnum.alert_success, "Thank you for verifying your email address.", Message.TypeEnum.TemporaryAlert, User);
                }
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("ConfirmEmail");
            }
            
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword() => View();

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {

           await ReCapchaVerification();

            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                var ResetPasswordVM = new GenericEmailViewModel {RootURL = GetRootURL()};

                await BusinessLogic.UserLogic.ResetPassword( ResetPasswordVM ,user, callbackUrl);
                db.SaveChanges();                
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation() => View();

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code) => code == null ? View("Error") : View();

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {

           await ReCapchaVerification();

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation() => View();

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

       
        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
               
                //case SignInStatus.Failure:
                default:

                    // If the user does not have an account, then create one. 
                    if (loginInfo.Login.LoginProvider == "LinkedIn")
                    {
                        return await LinkedInNewUser(loginInfo);

                    }
                    if (loginInfo.Login.LoginProvider == "Google")
                    {
                        return await GoogleLoginNewUser(loginInfo);
                        
                    }
                    if (loginInfo.ExternalIdentity.Name == "Facebook")
                    {
                        //make a call to get users email address
                        var identity = AuthenticationManager.GetExternalIdentity(DefaultAuthenticationTypes.ExternalCookie);
                        var facebookAccessToken = identity.FindFirstValue("FacebookAccessToken");
                        var fb = new FacebookClient(facebookAccessToken);                
                        dynamic facebookInfo = fb.Get("/me?fields=email,birthday,gender");
                        loginInfo.Email = facebookInfo.email;

                        //if we still dont have email address then ask user for one.
                        if (string.IsNullOrEmpty(loginInfo.Email))
                        {
                            ViewBag.ReturnUrl = returnUrl;
                            ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                            return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
                        }
                        return await FacebookLoginNewUser(loginInfo);
                    }

                    //if we get this far something has gone wrong.
                    return RedirectToAction("Login");

            }
            
        }

        private async Task<ActionResult> LinkedInNewUser(ExternalLoginInfo loginInfo)
        {
            // create user object
            var user = new ApplicationUser
            {
                UserName = loginInfo.ExternalIdentity.Name.Substring(0, Math.Min(10, loginInfo.ExternalIdentity.Name.Length)),
                Email = loginInfo.Email,
                IsExercisor = true,
                IsSponsor = true
            };
            

            //persist user
            var NewUserResult = await UserManager.CreateAsync(user);

            if (NewUserResult.Succeeded)
            {
                var AddLogInResult = await UserManager.AddLoginAsync(user.Id, loginInfo.Login);
                if (AddLogInResult.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    var reloadeduser = db.Users.FirstOrDefault(u => u.Id == user.Id);

                    string code = UserManager.GenerateEmailConfirmationToken(reloadeduser.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = reloadeduser.Id, code = code }, protocol: Request.Url.Scheme);

                    var WelcomeVM = new GenericEmailViewModel { RootURL = GetRootURL() };

                    await BusinessLogic.UserLogic.ConfigureNewUser(reloadeduser, WelcomeVM, callbackUrl);
                    db.SaveChanges();

                    return RedirectToAction("RegisterFurtherDetails");
                }
            }

            //if we're here then something went wrong
            var Response = new System.Text.StringBuilder();
            Response.Append("Tried to create a new user but encountered the following issues:-" + Environment.NewLine);
            foreach (var e in NewUserResult.Errors)
                Response.Append(e);

            ViewBag.Message = Response.ToString();
            return View("ExternalLoginFailure");
        }


        private async Task<ActionResult> GoogleLoginNewUser(ExternalLoginInfo loginInfo)
        {
            // create user object
            var user = new ApplicationUser
            {
                UserName =loginInfo.ExternalIdentity.Name.Substring(0, Math.Min(10, loginInfo.ExternalIdentity.Name.Length)),
                Email = loginInfo.Email,
                IsExercisor = true,
                IsSponsor = true
            };



            //persist user
            var NewUserResult = await UserManager.CreateAsync(user);

            if (NewUserResult.Succeeded)
            {
                var AddLogInResult = await UserManager.AddLoginAsync(user.Id, loginInfo.Login);
                if (AddLogInResult.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    var reloadeduser = db.Users.FirstOrDefault(u => u.Id == user.Id);

                    string code = UserManager.GenerateEmailConfirmationToken(reloadeduser?.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = reloadeduser?.Id, code = code }, protocol: Request.Url.Scheme);

                    var WelcomeVM = new GenericEmailViewModel { RootURL = GetRootURL() };

                    await BusinessLogic.UserLogic.ConfigureNewUser(reloadeduser, WelcomeVM,callbackUrl);
                    db.SaveChanges();

                    return RedirectToAction("RegisterFurtherDetails");
                }
            }

            //if we're here then something went wrong
            var Comment = new System.Text.StringBuilder();
            Comment.Append("Tried to create a new user but encountered the following issues:-" + Environment.NewLine);
            foreach (var e in NewUserResult.Errors)
                Comment.Append(e);

            ViewBag.Message = Response.ToString();
            return View("ExternalLoginFailure");
        }

        private async Task<ActionResult> FacebookLoginNewUser(ExternalLoginInfo loginInfo)
        {
            // create user object
            var user = new ApplicationUser { UserName = loginInfo.ExternalIdentity.Name.Substring(0, Math.Min(10, loginInfo.ExternalIdentity.Name.Length)), Email = loginInfo.Email };

            //grab profile pic and add to db
            user.ProfilePictureID = BusinessLogic.ImageLogic.GetAndSaveImageFromURL($"https://graph.facebook.com/{loginInfo.Login.ProviderKey}/picture",
                                                                        CalorieImage.ImageType.UserImage);

            user.IsExercisor = true;
            user.IsSponsor = true;

            //persist user
            var NewUserResult = await UserManager.CreateAsync(user);

            if (NewUserResult.Succeeded)
            {
                var AddLogInResult = await UserManager.AddLoginAsync(user.Id, loginInfo.Login);
                if (AddLogInResult.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    var reloadeduser = db.Users.FirstOrDefault(u => u.Id == user.Id);

                    string code = UserManager.GenerateEmailConfirmationToken(reloadeduser.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = reloadeduser.Id, code = code }, protocol: Request.Url.Scheme);

                    var WelcomeVM = new GenericEmailViewModel { RootURL = GetRootURL() };

                    await BusinessLogic.UserLogic.ConfigureNewUser(reloadeduser, WelcomeVM,callbackUrl);
                    db.SaveChanges();

                    return RedirectToAction("RegisterFurtherDetails");
                }
            }

            //if we're here then something went wrong
            var Comment = new System.Text.StringBuilder();
            Comment.Append("Tried to create a new user but encountered the following issues:-" + Environment.NewLine);
            foreach (var e in NewUserResult.Errors)
                Comment.Append(e);

            ViewBag.Message = Comment.ToString();
            return View("ExternalLoginFailure");
        }


        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }

                return await FacebookLoginNewUser(info);

              
            }

            //if we're here then something went wrong            
            ViewBag.Message = "Something went wrong trying to create a user. Maybe sign up manually?";
            return View("ExternalLoginFailure");
        }

        [AllowAnonymous]
        public string GetCurrentUserProfilePictureID()
        {

            ApplicationDbContext db = new ApplicationDbContext();
            var currentUser = db.Users.Find(User.Identity.GetUserId());
                        
            if (currentUser!=null)
            {
                return currentUser.ProfilePictureID.ToString();
            }
            return "0";
        }


        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure() => View();


        public ActionResult SuperAdmin()
        {
            
            if (IsSuperAdmin())
                return View();

            throw new HttpException(403, "Forbidden");

        }

        public ActionResult RecacheJustGivingCharities()
        {

            if (IsSuperAdmin())
            {
                BusinessLogic.JustGivingLogic.RecacheJustGivingCharities();
                BusinessLogic.Messaging.Add(Message.LevelEnum.alert_success, "JustGiving Charities Data Re-cached", Message.TypeEnum.TemporaryAlert, CurrentUser());
                db.SaveChanges();
                return View("SuperAdmin");
            }
            
                throw new HttpException(403, "Forbidden");
            
        }


        private bool IsSuperAdmin()
        {
            var usr = CurrentUser();
            return (usr != null && usr.IsSuperAdmin);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }

                db.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}