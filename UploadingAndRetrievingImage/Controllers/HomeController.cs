using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UploadingAndRetrievingImage.Models;


namespace UploadingAndRetrievingImage.Controllers
{
    public class HomeController : Controller
    {

        NewDbEntities db= new NewDbEntities();

        public ActionResult Index()
        {
            var data = db.ImageTables.ToList();
            return View(data);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(ImageTable s)
        {
            
            string fileName = Path.GetFileNameWithoutExtension(s.ImageFile.FileName);
            string extension = Path.GetExtension(s.ImageFile.FileName);
            fileName = fileName + extension;
            s.image_path= "~/Images/"+fileName;
            fileName=Path.Combine(Server.MapPath("~/Images"), fileName);
            s.ImageFile.SaveAs(fileName);
            db.ImageTables.Add(s);
             int a=db.SaveChanges();

            if (a > 0)
            {
                TempData["msg"] = "<script>alert('Data inserted successfully')</script>";
                ModelState.Clear();
            }
            else
            {
                TempData["msg"] = "<script> alert('Recond  not Insered!!!')   <script>";
            }

            return RedirectToAction("Index", TempData["msg"]);
        }


        public ActionResult Edit(int id)
        {
            var edit = db.ImageTables.Where(model => model.id == id).FirstOrDefault();
            Session["Image"] = edit.image_path;
            return View(edit);
        }
        [HttpPost]
        public ActionResult Edit(ImageTable s)
        {

            if (ModelState.IsValid == true)
            {
                if(s.ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(s.ImageFile.FileName);
                    string extension = Path.GetExtension(s.ImageFile.FileName);
                    fileName = fileName + extension;
                    s.image_path = "~/Images/" + fileName;
                    s.image_path = Session["Image"].ToString();
                    fileName = Path.Combine(Server.MapPath("~/Images"), fileName);
                    s.ImageFile.SaveAs(fileName);
                    db.Entry(s).State = EntityState.Modified;
                    int a = db.SaveChanges();

                    if (a > 0)
                    {
                        TempData["msg"] = "<script>alert('Data inserted successfully')</script>";
                        ModelState.Clear();
                    }
                    else
                    {
                        TempData["msg"] = "<script> alert('Recond  not Insered!!!')   <script>";
                    }

                    return RedirectToAction("Index", TempData["msg"]);

                }
                else
                {
                    s.image_path = Session["Image"].ToString();
                    db.Entry(s).State = EntityState.Modified;
                    int a = db.SaveChanges();

                    if (a > 0)
                    {
                        TempData["msg"] = "<script>alert('Data inserted successfully')</script>";
                        ModelState.Clear();
                    }
                    else
                    {
                        TempData["msg"] = "<script> alert('Recond  not Insered!!!')   <script>";
                    }

                }

            }
            return RedirectToAction("Index", TempData["msg"]);
        }



        public ActionResult Delete(int id)
        { 
            if(id>0)
            {
                var e= db.ImageTables.Where(model=>model.id==id).FirstOrDefault();
                db.Entry(e).State= EntityState.Deleted;
                db.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }

    }


}