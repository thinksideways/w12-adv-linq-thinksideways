using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleRpgEntities.Models.Equipments;


// TODO note this model has been updated from the previous version so a migration will be needed
public class Item
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }

    public int Attack { get; set; }
    public int Defense { get; set; }

    [ForeignKey(nameof(Inventory))]
    public int InventoryId { get; set; }

    // TODO: Update assignment template decimal sizing for the provided seed data
    // Any seed data that landed on a .0 is automatically inserted as .00 so 3 is inadequate
    [Column(TypeName = "decimal(4, 2)")]
    public decimal Weight { get; set; }

    public int Value { get; set; }
}
