using PromoCodeFactory.Core.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace PromoCodeFactory.Core.Domain.Administration
{
    public class Role
        : BaseEntity
    {
        [MaxLength(45)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
    }
}