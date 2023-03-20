using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHSIC.Data;
using WHSIC.Model;
using WHSIC.Service;

namespace WHSIC.BusinessLogic
{
    public class GalleryBusiness
    {
        public List<GalleryViewModel> GetAllImages()
        {
            using (var pr = new GalleryRepository())
            {
                return pr.getAll().Select(x => new GalleryViewModel()
                {
                    GalleryId = x.GalleryId,
                    ImageData = x.ImageData
                }).ToList();

            }
        }

        public void AddImage(GalleryViewModel model)
        {
            using (var pr = new GalleryRepository())
            {
                var p = new Gallery
                {
                    ImageData = model.ImageData
                };
                pr.Save(p);
            }
        }
        public void DeleteImage(int id)
        {
            using (var pr = new GalleryRepository())
            {
                Gallery p = pr.GetByID(id);

                if (p != null)
                {
                    pr.Delete(p);
                }
            }
        }
        public GalleryViewModel GetbyID(int id)
        {
            using (var pr = new GalleryRepository())
            {
                Gallery p = pr.GetByID(id);
                var view = new GalleryViewModel();

                if (p != null)
                {
                    view.ImageData = p.ImageData;
                    view.GalleryId = p.GalleryId;
                }
                return view;
            }


        }
    }
}
