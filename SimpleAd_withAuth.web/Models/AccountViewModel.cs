using SimpleAd_withAuth.data;

namespace SimpleAd_withAuth.web.Models
{
    public class AccountViewModel
    {
        public List<Ad> AllAds { get; set; }

        public Lister Lister { get; set; }
    }
}
