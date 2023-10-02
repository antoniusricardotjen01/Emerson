using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmersonDB.Model
{
    [Table("Variable")]
    public class Variable
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(255)]
        [Column("Name")]
        public string Name { get; set; }


        [Required(AllowEmptyStrings = true)]
        [MaxLength(255)]
        [Column("Unit")]
        public string Unit { get; set; }


        [Required(AllowEmptyStrings = true)]
        [MaxLength(255)]
        [Column("Value")]
        public string Value { get; set; }

        [Column("CityId")]
        public int? CityId { get; set; }

        [Column("Timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        [ForeignKey("CityId")]
        public virtual City City { get; set; }

    }
}
