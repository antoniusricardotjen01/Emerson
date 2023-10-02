using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmersonDB.Model
{
    [Table("City")]
    public class City
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(255)]
        [Column("CityName")]
        public string CityName { get; set; }
    }
}
