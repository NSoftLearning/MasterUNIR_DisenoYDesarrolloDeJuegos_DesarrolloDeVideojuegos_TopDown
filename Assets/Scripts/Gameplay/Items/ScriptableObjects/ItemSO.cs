using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Content/Items/NewItem")]
public class ItemSO : ScriptableObject
{
    public Sprite ItemIcon;
    public string itemName;
    public int price;

    public void UseItem() { }

}
