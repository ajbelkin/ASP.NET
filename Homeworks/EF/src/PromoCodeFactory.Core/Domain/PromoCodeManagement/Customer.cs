using PromoCodeFactory.Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    public class Customer
        : BaseEntity
    {
        [MaxLength(42)]
        public string FirstName { get; set; }

        [MaxLength(42)]
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        [MaxLength(100)]
        [Required]
        public string Email { get; set; }

        public virtual List<Preference> Preferences { get; set; } = [];

        public virtual List<PromoCode> PromoCodes { get; set; } = [];
    }
}