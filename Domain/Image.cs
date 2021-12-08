using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Image
    {
        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public Image()
        {

        }

        public Image(string title, string imageUrl)
        {
            Title = title;
            ImageUrl = imageUrl;
        }
    }
}
