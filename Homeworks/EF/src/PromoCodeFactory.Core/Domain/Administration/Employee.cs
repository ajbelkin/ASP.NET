using PromoCodeFactory.Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PromoCodeFactory.Core.Domain.Administration
{
    public class Employee
        : BaseEntity
    {
        [MaxLength(42)]
        public string FirstName { get; set; }

        [MaxLength(42)]
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        [MaxLength(100)]
        public string Email { get; set; }

        public virtual Role Role { get; set; }

        public int AppliedPromocodesCount { get; set; }
    }
}