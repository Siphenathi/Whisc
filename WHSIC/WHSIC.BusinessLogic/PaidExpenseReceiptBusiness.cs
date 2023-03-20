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
    public class PaidExpenseReceiptBusiness
    {


        public List<PaidExpenseReceiptViewModel> GetAllExpenses_PaidReceipt()
        {
            using (var pr = new PaidExpenseReceiptRepository())
            {
                return pr.getAll().Select(x => new PaidExpenseReceiptViewModel()
                {
                    paid_exp_id = x.paid_exp_id,
                    Id = x.Id,
                    Expense_Name = x.Expense_Paid.Expences.desc,
                    Date = x.Date,
                    Receipt = x.Receipt
                }).ToList();
            }
        }

        public void AddReceipt(byte[] ReceiptPath, int paid_exp_id)
        {
            using (var pr = new PaidExpenseReceiptRepository())
            {
                var z = new PaidExpenseReceipt
                {
                    Date = DateTime.Now,
                    Receipt = ReceiptPath,
                    paid_exp_id = paid_exp_id
                };
                pr.Save(z);
            }
        }

        public void DeleteExpense_PaidReceipt(int paid_exp_idpar)
        {
            using (var pr = new PaidExpenseReceiptRepository())
            {
 
                List<PaidExpenseReceiptViewModel> list = (from s in GetAllExpenses_PaidReceipt()
                                                 where s.paid_exp_id==paid_exp_idpar
                                                 select s).ToList(); 
                    foreach (var p in list)
                    {
                        PaidExpenseReceipt per = pr.GetByID(p.Id);
                        pr.Delete(per);
                    }

            }

        }
        public void DeleteExpense_PaidReceiptOnly(int id)
        {
            using (var pr = new PaidExpenseReceiptRepository())
            {
                       PaidExpenseReceipt per = pr.GetByID(id);
              if(per!=null)
                {
                    pr.Delete(per);
                }

            }
        }
        public PaidExpenseReceiptViewModel GetbyID(int id)
        {
            using (var pr = new PaidExpenseReceiptRepository())
            {
                PaidExpenseReceipt p = pr.GetByID(id);
                var view = new PaidExpenseReceiptViewModel();

                if (p != null)
                {
                    view.paid_exp_id = p.paid_exp_id;
                    view.Id = p.Id;
                    view.Date = p.Date;
                    view.Expense_Name = p.Expense_Paid.Expences.desc;
                    view.Receipt = p.Receipt;
                }
                return view;
            }
        }


        public void UpdateExpense_PaidReceipt(PaidExpenseReceiptViewModel model)
        {
            using (var pr = new PaidExpenseReceiptRepository())
            {
                PaidExpenseReceipt p = pr.GetByID(model.Id);
                if (p != null)
                {
                    p.Receipt = model.Receipt;
                    p.Date = model.Date;
                    p.paid_exp_id = model.paid_exp_id;
                    pr.Update(p);
                }
            }
        }
    }
}
