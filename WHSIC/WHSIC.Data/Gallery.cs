using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHSIC.Data
{
    public class Gallery
    {
        
            [Key]
            public int GalleryId { get; set; }
            public byte[] ImageData { get; set; }
    }
}
