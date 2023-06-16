using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Items {

    public abstract class Entity {

        [Key]
        public string Id { get; set; } = null!;
    }
}
