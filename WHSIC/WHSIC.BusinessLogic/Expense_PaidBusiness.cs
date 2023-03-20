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
    public class Expense_PaidBusiness
    {
        PaidExpenseReceiptBusiness perb = new PaidExpenseReceiptBusiness();
        public List<Expense_PaidViewModel> GetAllExpenses_Paid()
        {
            using (var pr = new Expense_PaidRepository())
            {
                return pr.getAll().Select(x => new Expense_PaidViewModel()
                {
                    paid_exp_id = x.paid_exp_id,
                    exp_id = x.exp_id,
                    date_paid = x.date_paid,
                    amount_paid = x.amount_paid,
                    Reference=x.Reference,
                    ExpenseDesc =x.Expences.desc,
                }).ToList();
            }
        }
        
        public void AddExpense_Paid(Expense_PaidViewModel model)
        {
            using (var pr = new Expense_PaidRepository())
            {
                var z = new Expense_Paid
                {
                    exp_id = model.exp_id,
                    date_paid = model.date_paid,
                    amount_paid = model.amount_paid,
                    Reference=model.Reference,
                };
                pr.Save(z);
                Expense_PaidViewModel exp = GetAllExpenses_Paid().Last();
                perb.AddReceipt(model.Receipt, exp.paid_exp_id);
            }
        }

        public void DeleteExpense_Paid(int id)
        {
            using (var pr = new Expense_PaidRepository())
            {
                Expense_Paid p = pr.GetByID(id);

                if (p != null)
                {
                    perb.DeleteExpense_PaidReceipt(p.paid_exp_id);
                    pr.Delete(p);
                }
            }

        }
        public Expense_PaidViewModel GetbyID(int id)
        {
            using (var pr = new Expense_PaidRepository())
            {
                Expense_Paid p = pr.GetByID(id);
                var view = new Expense_PaidViewModel();

                if (p != null)
                {
                    view.paid_exp_id = p.paid_exp_id;
                    view.exp_id = p.exp_id;
                    view.date_paid = p.date_paid;
                    view.amount_paid = p.amount_paid;
                    view.ExpenseDesc = p.Expences.desc;
                    view.Reference = p.Reference;
                }
                return view;
            }
        }
        public void UpdateExpense_Paid(Expense_PaidViewModel model)
        {
            using (var pr = new Expense_PaidRepository())
            {
                Expense_Paid p = pr.GetByID(model.paid_exp_id);
                if (p != null)
                {
                    p.exp_id = model.exp_id;
                    p.date_paid = model.date_paid;
                    p.amount_paid = model.amount_paid;

                    pr.Update(p);
                    perb.AddReceipt(model.Receipt, model.paid_exp_id);
                }
            }
        }
    }
}

