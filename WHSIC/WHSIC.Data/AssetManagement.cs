using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHSIC.Data
{
    class AssetManagement
    {

    }
    public class Asset_Renter
    {
        [Key]
        public int Renter_code { get; set; }
        public string FullName { get; set; }
        public string Contact { get; set; }
        public string email { get; set; }
        public string Physical_Address { get; set; }
        public string Code { get; set; }
        public byte[] Contract { get; set; }

        public ICollection<Asset_Rentalz> Asset_Rentals { get; set; }
        public ICollection<Damaged_Asset> Damaged_Assets { get; set; }
    }
    public class Supplier
    {
        [Key]
        public int supply_code { get; set; }
        public string name { get; set; }
        public string contact { get; set; }

        public string email { get; set; }
        public string address { get; set; }
        public ICollection<Asset> Asset { get; set; }
    }

    public class Asset_Rentalz
    {
        [Key]
        public int Rental_code { get; set; }
        public int Asset_Code { get; set; }
        public int Renter_code { get; set; }

        public DateTime HireDate { get; set; }

        public DateTime ReturnDate { get; set; }

        public decimal Amount { get; set; }

        public string Condition { get; set; }

        public int quantity { get; set; }
        public int returnedQty { get; set; }
        public bool Returned { get; set; }
        public virtual Asset_Renter Asset_Renters { get; set; }
        public virtual Asset Assets { get; set; }
    }

    public class Damaged_Asset
    {
        [Key]
        public int Damaged_ID { get; set; }
        public int Asset_Code { get; set; }
        public int Renter_code { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public DateTime PayDate { get; set; }
        public byte[] Quotation { get; set; }
        public virtual Asset_Renter Asset_Renter { get; set; }
        public virtual Asset Assets { get; set; }

    }
    public class Asset_Category
    {
        [Key]
        public int CategoryID { get; set; }
        public string CName { get; set; }
        public string Type { get; set; }
        public ICollection<Asset> Assets { get; set; }
    }

    public class Asset
    {
        [Key]
        public int Asset_Code { get; set; }

        public int supply_code { get; set; }
        public int CategoryID { get; set; }
        public int quantity { get; set; }
        public string AName { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string LifeSpan { get; set; }
        public string Warranty { get; set; }
        public bool ForRental { get; set; }
        public virtual Asset_Category Asset_Categories { get; set; }
        public virtual Supplier Suppliers { get; set; }
        public ICollection<Damaged_Asset> Damaged_Assets { get; set; }
        public ICollection<AssetCharge> AssetCharges { get; set; }
        public ICollection<Asset_Rentalz> Asset_Rentals { get; set; }
    }
    public class AssetCharge
    {
        [Key]
        public int charge_ID { get; set; }
        public int Asset_Code { get; set; }
        public decimal Rental_Amount { get; set; }
        public decimal PenaltyRate { get; set; }
        public virtual Asset Assets { get; set; }
    }

    public class Penalty
    {
        [Key]
        public int PID { get; set; }
        public int Rental_ID { get; set; }
        public decimal AmountBefore { get; set; }
        public decimal AddedAmount { get; set; }
        public DateTime ReturnDate { get; set; }
        public DateTime RecordDate { get; set; }
    }
}
