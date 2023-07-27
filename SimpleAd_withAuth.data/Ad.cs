using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAd_withAuth.data
{
    public class Ad
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ListerId { get; set; }

        public DateTime ListingDate { get; set; }

        public string PhoneNumber { get; set; }

        public string Listing { get; set; }

        public bool Delete { get; set; }


    }
}
