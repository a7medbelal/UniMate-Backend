
﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿using Uni_Mate.Models.GeneralEnum;
using Uni_Mate.Models.UserManagment;
namespace Uni_Mate.Models.ApartmentManagement
{
    public class Apartment : BaseEntity
    {
        public int Num { get; set; }
        public string? Description { get; set; }
        public string  Location { get; set; }
        public string ? DescripeLocation{ get; set; }
        public int NumberOfRooms { get; set; }
        public int Capecity { get; set; }
        [Precision(18, 2)]
        public  decimal? Price { get; set; }
        public Gender Gender { get; set; }
        public string? Floor { get; set; }
        public bool IsAvailable { get; set; }
        public ApartmentDurationType DurationType { get; set; }

        public string? OwnerID { get; set; }
        public Owner? Owner { get; set; }

        public ICollection<Room>? Rooms { get; set; }

        public ICollection<Image>? Images { get; set; }

        public ICollection<ApartmentFacility> ApartmentFacilities { get; set; } = new List<ApartmentFacility>();

        public ICollection<FavoriteApartment>? FavoriteStudent { get; set; }

    }
}
