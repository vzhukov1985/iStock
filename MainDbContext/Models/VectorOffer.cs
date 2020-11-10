using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;

namespace DbCore.Models
{
    public enum VectorOfferStatus
    {
        InActive,
        Active,
        DescriptionChanged,
        PriceChanged,
        PriceAndDescriptionChanged
    }
    public class VectorOffer
    {
        public Guid Id { get; set; }
        public bool IsVerified { get; set; }
        public VectorOfferStatus Status { get; set; }
        public string Sku { get; set; }
        public string Brand { get; set; }
        public string Name { get; set; }
        public double? Price { get; set; }
        public double? PriceLimit { get; set; }
        public bool? Pp1 { get; set; }
        public bool? Pp2 { get; set; }
        public bool? Mskdnt { get; set; }
        public bool? Nnach { get; set; }
        public int? PreorderInDays { get; set; }
        public bool? IsFavorite { get; set; }
        public string Supplier { get; set; }
        public Guid PricelistId { get; set; }
        
        [NotMapped]
        public object this[string propertyName]
        {
            get
            {
                Type myType = typeof(VectorOffer);
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                return myPropInfo.GetValue(this, null);
            }
            set
            {
                Type myType = typeof(VectorOffer);
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                myPropInfo.SetValue(this, value, null);
            }
        }
    }
}
