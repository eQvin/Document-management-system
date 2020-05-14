using Discipline_v2.Models;
using Discipline_v2.Models.Autorize;
using Discipline_v2.Models.ChatModels;
using Discipline_v2.Models.Main;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Policy;
using Z.EntityFramework.Plus;

namespace Discipline_v2.Controllers
{
    public class PersonalController : Controller
    {


        #region Index Redirect Users
        public ActionResult Index()
        {
            var newUser = User as CustomPrincipal;
            if (newUser != null)
            {
                using (DisDBContext db = new DisDBContext())
                {
                    user _users = db.Users.FirstOrDefault(m => m.id == newUser.UserId && m.email == newUser.UserEmail);
                    if (_users.position == 0)
                    {
                        return RedirectToAction("AdminPage", "Personal");
                    }
                    else if (_users.position == 1)
                    {
                        if (_users.check != 1)
                        {
                            return RedirectToAction("Upload", "Home", new { index = _users.id });
                        }
                        return RedirectToAction("CeoPage", "Personal");
                    }
                    else
                    {
                        if (_users.check != 1)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        return RedirectToAction("UserPage", "Personal");
                    }
                }
            }
            return RedirectToAction("AccessDenied", "Home");
        }
        #endregion


        #region  AdminPage
        public ActionResult AdminPage()
        {
            using (DisDBContext db = new DisDBContext())
            {
                BaseModel model = new BaseModel();

                #region Current User
                var newUser = User as CustomPrincipal;

                if (newUser != null)
                {
                    user _userAdmin = db.Users.FirstOrDefault(m => m.id == newUser.UserId && m.email == newUser.UserEmail);
                    if (_userAdmin.position == 0)
                    {
                        #endregion
                        #region User Layout
                        ViewBag.Email = _userAdmin.email;
                        #endregion

                        #region Chat

                        #region Chat models
                        List<user> user_list = new List<user>();
                        List<ChatModel> conversations = new List<ChatModel>();
                        ChatModel cm = new ChatModel();
                        #endregion

                        user_list = db.Users.Where(p => p.email != _userAdmin.email).ToList();
                        List<conversation> conver = db.Conversations.Where(p => p.user1 == newUser.UserEmail || p.user2 == newUser.UserEmail).ToList();
                        foreach (var c in conver)
                        {
                            string other = (c.user1 == newUser.UserEmail) ? c.user2 : c.user1;
                            var user = db.Users.First(p => p.email == other);
                            user_list.Remove(user);

                            DateTime lastConver = db.Messages.Where(p => p.conversation_id == c.id).Max(p => p.date);
                            message msg = db.Messages.FirstOrDefault(p => p.conversation_id == c.id && p.date == lastConver);
                            cm.fname = user.first_name;
                            cm.lname = user.last_name;
                            cm.date = msg.date.ToShortDateString();
                            cm.msg = msg.msg;
                            cm.otherUser = other;
                            conversations.Add(cm);

                        }
                        ViewBag.UserList = user_list;
                        ViewBag.Conversation = conversations;
                        #endregion

                        #region Company
                        foreach (company Companies in db.Companies.OrderByDescending(o => o.id))
                        {
                            if (Companies == null)
                            {
                                break;
                            }
                            model.Companies.Add(Companies);
                        }
                        #endregion

                        #region User Profile
                        model.User = db.Users.FirstOrDefault(m => m.id == _userAdmin.id);
                        #endregion

                        #region Documents flow

                        string userAdminId = _userAdmin.id.ToString();
                        string userAdminCompanyID = _userAdmin.company_id.ToString();
                        string userAdminDepartmentID = _userAdmin.department_id.ToString();


                        #region _userAdmin sent documents
                        foreach (document DocumentsHistory in db.Documents.Where(p => p.owner_id == userAdminId).OrderByDescending(o => o.id))
                        {
                            if (DocumentsHistory == null)
                            {
                                break;
                            }
                            model.DocumentsHistory.Add(DocumentsHistory);
                        }
                        #endregion

                        #region _userAdmin inbox documents
                        foreach (document DocumentsInbox in db.Documents.Where(p => ((p.send_id == "user_" + userAdminId || p.send_id == "company_" + userAdminCompanyID || p.send_id == "department_" + userAdminDepartmentID) && p.owner_id != userAdminId)).OrderByDescending(o => o.id))
                        {
                            if (DocumentsInbox == null)
                            {
                                break;
                            }
                            model.DocumentsInbox.Add(DocumentsInbox);
                        }
                        #endregion


                        #endregion

                        return View(model);
                    }
                }
            }

            return RedirectToAction("AccessDenied", "Home");
        }

