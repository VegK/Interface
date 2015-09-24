using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ItemController : MonoBehaviour
{
	#region Properties
	#region Public
	public Image ImageItem;
	/// <summary>
	/// Тип предмета.
	/// </summary>
	public ItemType Type { get; set; }
	/// <summary>
	/// Название предмета.
	/// </summary>
	public string Title { get; set; }
	/// <summary>
	/// Описание предмета.
	/// </summary>
	public string Description { get; set; }
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
	#endregion
	#region Private
	private GameObject _cloneMove;
	private bool _fixedCell;
	private bool _produceClone;
	#endregion
	#endregion

	#region Methods
	#region Public
	public void OnPointerEnter(BaseEventData data)
	{
		
	}
	public void OnPointerExit(BaseEventData data)
	{
		
	}
	public void OnPointerDown(BaseEventData data)
	{
		if (FixedCell && !ProduceClone)
			return;

		_cloneMove = Instantiate(ImageItem.gameObject);
		_cloneMove.SetActive(true);

		_cloneMove.transform.SetParent(GetComponentInParent<Canvas>().transform);
		_cloneMove.transform.position = transform.position;

		ImageItem.gameObject.SetActive(ProduceClone);
	}
	public void OnPointerUp(BaseEventData data)
	{
		if (FixedCell && !ProduceClone)
			return;

		_cloneMove.SetActive(false);
        ImageItem.gameObject.SetActive(true);

		var inputModule = EventSystem.current.currentInputModule as CustomStandaloneInputModule;
		CellController enter, press;

		if (inputModule != null)
		{
			inputModule.GetDropData(out enter, out press);
			if (enter != null && press != null)
				if (enter.Item == null || !enter.Item.FixedCell)
				{
					// Если предмет может создавать свои копии, то создаём
					// в противном случаи меняем местами предметы.
					if (ProduceClone)
					{
						if (enter.Item == null)
							LibraryController.Instance.CreateCloneItem(enter, press.Item);
					}
					else
						LibraryController.Instance.SwapItemsInCell(press, enter);
				}
		}

		Destroy(_cloneMove);
	}
	public void OnDrag(BaseEventData data)
	{
		if (FixedCell && !ProduceClone)
			return;

		var pointer = data as PointerEventData;
		if (pointer != null)
			_cloneMove.transform.position = pointer.position;
	}
	#endregion
	#region Private

	#endregion
	#endregion
}