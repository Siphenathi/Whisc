using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WHSIC.Model;
using WHSIC.Data;

namespace WHSIC.BusinessLogic
{
    public class Recurring_Expense
    {
        ExpenseBusiness eb = new ExpenseBusiness();
        Expense_PaidBusiness epaid = new Expense_PaidBusiness();
        ApplicationDbContext db = new ApplicationDbContext();

        public List<ExpenseViewModel> ExpList()
        {
            List<ExpenseViewModel> myList = (from e in eb.GetAllExpenses() where e.recurring == true select e).ToList();
            return myList;
        }
        public void RecurringExpensePayment()
        {
            for (int x = 0; x < ExpList().Count; x++)
            {
                if(Convert.ToInt32(ExpList()[x].payment_due_date) == GetNistTime().Day)
                  {
                    var search = epaid.GetAllExpenses_Paid().Where(a => a.date_paid.Date.ToShortDateString().Equals(GetNistTime().ToShortDateString()) && a.exp_id==ExpList()[x].exp_id).FirstOrDefault();

                    if(search==null)
                    {
                        Expense_PaidViewModel paid = new Expense_PaidViewModel
                        {
                            exp_id=ExpList()[x].exp_id,
                            date_paid=GetNistTime(),
                            amount_paid=ExpList()[x].amount,
                            Reference="Bank"
                        };
                        epaid.AddExpense_Paid(paid);
                        SubtractIncome(paid.amount_paid);
                    }
                }
            }
        }

        public void SubtractIncome(decimal amt)
        {
            decimal val = 0;

            foreach (var CO in db.Incomes.ToList())
            {
                val = CO.TotIncome;
                val = val - amt;
                CO.TotIncome = val;
                db.SaveChanges();
            }
        }

        public DateTime GetNistTime()
        {
            DateTime dateTime = DateTime.Now;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://nist.time.gov/actualtime.cgi?lzbc=siqm9b");
                request.Method = "GET";
                request.Accept = "text/html, application/xhtml+xml, */*";
                request.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; Trident/6.0)";
                request.ContentType = "application/x-www-form-urlencoded";
                request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader stream = new StreamReader(response.GetResponseStream());
                    string html = stream.ReadToEnd();
                    string time = Regex.Match(html, @"(?<=\btime="")[^""]*").Value;
                    double milliseconds = Convert.ToInt64(time) / 1000.0;
                    dateTime = new DateTime(1970, 1, 1).AddMilliseconds(milliseconds).ToLocalTime();
                }
            }
            catch (Exception c)
            {

            }
            return dateTime;
        }
    }
}