        #endregion


        #region CeoPage
        public ActionResult CeoPage()
        {
            using (DisDBContext db = new DisDBContext())
            {
                BaseModel model = new BaseModel();

                #region Current User
                var newUser = User as CustomPrincipal;

                if (newUser != null)
                {
                    user _userAdmin = db.Users.FirstOrDefault(m => m.id == newUser.UserId && m.email == newUser.UserEmail);
                    if (_userAdmin.position == 1)
                    {
                        #endregion
                        #region User Layout
                        ViewBag.Email = _userAdmin.email;
                        #endregion

                        #region Chat

                        #region Chat models
                        List<user> user_list = new List<user>();
                        List<ChatModel> conversations = new List<ChatModel>();
                        ChatModel cm = new ChatModel();
                        #endregion

                        user_list = db.Users.Where(p => p.email != _userAdmin.email && p.company_id == _userAdmin.company_id).ToList();
                        List<conversation> conver = db.Conversations.Where(p => p.user1 == newUser.UserEmail || p.user2 == newUser.UserEmail).ToList();
                        foreach (var c in conver)
                        {
                            string other = (c.user1 == newUser.UserEmail) ? c.user2 : c.user1;
                            user user = db.Users.First(p => p.email == other);
                            user_list.Remove(user);

                            DateTime lastConver = db.Messages.Where(p => p.conversation_id == c.id).Max(p => p.date);
                            message msg = db.Messages.FirstOrDefault(p => p.conversation_id == c.id && p.date == lastConver);
                            cm.fname = user.first_name;
                            cm.lname = user.email;
                            cm.date = msg.date.ToShortDateString();
                            cm.msg = msg.msg;
                            cm.otherUser = other;
                            conversations.Add(cm);

                        }
                        ViewBag.UserList = user_list;
                        ViewBag.Conversation = conversations;
                        #endregion

                        #region Company
                        model.Company = db.Companies.FirstOrDefault(c => c.id == _userAdmin.company_id);
                        #endregion

                        #region User Profile
                        model.User = db.Users.FirstOrDefault(m => m.id == _userAdmin.id);
                        #endregion

                        #region Departments
                        foreach (department Department in db.Departments.Where(p => p.company_id == _userAdmin.company_id).OrderByDescending(o => o.id))
                        {
                            if (Department == null)
                            {
                                break;
                            }
                            model.Departments.Add(Department);
                        }
                        #endregion

                        #region Documents flow

                        string userAdminId = _userAdmin.id.ToString();
                        string userAdminCompanyID = _userAdmin.company_id.ToString();
                        string userAdminDepartmentID = _userAdmin.department_id.ToString();


                        #region _userAdmin sent documents
                        foreach (document DocumentsHistory in db.Documents.Where(p => p.owner_id == userAdminId).OrderByDescending(o => o.id))
                        {
                            if (DocumentsHistory == null)
                            {
                                break;
                            }
                            model.DocumentsHistory.Add(DocumentsHistory);
                        }
                        #endregion

                        #region _userAdmin inbox documents
                        foreach (document DocumentsInbox in db.Documents.Where(p => ((p.send_id == "user_" + userAdminId || p.send_id == "company_" + userAdminCompanyID || p.send_id == "department_" + userAdminDepartmentID || p.send_id == "companyAll" || p.send_id == "companyAll_" + userAdminCompanyID) && p.owner_id != userAdminId)).OrderByDescending(o => o.id))
                        {
                            if (DocumentsInbox == null)
                            {
                                break;
                            }
                            model.DocumentsInbox.Add(DocumentsInbox);
                        }
                        #endregion


                        #endregion

                        return View(model);
                    }
                }
            }

            return RedirectToAction("AccessDenied", "Home");
        }

        #endregion


