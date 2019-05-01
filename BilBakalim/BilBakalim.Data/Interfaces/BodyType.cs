using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BilBakalim.Data.Interfaces
{
  public  class BodyType
    {
        public BodyType()
        {

            //SinifKat = new List<SinifKategori>();

        }
        public string Ad { get; set; }
        public HttpPostedFileBase PicData { get; set; }
        //public List<SinifKategori> SinifKat { get; set; }
        //public SinifKategori Sinifi { get; set; }
       // public HttpPostedFileBase resim { get; set; }

    }
}
