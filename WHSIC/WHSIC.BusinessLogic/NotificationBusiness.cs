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

    public class NotificationBusiness
    {
        public List<NotificationViewModel> GetAllNotifications()
        {
            using (var pr = new NotificationRepository())
            {
                return pr.getAll().Select(x => new NotificationViewModel
                {
                    NotID = x.NotID,
                    Text = x.Text,
                    DateTime = x.DateTime,
                    Red = x.Red

                }).ToList();

            }
        }

        public void AddNotification(NotificationViewModel model)
        {
            using (var pr = new NotificationRepository())
            {
                var p = new Notification
                {
                    NotID = model.NotID,
                    Text = model.Text,
                    DateTime = model.DateTime,
                    Red = model.Red
                };
                pr.Save(p);
            }
        }

        public NotificationViewModel GetbyID(int id)
        {
            using (var pr = new NotificationRepository())
            {
                Notification p = pr.GetByID(id);
                var view = new NotificationViewModel();

                if (p != null)
                {
                    view.NotID = p.NotID;
                    view.Text = p.Text;
                    view.DateTime = p.DateTime;
                    view.Red = p.Red;
                }
                return view;
            }
        }

        public void UpdateNotification(NotificationViewModel model)
        {
            using (var pr = new NotificationRepository())
            {
                Notification p = pr.GetByID(model.NotID);
                if (p != null)
                {
                    p.Red = model.Red;
                    pr.Update(p);
                }

            }
        }

    }
}