        #region UserPage
        public ActionResult UserPage()
        {
            using (DisDBContext db = new DisDBContext())
            {
                BaseModel model = new BaseModel();

                #region Current User
                var newUser = User as CustomPrincipal;

                if (newUser != null)
                {
                    user _userAdmin = db.Users.FirstOrDefault(m => m.id == newUser.UserId && m.email == newUser.UserEmail);
                    if (_userAdmin.position == 2)
                    {
                        #endregion

                        #region User Layout
                        ViewBag.Email = _userAdmin.email;
                        #endregion

                        #region Chat

                        #region Chat models
                        List<user> user_list = new List<user>();
                        List<ChatModel> conversations = new List<ChatModel>();
                        ChatModel cm = new ChatModel();
                        #endregion

                        user_list = db.Users.Where(p => p.email != _userAdmin.email && p.company_id == _userAdmin.company_id).ToList();
                        List<conversation> conver = db.Conversations.Where(p => p.user1 == newUser.UserEmail || p.user2 == newUser.UserEmail).ToList();
                        foreach (var c in conver)
                        {
                            string other = (c.user1 == newUser.UserEmail) ? c.user2 : c.user1;
                            var user = db.Users.First(p => p.email == other);
                            user_list.Remove(user);

                            DateTime lastConver = db.Messages.Where(p => p.conversation_id == c.id).Max(p => p.date);
                            message msg = db.Messages.FirstOrDefault(p => p.conversation_id == c.id && p.date == lastConver);
                            cm.fname = user.first_name;
                            cm.lname = user.last_name;
                            cm.date = msg.date.ToShortDateString();
                            cm.msg = msg.msg;
                            cm.otherUser = other;
                            conversations.Add(cm);

                        }
                        ViewBag.UserList = user_list;
                        ViewBag.Conversation = conversations;
                        #endregion

                        #region Company
                        model.Company = db.Companies.FirstOrDefault(c => c.id == _userAdmin.company_id);
                        #endregion

                        #region User Profile
                        model.User = db.Users.FirstOrDefault(m => m.id == _userAdmin.id);
                        #endregion

                        #region Departments
                        foreach (department Department in db.Departments.Where(p => p.company_id == _userAdmin.company_id).OrderByDescending(o => o.id))
                        {
                            if (Department == null)
                            {
                                break;
                            }
                            model.Departments.Add(Department);
                        }
                        #endregion

                        #region Documents flow

                        string userAdminId = _userAdmin.id.ToString();
                        string userAdminCompanyID = _userAdmin.company_id.ToString();
                        string userAdminDepartmentID = _userAdmin.department_id.ToString();

                        document FAsfa = db.Documents.Where(p => p.owner_id == userAdminId).FirstOrDefault();

                        #region _userAdmin sent documents
                        foreach (document DocumentsHistory in db.Documents.Where(p => p.owner_id == userAdminId).OrderByDescending(o => o.id))
                        {
                            if (DocumentsHistory == null)
                            {
                                break;
                            }
                            model.DocumentsHistory.Add(DocumentsHistory);
                        }
                        #endregion

                        #region _userAdmin inbox documents
                        foreach (document DocumentsInbox in db.Documents.Where(p => ((p.send_id == "user_" + userAdminId || p.send_id == "company_" + userAdminCompanyID || p.send_id == "department_" + userAdminDepartmentID || p.send_id == "companyAll" || p.send_id == "companyAll_" + userAdminCompanyID) && p.owner_id != userAdminId)).OrderByDescending(o => o.id))
                        {
                            if (DocumentsInbox == null)
                            {
                                break;
                            }
                            model.DocumentsInbox.Add(DocumentsInbox);
                        }
                        #endregion


                        #endregion

                        return View(model);
                    }
                }
            }

            return RedirectToAction("AccessDenied", "Home");
        }
        #endregion


