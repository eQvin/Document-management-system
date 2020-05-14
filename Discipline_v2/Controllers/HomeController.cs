using Discipline_v2.Models;
using Discipline_v2.Models.Autorize;
using Discipline_v2.Models.ChatModels;
using Discipline_v2.Models.Main;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Discipline_v2.Controllers
{
    public class HomeController : Controller
    {

        #region Main Index
        public ActionResult Index()
        {
            using (DisDBContext db = new DisDBContext())
            {
                #region Current User
                var newUser = User as CustomPrincipal;
                if (newUser != null)
                {
                    ViewBag.Email = newUser.UserEmail;
                }
                #endregion
            }

            return View();
        }
        #endregion


        #region User Logout
        public ActionResult LogOut()
        {
            var CurrentUser = User as CustomPrincipal;
            if (CurrentUser != null)
            {
                //DeleteMainXMLObject(CurrentUser.UserLogin);
                FormsAuthentication.SignOut();
            }
            return RedirectToAction("Index", "Home", null);
        }
        #endregion


        #region Login and Registration
        [HttpPost]
        public ActionResult login(string email, string password)
        {
            if (email != null && password != null)
            {
                // поиск пользователя в бд
                user user = null;
                using (DisDBContext db = new DisDBContext())
                {
                    user = db.Users.FirstOrDefault(u => u.email.ToLower().Contains(email.ToLower().Trim()) && u.password.Trim() == password.Trim());
                }
                if (user != null)
                {
                    if (user.email_check != 1)
                    {
                        #region sendEmail 
                        string htmlCodeFull;
                        string code = user.email;
                        var callbackUrl = Url.Action("ConfirmEmail", "Home", new { userId = user.id, code = code }, protocol: Request.Url.Scheme);

                        StreamReader readerhtmlCodeHead = new StreamReader(Server.MapPath("~/Views/Mailing/emailConfirmHead.html"));
                        string htmlCodeHead = readerhtmlCodeHead.ReadToEnd();

                        StreamReader readerhtmlCodeFooter = new StreamReader(Server.MapPath("~/Views/Mailing/emailConfirmFooter.html"));
                        string htmlCodeFooter = readerhtmlCodeFooter.ReadToEnd();


                        string htmlCodeMiddle = "<table id=\"rec85502797\" style=\"width:100%; border-collapse:collapse; border-spacing:0; margin:0; border:0;\" cellpadding=\"0\" cellspacing=\"0\" data-record-type=\"618\"><tr><td style=\"padding-left:15px; padding-right:15px; \"><table id=\"recin85502797\" class=\"r\" style=\"margin: 0 auto;background-color:#ffffff;border-spacing: 0;width:600px;\" align=\"center\"><tr><td style=\"padding-top:30px;padding-bottom:30px;padding-left:30px;padding-right:30px;\"><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"width:100%;\"><tr><td><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"margin: 0 auto;\"><tr><td><a style=\"display: table-cell; text-decoration: none; font-size: 15px; text-align: center; font-weight: bold; font-family:Helvetica Neue, Helvetica, Arial, sans-serif; width: 100%;color:#ffffff; border:0px solid ;\" href=\" " + callbackUrl + " \"><img src=\"https://i.imgur.com/5p3P6ft.png\" alt=\"Confirm\" width=\"200px\" height=\"70px\"></a></td></tr></table></td></tr></table></td></tr></table></td></tr></table>";
                        htmlCodeFull = htmlCodeHead + htmlCodeMiddle + htmlCodeFooter;

                        Method m = new Method();
                        m.sendEmail(user.email.Trim(), "Confirm registration", htmlCodeFull);
                        #endregion
                        ViewBag.Message = "Confirm your email. We send a new message";
                    }

                    CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
                    serializeModel.UserId = user.id;
                    serializeModel.UserEmail = user.email.Trim();
                    serializeModel.position = user.position;

                    string userData = JsonConvert.SerializeObject(serializeModel);
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                    1,
                    "COOKIE__AUTH_",
                    DateTime.Now,
                    DateTime.Now.AddMinutes(15),
                    false,
                    userData);

                    string encTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    Response.Cookies.Add(faCookie);
                    return Json("success", JsonRequestBehavior.AllowGet);


                }
                else
                {
                    return Json("errorEmail", JsonRequestBehavior.AllowGet);
                }
            }
            return Json("errorEmail", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Registration(user model)
        {
            if (ModelState.IsValid)
            {
                user _user = new user();
                using (DisDBContext db = new DisDBContext())
                {
                    _user = db.Users.FirstOrDefault(u => u.email.ToLower().Trim().Contains(model.email.ToLower().Trim())); //проверяем есть ли п ользователь с таким email

                    if (_user == null)
                    {
                        company userCompany = new company();
                        department userDepartment = new department();
                        string companName = model.last_name;

                        userCompany = db.Companies.Where(u => u.company_name.Trim() == companName.Trim()).FirstOrDefault();
                        if (userCompany == null)
                        {
                            _user = new user();
                            userCompany = new company();
                            userCompany.company_name = companName;

                            // Copmany
                            db.Companies.Add(userCompany);
                            db.SaveChanges();
                            //userCompany = db.Companies.Where(u => u.company_name.Trim() == companName.Trim()).FirstOrDefault();

                            // Department
                            userDepartment.dpt_name = "Directors";
                            userDepartment.dpt_description = "Executives members";
                            userDepartment.company_id = userCompany.id;
                            db.Departments.Add(userDepartment);
                            db.SaveChanges();

                            // User
                            _user.first_name = model.first_name;
                            _user.email = model.email;
                            _user.password = model.password;
                            HashMD5 Hash =new HashMD5();
                            _user.security_stamp = Hash.HashMD5String(DateTime.Now.ToString());
                            _user.company_id = userCompany.id;
                            _user.department_id = userDepartment.id;
                            db.Users.Add(_user);
                            db.SaveChanges();

                            _user = db.Users.Where(u => u.email.Trim() == model.email && u.password.Trim() == model.password).FirstOrDefault();


                            if (_user != null)
                            {
                                #region sendEmail 
                                string htmlCodeFull;
                                string code = _user.email;
                                var callbackUrl = Url.Action("ConfirmEmail", "Home", new { userId = _user.id, code = code }, protocol: Request.Url.Scheme);

                                StreamReader readerhtmlCodeHead = new StreamReader(Server.MapPath("~/Views/Mailing/emailConfirmHead.html"));
                                string htmlCodeHead = readerhtmlCodeHead.ReadToEnd();

                                StreamReader readerhtmlCodeFooter = new StreamReader(Server.MapPath("~/Views/Mailing/emailConfirmFooter.html"));
                                string htmlCodeFooter = readerhtmlCodeFooter.ReadToEnd();


                                string htmlCodeMiddle = "<table id=\"rec85502797\" style=\"width:100%; border-collapse:collapse; border-spacing:0; margin:0; border:0;\" cellpadding=\"0\" cellspacing=\"0\" data-record-type=\"618\"><tr><td style=\"padding-left:15px; padding-right:15px; \"><table id=\"recin85502797\" class=\"r\" style=\"margin: 0 auto;background-color:#ffffff;border-spacing: 0;width:600px;\" align=\"center\"><tr><td style=\"padding-top:30px;padding-bottom:30px;padding-left:30px;padding-right:30px;\"><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"width:100%;\"><tr><td><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"margin: 0 auto;\"><tr><td><a style=\"display: table-cell; text-decoration: none; font-size: 15px; text-align: center; font-weight: bold; font-family:Helvetica Neue, Helvetica, Arial, sans-serif; width: 100%;color:#ffffff; border:0px solid ;\" href=\" " + callbackUrl + " \"><img src=\"https://i.imgur.com/5p3P6ft.png\" alt=\"Confirm\" width=\"200px\" height=\"70px\"></a></td></tr></table></td></tr></table></td></tr></table></td></tr></table>";
                                htmlCodeFull = htmlCodeHead + htmlCodeMiddle + htmlCodeFooter;

                                Method m = new Method();
                                m.sendEmail(_user.email.Trim(), "Confirm registration", htmlCodeFull);
                                #endregion
                                return Json("success", JsonRequestBehavior.AllowGet); // return RedirectToAction("ConfirmWaitingPage", "Home");
                            }
                            else { return Json("error", JsonRequestBehavior.AllowGet); }
                        }
                        else
                        {
                            return Json("failedCompany", JsonRequestBehavior.AllowGet); //Пользователь с таким email уже зарегистрирован 
                        }
                    }
                    else
                    {
                        return Json("failedEmail", JsonRequestBehavior.AllowGet); //Пользователь с таким email уже зарегистрирован!
                    }
                }
            }
            return Json("error", JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Confirm Email Company 
        public ActionResult ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            using (DisDBContext db = new DisDBContext())
            {
                int user_Id = Convert.ToInt32(userId);
                user _users = db.Users.FirstOrDefault(m => m.id == user_Id && m.email.StartsWith(code));
                if (_users != null)
                {
                    _users.email_check = 1;
                    db.SaveChanges();

                    CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
                    serializeModel.UserId = _users.id; //1;
                    serializeModel.UserEmail = _users.email.Trim();

                    string userData = JsonConvert.SerializeObject(serializeModel);
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                    1,
                    "COOKIE__AUTH_",
                    DateTime.Now,
                    DateTime.Now.AddMinutes(15),
                    false,
                    userData);

                    string encTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    Response.Cookies.Add(faCookie);
                    if (_users.check != 1)
                    {
                        return RedirectToAction("Upload", "Home", new { index = _users.id });
                    }
                }
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult ConfirmEmailUser()
        {

            return View();
        }

        [HttpGet]
        public ActionResult ConfirmCompanyUser()
        {

            return View();
        }
        #endregion


        #region Upload Company Filds
        [HttpGet]
        public ActionResult Upload(int? index)
        {
            if (index != null)
            {
                using (DisDBContext db = new DisDBContext())
                {
                    #region Current User
                    var newUser = User as CustomPrincipal;
                    if (newUser != null)
                    {
                        ViewBag.Email = newUser.UserEmail;
                    }
                    #endregion
                    user _users = db.Users.Where(m => m.id == index).FirstOrDefault();
                    if (_users.position == 1 || _users.position == 0)  // CEO and Admin
                    {
                        company userCompany = new company();
                        userCompany = db.Companies.Where(u => u.id == _users.company_id).FirstOrDefault();
                        ViewBag.CompanyName = userCompany.company_name;
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home", null);
                    }
                }
            }
            else return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Upload(company newCompany)
        {
            var curentUser = User as CustomPrincipal;   //Идентификатор Портнера в базе данных
            if (curentUser != null)
            {
                using (DisDBContext db = new DisDBContext())
                {
                    //Нахидим из Базы пользователя
                    user User = db.Users.FirstOrDefault(u => u.id == curentUser.UserId);

                    if (User.position == 1 || User.position == 0)
                    {
                        company userCompany = new company();
                        userCompany = db.Companies.Where(u => u.id == User.company_id).FirstOrDefault();

                        userCompany.company_country = newCompany.company_country;
                        userCompany.ceo_name = newCompany.ceo_name;
                        userCompany.bank_detail = newCompany.bank_detail;
                        userCompany.address = newCompany.address;
                        userCompany.post_index = newCompany.post_index;
                        userCompany.site = newCompany.site;
                        userCompany.tell = newCompany.tell;
                        db.SaveChanges();

                        #region Comment
                        //foreach (var file in uploadFile)
                        //{
                        //    if (file != null)
                        //    {
                        //        #region Готовим директорию на сервере
                        //        var path = Server.MapPath("~/Files/" + newUser.id.ToString().Trim() + "/docs");
                        //        Directory.CreateDirectory(path);
                        //        string fileName = Path.GetFileName(file.FileName);  //fileName = fileName.Replace(fileName.Substring(0, fileName.LastIndexOf('.')), "Ava");
                        //        #endregion

                        //        #region Сохраняем файл на сервере
                        //        file.SaveAs(Server.MapPath("~/Files/" + newUser.id.ToString() + "/docs/" + fileName));

                        //        user_doc doc = new user_doc();
                        //        doc.owner_id = newUser.id;
                        //        doc.doc_name = "../../Files/" + newUser.id.ToString().Trim() + "/docs/" + fileName;
                        //        db.User_docs.Add(doc);
                        //        db.SaveChanges();
                        //        #endregion
                        //    }
                        //}
                        #endregion

                        return RedirectToAction("ConfirmCompanyUser", "Home");
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Index", "Home");
        }
        #endregion


        #region Error pages
        [HttpGet]
        public ActionResult AccessDenied()
        {

            return View();
        }
        #endregion

    }
}