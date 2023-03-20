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
    public  class MemberBusiness
    {
        public List<MemberViewModel> GetAllMembers()
        {
            using (var pr = new MemberRepository())
            {
                return pr.getAll().Select(x => new MemberViewModel()
                {
                    id_number=x.id_number,
                    BranchID=x.BranchID,
                    name=x.name,
                    surname=x.surname,
                    email=x.email,
                    contact=x.contact,
                    employed=x.employed,
                    marital_status=x.marital_status,
                    gender=x.gender,
                    home_address=x.home_address,
                    postal_address=x.postal_address,
                    next_of_kin=x.next_of_kin,
                    kin_contact=x.kin_contact,
                    branchName=x.branches.BranchName
                }).ToList();

            }
        }

        public void AddMembers(MemberViewModel model)
        {
            using (var pr = new MemberRepository())
            {
                var p = new Member
                {
                    id_number=model.id_number,
                    BranchID = model.BranchID,
                    name = model.name,
                    surname = model.surname,
                    email = model.email,
                    contact = model.contact,
                    employed = model.employed,
                    marital_status = model.marital_status,
                    gender = model.gender,
                    home_address = model.home_address,
                    postal_address = model.postal_address,
                    next_of_kin = model.next_of_kin,
                    kin_contact = model.kin_contact
                };
                pr.Save(p);
            }
        }

        public void DeleteMembers(string id)
        {
            using (var pr = new MemberRepository())
            {
                Member p = pr.GetByID(id);

                if (p != null)
                {
                    pr.Delete(p);
                }
            }

        }

        public MemberViewModel GetbyID(string id)
        {
            using (var pr = new MemberRepository())
            {
                Member p = pr.GetByID(id);
                var view = new MemberViewModel();

                if (p != null)
                {
                    view.id_number = p.id_number;
                    view.BranchID = p.BranchID;
                    view.name = p.name;
                    view.surname = p.surname;
                    view.email = p.email;
                    view.contact = p.contact;
                    view.employed = p.employed;
                    view.marital_status = p.marital_status;
                    view.gender = p.gender;
                    view.home_address = p.home_address;
                    view.postal_address = p.postal_address;
                    view.next_of_kin = p.next_of_kin;
                    view.kin_contact = p.kin_contact;
                }
                return view;
            }


        }
        public void UpdateMembers(MemberViewModel model)
        {
            using (var pr = new MemberRepository())
            {
                Member p = pr.GetByID(model.id_number);

                if (p != null)
                {
                    p.id_number = model.id_number;
                    p.BranchID = model.BranchID;
                    p.name = model.name;
                    p.surname = model.surname;
                    p.email = model.email;
                    p.contact = model.contact;
                    p.employed = model.employed;
                    p.marital_status = model.marital_status;
                    p.gender = model.gender;
                    p.home_address = model.home_address;
                    p.postal_address = model.postal_address;
                    p.next_of_kin = model.next_of_kin;
                    p.kin_contact = model.kin_contact;

                    pr.Update(p);
                }

            }

        }
    }
}