        #region UploadFile
        [HttpPost]
        public JsonResult UploadFile(string description, string tittle, string send_id)
        {
            using (DisDBContext db = new DisDBContext())
            {
                #region Current User
                var newUser = User as CustomPrincipal;
                if (newUser != null)
                {
                    user _userAdmin = db.Users.FirstOrDefault(m => m.id == newUser.UserId && m.email == newUser.UserEmail);
                    if (_userAdmin != null)
                    {
                        #endregion
                        foreach (string file in Request.Files)
                        {
                            var upload = Request.Files[file];
                            if (upload != null)
                            {
                                // получаем имя файла
                                string fileName = System.IO.Path.GetFileName(upload.FileName);

                                var path = Server.MapPath("~/Files/Documents");
                                Directory.CreateDirectory(path);
                                if (send_id == "company")
                                {
                                    send_id = "company_" + _userAdmin.company_id.ToString();
                                }

                                document documentFile = new document();
                                documentFile.owner_id = _userAdmin.id.ToString();
                                documentFile.send_id = send_id;
                                documentFile.description = description;
                                documentFile.tittle = tittle;
                                documentFile.status = 0;
                                documentFile.date = DateTime.Now;
                                db.Documents.Add(documentFile);
                                db.SaveChanges();

                                fileName = fileName.Replace(fileName.Substring(0, fileName.LastIndexOf('.')), documentFile.id.ToString());


                                upload.SaveAs(Server.MapPath("~/Files/Documents/" + fileName));
                                documentFile.file_name = fileName;
                                documentFile.file_path = "~/Files/Documents/" + fileName;
                                db.SaveChanges();
                                return Json("File uploaded");
                            }
                        }
                    }
                }
                return Json("Error");
            }
        }

        public JsonResult DepartmentUser(int dptId)
        {
            using (DisDBContext db = new DisDBContext())
            {
                BaseModel model = new BaseModel();
                #region Current User
                var newUser = User as CustomPrincipal;
                user _userAdmin = db.Users.FirstOrDefault(m => m.id == newUser.UserId && m.email == newUser.UserEmail);
                #endregion
                if (_userAdmin != null)
                {
                    foreach (user Users in db.Users.Where(p => (p.department_id == dptId && p.id != _userAdmin.id)).OrderByDescending(o => o.id))
                    {
                        if (Users == null)
                        {
                            break;
                        }
                        model.Users.Add(Users);
                    }
                    return Json(model);
                }
                else { return Json("error", JsonRequestBehavior.AllowGet); }
            }

        }



        #endregion


        #region DownloadFile And File Status
        public FileResult DownloadFile(string fileName, int id)
        {
            using (DisDBContext db = new DisDBContext())
            {
                DisDBContext db2 = new DisDBContext();
                #region Current User
                var newUser = User as CustomPrincipal;
                #endregion
                if (newUser != null)
                {
                    user _userAdmin = db.Users.FirstOrDefault(m => m.id == newUser.UserId && m.email == newUser.UserEmail);
                    if (_userAdmin != null)
                    {
                        if (id != 0)
                        {
                            document currentDocument = db.Documents.FirstOrDefault(m => m.id == id);
                            currentDocument.status = 1;
                            db.SaveChanges();
                        }
                        string dir = fileName.Substring(0, fileName.LastIndexOf("/") + 1);
                        string file = fileName.Substring(fileName.LastIndexOf("/") + 1);

                        var filepath = System.IO.Path.Combine(Server.MapPath(dir), file);  //System.IO.Path.Combine(Server.MapPath("/Files/"), fileName);
                        return File(filepath, MimeMapping.GetMimeMapping(dir), file);
                    }
                }
                return null;
            }
        }

        public JsonResult DocumentStatus(int id, int status)
        {
            using (DisDBContext db = new DisDBContext())
            {
                BaseModel model = new BaseModel();
                #region Current User
                var newUser = User as CustomPrincipal;
                user _userAdmin = db.Users.FirstOrDefault(m => m.id == newUser.UserId && m.email == newUser.UserEmail);
                #endregion
                if (_userAdmin != null)
                {
                    document currentDocument = db.Documents.FirstOrDefault(m => m.id == id);
                    currentDocument.status = status;
                    db.SaveChanges();
                    return Json(id, JsonRequestBehavior.AllowGet);
                }
                else { return Json("error", JsonRequestBehavior.AllowGet); }
            }

        }
        #endregion


