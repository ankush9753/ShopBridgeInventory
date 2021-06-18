namespace ShopBridgeInventory.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public string Color { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int AvailableQuantity { get; set; }
    }
}
