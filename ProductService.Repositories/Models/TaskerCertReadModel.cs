using ProductService.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Repositories.Models
{
    public class TaskerCertReadModel
    {
        public int TaskerId { get; set; }

        public string TaskerName { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public DateTime CreateAt { get; set; }

        public bool Status { get; set; }

    }
}