        #region Company User Department Settings
        [HttpPost]
        public ActionResult CreateUser(user model)
        {
            using (DisDBContext db = new DisDBContext())
            {
                var newUser = User as CustomPrincipal;
                if (newUser != null)
                {
                    user _userAdmin = db.Users.FirstOrDefault(m => m.id == newUser.UserId && m.email == newUser.UserEmail);
                    if (_userAdmin.position == 1 || _userAdmin.position == 0)
                    {
                        user _userNew = new user();
                        _userNew = db.Users.FirstOrDefault(p => p.email.ToLower() == model.email.ToLower()); //проверяем есть ли п ользователь с таким email
                        if (_userNew == null)
                        {
                            _userNew = new user();
                            _userNew.first_name = model.first_name;
                            _userNew.last_name = model.last_name;
                            _userNew.sur_name = model.sur_name;
                            _userNew.register_day = DateTime.Now;
                            _userNew.birth_day = model.birth_day;
                            _userNew.iin = model.iin;
                            _userNew.sex = model.sex;
                            _userNew.tel_number = model.tel_number;
                            _userNew.department_id = model.department_id;

                            _userNew.check = 1;
                            _userNew.position = 2;

                            HashMD5 Hash = new HashMD5();
                            string newPassWord = Hash.CreatePassword(6);
                            _userNew.security_stamp = Hash.HashMD5String(DateTime.Now.ToString());
                            _userNew.password = newPassWord;

                            _userNew.email = model.email;
                            _userNew.company_id = _userAdmin.company_id;
                            db.Users.Add(_userNew);
                            db.SaveChanges();

                            _userNew = db.Users.Where(u => u.email.Trim() == model.email && u.password.Trim() == newPassWord).FirstOrDefault();


                            if (_userNew != null)
                            {
                                #region sendEmail 
                                string htmlCodeFull;
                                string code = _userNew.email;
                                var callbackUrl = Url.Action("ConfirmEmail", "Home", new { userId = _userNew.id, code = code }, protocol: Request.Url.Scheme);

                                StreamReader readerhtmlCodeHead = new StreamReader(Server.MapPath("~/Views/Mailing/emailConfirmHead.html"));
                                string htmlCodeHead = readerhtmlCodeHead.ReadToEnd();

                                StreamReader readerhtmlCodeFooter = new StreamReader(Server.MapPath("~/Views/Mailing/emailConfirmFooter.html"));
                                string htmlCodeFooter = readerhtmlCodeFooter.ReadToEnd();


                                string htmlCodeMiddle = "<table id=\"rec85502797\" style=\"width:100%; border-collapse:collapse; border-spacing:0; margin:0; border:0;\" cellpadding=\"0\" cellspacing=\"0\" data-record-type=\"618\"><tr><td style=\"padding-left:15px; padding-right:15px; \"><table id=\"recin85502797\" class=\"r\" style=\"margin: 0 auto;background-color:#ffffff;border-spacing: 0;width:600px;\" align=\"center\"><tr><td style=\"padding-top:30px;padding-bottom:30px;padding-left:30px;padding-right:30px;\"><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"width:100%;\"><tr><td><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"margin: 0 auto;\"><tr><td><a style=\"display: table-cell; text-decoration: none; font-size: 15px; text-align: center; font-weight: bold; font-family:Helvetica Neue, Helvetica, Arial, sans-serif; width: 100%;color:#ffffff; border:0px solid ;\" href=\" " + callbackUrl + " \"><img src=\"https://i.imgur.com/5p3P6ft.png\" alt=\"Confirm\" width=\"200px\" height=\"70px\"></a></td></tr></table></td></tr></table></td></tr></table></td></tr></table>";

                                string htmlCodeMiddle2 = "<table id=\"rec85502797\" style=\"width:100%; border-collapse:collapse; border-spacing:0; margin:0; border:0;\" cellpadding=\"0\" cellspacing=\"0\" data-record-type=\"618\"><tr><td style=\"padding-left:15px; padding-right:15px; \"><table id=\"recin85502797\" class=\"r\" style=\"margin: 0 auto;background-color:#ffffff;border-spacing: 0;width:600px;\" align=\"center\"><tr><td style=\"padding-top:30px;padding-bottom:30px;padding-left:30px;padding-right:30px;\"><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"width:100%;\"><tr><td><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"margin: 0 auto;\"><tr><td><div style=\"margin-right: auto; font-family: Helvetica Neue, Helvetica, Arial, sans-serif; color:#444444;font-size:20px;line-height:1.45;\"><p>Email: " + _userNew.email + "</p><p>Password: " + _userNew.password + "</p><br /></div></td></tr></table></td></tr></table></td></tr></table></td></tr></table>";
                                htmlCodeFull = htmlCodeHead + htmlCodeMiddle + htmlCodeMiddle2 + htmlCodeFooter;

                                Method m = new Method();
                                m.sendEmail(_userNew.email.Trim(), "Confirm registration", htmlCodeFull);
                                #endregion
                                return Json("success", JsonRequestBehavior.AllowGet); // return RedirectToAction("ConfirmWaitingPage", "Home");
                            }
                            else { return Json("error", JsonRequestBehavior.AllowGet); }
                        }
                        else
                        {
                            return Json("errorMail", JsonRequestBehavior.AllowGet); //Пользователь с таким email уже зарегистрирован 
                        }
                    }
                }
                return RedirectToAction("AccessDenied", "Home");
            }
        }

