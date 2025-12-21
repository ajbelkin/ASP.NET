using PromoCodeFactory.Core.Domain;
using PromoCodeFactory.Core.Domain.Administration;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime;

namespace PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    public class PromoCode
        : BaseEntity
    {
        [MaxLength(20)]
        public string Code { get; set; }

        [MaxLength(100)]
        public string ServiceInfo { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        [MaxLength(100)]
        public string PartnerName { get; set; }

        public Guid CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Employee PartnerManager { get; set; }

        public virtual Preference Preference { get; set; }
    }
}