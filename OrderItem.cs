//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HungerHunt
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderItem
    {
        public long OrderItemId { get; set; }
        public long OrderItemOrderId { get; set; }
        public long OrderItemFoodId { get; set; }
        public long OrderItemQty { get; set; }
        public long OrderItemUnitPrice { get; set; }
    
        public virtual Food Food { get; set; }
        public virtual Order Order { get; set; }
    }
}