        [HttpPost]
        public ActionResult DeleteUser(string deleteUserInput)
        {
            using (DisDBContext db = new DisDBContext())
            {
                var newUser = User as CustomPrincipal;
                if (newUser != null)
                {
                    user _userAdmin = db.Users.FirstOrDefault(m => m.id == newUser.UserId && m.email == newUser.UserEmail);
                    if (_userAdmin.position == 1)
                    {
                        user _userNew = new user();
                        _userNew = db.Users.FirstOrDefault(p => (  ( (p.email.ToLower() == deleteUserInput.ToLower() || p.iin == deleteUserInput) || p.id == Int32.Parse(deleteUserInput) ) && p.company_id == _userAdmin.company_id )); //проверяем есть ли п ользователь с таким email
                        if (_userNew != null)
                        {
                            db.Users.Where(a => a.id == _userNew.id).Delete();
                            db.SaveChanges();
                            return Json("success", JsonRequestBehavior.AllowGet);
                        }
                        else { return Json("error", JsonRequestBehavior.AllowGet); }
                    }
                    else if (_userAdmin.position == 0)
                    {
                        user _userNew = new user();
                        _userNew = db.Users.FirstOrDefault(p => (p.email.ToLower() == deleteUserInput.ToLower() || p.iin == deleteUserInput) || p.id == Int32.Parse(deleteUserInput)); //проверяем есть ли п ользователь с таким email
                        if (_userNew != null)
                        {
                            db.Users.Where(a => a.id == _userNew.id).Delete();
                            db.SaveChanges();
                            return Json("success", JsonRequestBehavior.AllowGet);
                        }
                        else { return Json("error", JsonRequestBehavior.AllowGet); }
                    }
                }
                return RedirectToAction("AccessDenied", "Home");
            }

        }

        [HttpPost]
        public ActionResult CompanyChange(company model)
        {
            using (DisDBContext db = new DisDBContext())
            {
                var newUser = User as CustomPrincipal;
                if (newUser != null)
                {
                    user _userAdmin = db.Users.FirstOrDefault(m => m.id == newUser.UserId && m.email == newUser.UserEmail);
                    if (_userAdmin.position == 1 || _userAdmin.position == 0)
                    {

                        company userCompany = db.Companies.FirstOrDefault(c => c.id == _userAdmin.company_id);
                        if (userCompany != null)
                        {
                            userCompany.company_country = model.company_country;
                            userCompany.address = model.address;
                            userCompany.bank_detail = model.bank_detail;
                            userCompany.ceo_name = model.ceo_name;
                            userCompany.post_index = model.post_index;
                            userCompany.site = model.site;
                            userCompany.tell = model.tell;
                            db.SaveChanges();
                            return Json("success", JsonRequestBehavior.AllowGet);
                        }
                        else { return Json("error", JsonRequestBehavior.AllowGet); }
                    }
                }
                return RedirectToAction("AccessDenied", "Home");
            }


        }

        [HttpPost]
        public ActionResult UserProfileChange(user model)
        {
            using (DisDBContext db = new DisDBContext())
            {
                var newUser = User as CustomPrincipal;
                if (newUser != null)
                {
                    user _userAdmin = db.Users.FirstOrDefault(m => m.id == newUser.UserId && m.email == newUser.UserEmail);
                    if (_userAdmin != null)
                    {
                        _userAdmin.first_name = model.first_name;
                        _userAdmin.last_name = model.last_name;
                        _userAdmin.sur_name = model.sur_name;
                        _userAdmin.birth_day = model.birth_day;
                        _userAdmin.iin = model.iin;
                        _userAdmin.sex = model.sex;
                        _userAdmin.tel_number = model.tel_number;
                        db.SaveChanges();
                        return Json("success", JsonRequestBehavior.AllowGet);
                    }
                    else { return Json("error", JsonRequestBehavior.AllowGet); }
                }
                return RedirectToAction("AccessDenied", "Home");
            }

        }

