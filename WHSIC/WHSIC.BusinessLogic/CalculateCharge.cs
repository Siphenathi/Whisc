using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WHSIC.Data;

namespace WHSIC.BusinessLogic
{
    public class CalculateCharge
    {
       private ApplicationDbContext db = new ApplicationDbContext();
        SMS sms = new SMS();
        public void DoTheWork()
        {
            decimal additionalcost = 0,increase=0;
            List<Asset_Rentalz> RentalList = (from r in db.Asset_Rentals.ToList() where r.Returned==false select r).ToList();
            foreach(var re in RentalList)
            {
                DateTime now = DateTime.Now;
                Penalty find = db.Penalties.ToList().Find(x => x.RecordDate.ToShortDateString().Equals(now.ToShortDateString()) && x.Rental_ID == re.Rental_code);
                if (find == null)
                {
                    if(re.ReturnDate.AddDays(1)<now)
                    {
                        AssetCharge charge = db.AssetCharges.ToList().Find(x => x.Asset_Code == re.Asset_Code);
                        additionalcost = charge.Rental_Amount * (re.quantity - re.returnedQty);
                        additionalcost = additionalcost * (charge.PenaltyRate / 100);
                        increase = additionalcost;

                        Asset_Rentalz rent = db.Asset_Rentals.ToList().Find(x => x.Rental_code == re.Rental_code);
                        rent.Amount = rent.Amount + additionalcost;

                        Penalty pe = new Penalty
                        {
                            Rental_ID = re.Rental_code,
                            RecordDate = now,
                            ReturnDate = re.ReturnDate,
                            AmountBefore=re.Amount,
                            AddedAmount=additionalcost
                            
                        };
                        db.Penalties.Add(pe);
                        db.SaveChanges();
                        Asset ass = db.Assets.Find(re.Asset_Code);
                        Asset_Renter renter = db.Asset_Renters.Find(re.Renter_code);
                        //sms.sending_full_sms(renter.Contact,"Hi "+renter.FullName+" , a penalty of "+ increase.ToString("R00.00")+" has been added into your rental amount for not returning "+ ass.AName+" on "+re.ReturnDate.ToShortDateString());
                    }
                }
            }
        }
    }
}