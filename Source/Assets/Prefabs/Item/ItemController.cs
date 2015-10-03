using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
	#region Properties
	#region Public
	public Image ImageItem;

	/// <summary>
	/// Базовый предмет из библиотеки.
	/// </summary>
	public Item BaseItem { get; set; }
	/// <summary>
	/// Предмет не может перемещатся из своей ячейки.
	/// </summary>
	public bool FixedCell
	{
		get
		{
			return _fixedCell;
		}
		set
		{
			if (!value)
				_produceClone = false;
			_fixedCell = value;
		}
	}
	/// <summary>
	/// При перемещении предмета создаётся клон.
	/// </summary>
	public bool ProduceClone
	{
		get
		{
			return _produceClone;
        }
		set
		{
			if (value)
				_fixedCell = true;
			_produceClone = value;
        }
	}
	/// <summary>
	/// Уровень модификации предмета.
	/// </summary>
	public int Modification { get; set; }
	/// <summary>
	/// Редкость предмета.
	/// </summary>
	public Rarity RarityItem
	{
		get
		{
			return _rarityItem;
        }
		set
		{
			_rarityItem = value;
			ActivateShine();
        }
	}
	#endregion
	#region Private
	private GameObject _cloneMove;
	private bool _fixedCell;
	private bool _produceClone;
	private Vector2 _size;
	private CellController _prevSelectedCell;

	private Rarity _rarityItem;
	#endregion
	#endregion

	#region Methods
	#region Public
	public void OnPointerEnter(BaseEventData data)
	{
		if (BaseItem != null)
		{
			var pos = transform.position;
			var itemName = BaseItem.Name;

			// Добавляем редкость предмета.
			var itemRarity = string.Empty;
			var rgb = string.Empty;
			var rarityColor = Parameters.Instance.RarityColor;
			switch (RarityItem)
			{
				case Rarity.Rare:
					rgb = rarityColor.Rare.ToHexStringRGB();
					itemRarity = "<color=#" + rgb + ">Редкий</color> ";
					break;
				case Rarity.Epic:
					rgb = rarityColor.Epic.ToHexStringRGB();
					itemRarity = "<color=#" + rgb + ">Эпичный</color> ";
					break;
				case Rarity.Legendary:
					rgb = rarityColor.Legendary.ToHexStringRGB();
					itemRarity = "<color=#" + rgb + ">Легендарный</color> ";
					break;
			}
			if (!string.IsNullOrEmpty(itemRarity))
				itemName = itemRarity + itemName;

			// Добавляем уровень модификации.
			if (Modification > 0)
				itemName = "<color=red>+" + Modification + "</color> " + itemName;

			Parameters.Instance.ToolTip.Show(pos, _size, itemName, BaseItem.Description);
		}
	}
	public void OnPointerExit(BaseEventData data)
	{
		Parameters.Instance.ToolTip.Hide(1);
	}
	public void OnPointerDown(BaseEventData data)
	{
		var pointer = data as PointerEventData;
		if (pointer != null && pointer.button != PointerEventData.InputButton.Left)
			return;
		if (FixedCell && !ProduceClone)
			return;

		Parameters.Instance.ToolTip.Hide();
		Parameters.Instance.ToolTip.FixedHide = true;

		_cloneMove = Instantiate(ImageItem.gameObject);
		_cloneMove.transform.SetParent(Parameters.Instance.MainCanvas.transform);
		_cloneMove.transform.position = transform.position;

		ImageItem.gameObject.SetActive(ProduceClone);

		// Выделяем доступные ячейки инвентаря.
		EquipmentController.Instance.ResetAvailableCells();
		EquipmentController.Instance.EnableAvailableCells(BaseItem.GetItemType(), ProduceClone, this);

		// Показываем задний фон ячейки инвентаря.
		var inputModule = EventSystem.current.currentInputModule as CustomStandaloneInputModule;
		CellController enter, press;
		if (inputModule != null)
		{
			inputModule.GetDropData(out enter, out press);
			var cell = press as Equipment.EquipmentCellController;
			if (cell != null)
				cell.SetBackground(true);
        }
	}
	public void OnPointerUp(BaseEventData data)
	{
		EquipmentController.Instance.ResetAvailableCells();
		if (_prevSelectedCell != null)
			_prevSelectedCell.SetSelected(false);

		var pointer = data as PointerEventData;
		if (pointer != null && pointer.button != PointerEventData.InputButton.Left)
			return;
		if (FixedCell && !ProduceClone)
			return;

		Parameters.Instance.ToolTip.FixedHide = false;
		_cloneMove.SetActive(false);
        ImageItem.gameObject.SetActive(true);

		var inputModule = EventSystem.current.currentInputModule as CustomStandaloneInputModule;
		CellController enter, press;
		if (inputModule != null)
		{
			inputModule.GetDropData(out enter, out press);
			if (enter != null && press != null)
			{
				enter.SetSelected(false);
				press.SetSelected(false);
				if (enter.Item == null || !enter.Item.FixedCell)
				{
					switch (enter.Type)
					{
						case CellType.Recycle:
							if (!press.Item.FixedCell)
								BaseInventory.RecycleItem(press);
							break;
						case CellType.Modification:
							BaseInventory.ModificationItem(this);
							break;
						default:
							// Если предмет может создавать свои копии, то создаём
							// в противном случаи меняем местами предметы.
							if (ProduceClone)
							{
								if (enter.Item == null)
									BaseInventory.CreateCloneItem(enter, press.Item);
							}
							else
								BaseInventory.SwapItemsInCell(press, enter);
							break;
					}
				}
			}
		}

		Destroy(_cloneMove);
		_cloneMove = null;
    }
	public void OnDrag(BaseEventData data)
	{
		if (FixedCell && !ProduceClone)
			return;

		var pointer = data as PointerEventData;
		if (pointer != null && pointer.button == PointerEventData.InputButton.Left)
		{
			_cloneMove.transform.position = pointer.position;


			// Выделяем ячейку если предмет можно переместить.
			var inputModule = EventSystem.current.currentInputModule as CustomStandaloneInputModule;
			CellController enter, press;
			if (inputModule != null)
			{
				inputModule.GetDropData(out enter, out press);

				if (enter == null && _prevSelectedCell != null)
					_prevSelectedCell.SetSelected(false);
				if (enter != null)
				{
					var selected = enter.CheckPutItem(press.Item);
					if (ProduceClone)
						selected &= (enter.Item == null);
					else
					{
						if (press != null)
							selected &= press.CheckPutItem(enter.Item);
					}
					enter.SetSelected(selected);
					if (enter != _prevSelectedCell)
					{
						if (_prevSelectedCell != null)
							_prevSelectedCell.SetSelected(false);
						_prevSelectedCell = enter;
					}
				}
			}
		}
	}
	#endregion
	#region Private
	private void Start()
	{
		_size = GetComponent<RectTransform>().sizeDelta;
	}
	/// <summary>
	/// Активировать блеск предмета согласно его редкости.
	/// </summary>
	private void ActivateShine()
	{
		var shine = ImageItem.GetComponent<Shine>();
		if (shine == null)
			return;

		var rarityColor = Parameters.Instance.RarityColor;
		switch (RarityItem)
		{
			default:
			case Rarity.Normal:
				break;
			case Rarity.Epic:
				shine.ColorShine = rarityColor.Epic;
				break;
			case Rarity.Rare:
				shine.ColorShine = rarityColor.Rare;
				break;
			case Rarity.Legendary:
				shine.ColorShine = rarityColor.Legendary;
				break;
		}
		shine.enabled = (RarityItem != Rarity.Normal);
	}
	#endregion
	#endregion
}