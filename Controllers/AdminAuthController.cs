﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HungerHunt.Controllers
{
    public class AdminAuthController : Controller
    {
        public static bool WriteExceptionToFile(string exception, out string fileException)
        {
            string exceptionFile = @"C:\Users\SHYAM SUNDER\Desktop\RLL\HungerHunt\ExceptionFiles\HungerhuntAdminException.txt";

            try
            {
                FileInfo fileInfo = new FileInfo(exceptionFile);
                if (!fileInfo.Exists)
                    using (fileInfo.CreateText()) { };

                using (StreamWriter streamWriter = fileInfo.AppendText())
                {
                    streamWriter.WriteLine(exception);
                }

                fileException = null;
                return true;
            }
            catch (Exception e)
            {
                fileException = DateTime.Now + " AdminFileWriteException : " + e.Message.ToString();
                return false;
            }
        }

        public ActionResult AdminIndex()
        {
            try
            {
                Admin admin = new Admin();
                if (admin.AdminUsername != null)
                    return View("AdminIndex", admin);
                return View(admin);
            }
            catch (Exception e)
            {
                string exceptionMessage = DateTime.Now + " ActionResult : " + Request.RequestContext.RouteData.Values["action"].ToString() + "Exception : " + e.Message.ToString();
                WriteExceptionToFile(exceptionMessage, out string fileExceptionMessage);
                return Content(exceptionMessage + "\n" + fileExceptionMessage);
            }
        }

        [HttpPost]
        public ActionResult AdminIndex(Admin admin, FormCollection forms)
        {
            try
            {
                HungerHuntEntities foodContext = new HungerHuntEntities();
                IList<Admin> AdminList = (from adm in foodContext.Admins where adm.AdminUsername == admin.AdminUsername select adm).ToList();
                long adminId = 0;
                foreach (Admin adm in AdminList) adminId = adm.AdminId;
                Admin adminFound = foodContext.Admins.Find(adminId);
                bool auth = forms["userReg"].ToString() != "reg";

                if (auth) // login
                {
                    if (adminFound != null && adminFound.AdminPassword != admin.AdminPassword)
                    {
                        admin.AdminUsername = "BadCredentials";
                        return View(admin);
                    }
                    else if (adminFound != null)
                    {
                        return RedirectToAction("AdminHome", "Admin", adminFound);
                    }
                    else
                    {
                        admin.AdminUsername = "NotRegistered";
                        return View(admin);
                    }

                }
                else //register
                {
                    if (admin.AdminUsername == null || admin.AdminFName == null || admin.AdminLName == null || admin.AdminPhone == null || admin.AdminPassword != forms["ConfirmPassword"].ToString())
                    {
                        admin.AdminUsername = "AdminInvalid";
                        return View("AdminIndex", admin);
                    }
                    else if (adminFound == null)
                    {
                        foodContext.Admins.Add(admin);
                        if (foodContext.SaveChanges() > 0)
                        {
                            admin.AdminUsername = "AdminRegistered";
                            return View("AdminIndex", admin);
                        }
                        else
                        {
                            return View(admin);
                        }
                    }
                    else
                    {
                        admin.AdminUsername = "AdminExists";
                        return View("AdminIndex", admin);
                    }

                }
            }
            catch (Exception e)
            {
                string exceptionMessage = DateTime.Now + " ActionResult : " + Request.RequestContext.RouteData.Values["action"].ToString() + "Exception : " + e.Message.ToString();
                WriteExceptionToFile(exceptionMessage, out string fileExceptionMessage);
                return Content(exceptionMessage + "\n" + fileExceptionMessage);
            }
        }
    }
}