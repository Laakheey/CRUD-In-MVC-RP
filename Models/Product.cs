using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_Assignment.Models
{
    public enum Category
    {
        Electronic = 1,
        Clothes = 2,
        Shoes = 3,
        Toy = 4,
        Others = 5
    }
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
        [Required]
        [StringLength(50)]
        [MinLength(3)]
        public string ProductName { get; set; }
        [Required]
        [MinLength(2)]
        public string Description { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        public string Image { get; set; }
        [Required]
        public Category Category { get; set; }
        public IEnumerable<Product> Products { get; set;}


    }

}