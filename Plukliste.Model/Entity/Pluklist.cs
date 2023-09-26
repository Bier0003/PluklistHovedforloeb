namespace Plukliste.Model.Entity;

public class Pluklist
{
    public int Id;
    public string? Name;
    public string? Forsendelse;
    public string? Adresse;
    public List<Item> Lines = new List<Item>();
    public void AddItem(Item item) { Lines.Add(item); }
}
