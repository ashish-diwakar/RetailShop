//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SG_Store.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class stock
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public stock()
        {
            this.order_items = new HashSet<order_items>();
        }
    
        public int store_id { get; set; }
        public int product_id { get; set; }
        public Nullable<int> quantity { get; set; }
        public Nullable<int> vendor_id { get; set; }
        public Nullable<decimal> quantity_kg { get; set; }
        public Nullable<decimal> MRP { get; set; }
        public Nullable<decimal> purchase_price { get; set; }
        public Nullable<bool> is_per_kg_item { get; set; }
        public Nullable<bool> is_active { get; set; }
        public Nullable<System.DateTime> added_on { get; set; }
        public Nullable<decimal> total_bill_amount { get; set; }
        public Nullable<decimal> sale_discount_percentage { get; set; }
        public Nullable<decimal> sale_price { get; set; }
        public string item_code { get; set; }
        public string item_barcode { get; set; }
        public Nullable<int> sold_quantity { get; set; }
        public Nullable<decimal> sold_quantity_kg { get; set; }
        public int stock_id { get; set; }
    
        public virtual product product { get; set; }
        public virtual store store { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<order_items> order_items { get; set; }
        public virtual vendor vendor { get; set; }
    }
}