        [HttpPost]
        public ActionResult CreateDepartment(department model)
        {
            using (DisDBContext db = new DisDBContext())
            {
                var newUser = User as CustomPrincipal;
                if (newUser != null)
                {
                    user _userAdmin = db.Users.FirstOrDefault(m => m.id == newUser.UserId && m.email == newUser.UserEmail);
                    if (_userAdmin != null)
                    {
                        if (_userAdmin.position == 1)
                        {
                            department companyDepartment = new department();
                            companyDepartment.dpt_name = model.dpt_name;
                            companyDepartment.dpt_description = model.dpt_description;
                            companyDepartment.company_id = _userAdmin.company_id;

                            db.SaveChanges();
                            return Json("success", JsonRequestBehavior.AllowGet);
                        }
                    }
                    else { return Json("error", JsonRequestBehavior.AllowGet); }
                }
            }
            return RedirectToAction("AccessDenied", "Home");
        }

        #endregion


        #region Chat View 
        public ActionResult Chat(string email)
        {
            var newUser = User as CustomPrincipal;
            ChatModel cm = new ChatModel();
            List<ChatModel> conversations = new List<ChatModel>();
            using (DisDBContext db = new DisDBContext())
            {
                ViewBag.Email = newUser.UserEmail;
                var authUser = db.Users.FirstOrDefault(p => p.id == newUser.UserId);
                ViewBag.FullName = authUser.last_name.Trim() + " " + authUser.first_name.Trim();
                var user = db.Users.FirstOrDefault(p => p.email == email);
                ViewBag.ConvName = user.last_name + " " + user.first_name;
                conversation con = db.Conversations.FirstOrDefault(p => p.user1 == newUser.UserEmail && p.user2 == email || p.user2 == newUser.UserEmail && p.user1 == email);
                if (con != null)
                {
                    ViewBag.ConvId = con.id;
                    foreach (var msg in db.Messages.Where(p => p.conversation_id == con.id))
                    {
                        conversations.Add(new ChatModel
                        {
                            fname = user.first_name,
                            lname = user.last_name,
                            date = msg.date.ToString("dd.MM.yyyy hh:mm"),
                            msg = msg.msg,
                            sender = (msg.sender == newUser.UserEmail) ? 1 : 0,
                            msg_type = msg.msg_type
                        });
                    }
                }
                else
                {
                    //Add new conversation

                    var conver = db.Conversations.Add(new conversation
                    {
                        user1 = newUser.UserEmail,
                        user2 = email
                    });
                    db.SaveChanges();
                    ViewBag.ConvId = conver.id;


                }
                // ViewBag.Conversation = conversations;
            }

            return View(conversations);
        }

        public ActionResult ChatIn(string chatMessage, string chatId)
        {
            using (DisDBContext db = new DisDBContext())
            {
                int chatid = Convert.ToInt32(chatId);
                var newUser = User as CustomPrincipal;
                if (!string.IsNullOrEmpty(chatMessage))
                {
                    message m = new message
                    {
                        conversation_id = chatid,
                        msg = chatMessage,
                        date = DateTime.Now,
                        sender = newUser.UserEmail,
                        msg_type = "msg"
                    };

                    db.Messages.Add(m);
                    db.SaveChanges();
                }



                if (Request.Files.Count > 0)
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }


                        var path = Server.MapPath("~/Files/Message/" + chatId + "/");
                        Directory.CreateDirectory(path);
                        //Сохраняем файл на сервере
                        string filePath = "/Files/Message/" + chatId + "/" + fname;
                        file.SaveAs(Server.MapPath("~/Files/Message/" + chatId + "/" + fname));

                        //Сохраняем данные о рисунках в базе данных
                        db.Messages.Add(new message
                        {
                            conversation_id = chatid,
                            msg = chatMessage,
                            date = DateTime.Now,
                            sender = newUser.UserEmail,
                            msg_type = "pic"
                        });
                        db.SaveChanges();
                    }

                    // Returns message that successfully uploaded  

                }
                var msg = db.Messages.Where(m => m.conversation_id == chatid).ToList().OrderBy(m => m.date);

                return View();
            }
        }

        #endregion


    }
}