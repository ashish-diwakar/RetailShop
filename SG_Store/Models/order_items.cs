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
    
    public partial class order_items
    {
        public int order_item_id { get; set; }
        public int order_id { get; set; }
        public int stock_id { get; set; }
        public int quantity { get; set; }
        public decimal list_price { get; set; }
        public decimal discount { get; set; }
        public Nullable<decimal> quantity_kg { get; set; }
        public Nullable<decimal> total_amount { get; set; }
    
        public virtual order order { get; set; }
        public virtual stock stock { get; set; }
    }
}
