﻿using System.ComponentModel.DataAnnotations.Schema;
using Uni_Mate.Models.UserManagment;

namespace Uni_Mate.Models.ApartmentManagement
{
    public class FavoriteApartment:BaseEntity
    {
        [ForeignKey(nameof(Apartment))]
        public int ApartmentId { get; set; }
        public Apartment? Apartment { get; set; }
        [ForeignKey(nameof(Student))]
        public string? UserId { get; set; }
        public User? User { get; set; }
    }
}
