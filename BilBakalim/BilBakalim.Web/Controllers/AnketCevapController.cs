using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BilBakalim.Data;

namespace BilBakalim.Web.Controllers
{
    public class AnketCevapController : Controller
    {
        private BilBakalimContext db = new BilBakalimContext();

        private static List<AnketSoru> anketSoru;

        private static AnketSoru gosterilenAnketSoru;

        private static int basimDurumu = 1;

        // GET: AnketCevap
        public ActionResult AnketSoru(int? id, string secim)
        {
            try
            {
                if (anketSoru == null && id != null)
                {
                    anketSoru = db.AnketSoru.Where(x => x.SinifID == id).ToList();
                }
                else if (anketSoru != null)
                {
                    if (anketSoru.Count == 0)
                    {
                        anketSoru = null;
                        gosterilenAnketSoru = null;
                        basimDurumu = 1;
                        return RedirectToAction("AnketSoruSon");
                    }
                }

                if (secim != null)
                {
                    AnketOturum anketOturum = db.AnketOturum.FirstOrDefault(x => x.SoruID == gosterilenAnketSoru.ID && x.Pin == null);
                    if (anketOturum == null)
                    {
                        anketOturum = new AnketOturum();
                        anketOturum.SoruID = gosterilenAnketSoru.ID;
                        switch (secim)
                        {
                            case "a":
                                anketOturum.Cevap1 = 1;
                                anketOturum.Cevap2 = 0;
                                anketOturum.Cevap3 = 0;
                                anketOturum.Cevap4 = 0;
                                basimDurumu = 1;
                                break;
                            case "b":
                                anketOturum.Cevap1 = 0;
                                anketOturum.Cevap2 = 1;
                                anketOturum.Cevap3 = 0;
                                anketOturum.Cevap4 = 0;
                                basimDurumu = 1;
                                break;
                            case "c":
                                anketOturum.Cevap1 = 0;
                                anketOturum.Cevap2 = 0;
                                anketOturum.Cevap3 = 1;
                                anketOturum.Cevap4 = 0;
                                basimDurumu = 1;
                                break;
                            case "d":
                                anketOturum.Cevap1 = 0;
                                anketOturum.Cevap2 = 0;
                                anketOturum.Cevap3 = 0;
                                anketOturum.Cevap4 = 1;
                                basimDurumu = 1;
                                break;
                        }

                        db.AnketOturum.Add(anketOturum);
                    }
                    else
                    {
                        switch (secim)
                        {
                            case "a":
                                anketOturum.Cevap1++;
                                basimDurumu = 1;
                                break;
                            case "b":
                                anketOturum.Cevap2++;
                                basimDurumu = 1;
                                break;
                            case "c":
                                anketOturum.Cevap3++;
                                basimDurumu = 1;
                                break;
                            case "d":
                                anketOturum.Cevap4++;
                                basimDurumu = 1;
                                break;
                        }
                    }

                    db.SaveChanges();
                }

                return View();
            }
            catch
            {
                AnketSoru anketSoru = new AnketSoru();
                return View(anketSoru);
            } 
        }

        public PartialViewResult AnketSoruGec()
        {
            if (anketSoru != null)
            {
                if (anketSoru.Count != 0 && basimDurumu == 1)
                {
                    gosterilenAnketSoru = anketSoru.First();
                    anketSoru.Remove(gosterilenAnketSoru);
                    basimDurumu = 0;
                }    
            }
            return PartialView(gosterilenAnketSoru);
        }

        public ActionResult AnketSoruSon()
        {
            return View();
        }

    }
}