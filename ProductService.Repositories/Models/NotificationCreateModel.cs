using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Repositories.Models
{
    public class NotificationCreateModel
    {
        public string Message { get; set; }
        public DateTime Created { get; set; }
    }
}
