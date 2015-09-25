using System.Xml.Serialization;

public partial class Item
{
	/// <summary>
	/// Получить тип предмета.
	/// </summary>
	/// <returns>Тип предмета.</returns>
	public ItemType GetItemType()
	{
		return (ItemType)Type;
	}
}