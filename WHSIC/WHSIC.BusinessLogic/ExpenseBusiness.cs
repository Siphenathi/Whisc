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
    public class ExpenseBusiness
    {
        public List<ExpenseViewModel> GetAllExpenses()
        {
            using (var pr = new ExpenseRepository())
            {
                return pr.getAll().Select(x => new ExpenseViewModel()
                {
                    exp_id = x.exp_id,
                    desc = x.desc,
                    payment_due_date=x.payment_due_date,
                    exp_acc_no=x.exp_acc_no,
                    from_acc_no=x.from_acc_no,
                    amount=x.amount,
                    recurring=x.recurring
                }).ToList();
            }
        }

        public void AddExpense(ExpenseViewModel model)
        {
            using (var pr = new ExpenseRepository())
            {
                var z = new Expense
                {
                   desc=model.desc,
                   payment_due_date=model.payment_due_date,
                   exp_acc_no=model.exp_acc_no,
                   from_acc_no=model.from_acc_no,
                   amount=model.amount,
                   recurring=model.recurring
                };
                pr.Save(z);
            }
        }
        public void DeleteExpense(int id)
        {
            using (var pr = new ExpenseRepository())
            {
                Expense p = pr.GetByID(id);

                if (p != null)
                {
                    pr.Delete(p);
                }
            }

        }
        public ExpenseViewModel GetbyID(int id)
        {
            using (var pr = new ExpenseRepository())
            {
                Expense p = pr.GetByID(id);
                var view = new ExpenseViewModel();

                if (p != null)
                {
                    view.exp_id = p.exp_id;
                    view.desc = p.desc;
                    view.amount = p.amount;
                    view.exp_acc_no = p.exp_acc_no;
                    view.payment_due_date = p.payment_due_date;
                    view.from_acc_no = p.from_acc_no;
                    view.recurring = p.recurring;
                }
                return view;
            }
        }
        public void UpdateExpense(ExpenseViewModel model)
        {
            using (var pr = new ExpenseRepository())
            {
                Expense p = pr.GetByID(model.exp_id);

                if (p != null)
                {
                    p.desc = model.desc;
                    p.amount = model.amount;
                    p.exp_acc_no = model.exp_acc_no;
                    p.payment_due_date = model.payment_due_date;
                    p.from_acc_no = model.from_acc_no;
                    p.recurring = model.recurring;

                    pr.Update(p);
                }

            }

        }
    }
}
