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
	#endregion
	#region Private
	private GameObject _cloneMove;
	private bool _fixedCell;
	private bool _produceClone;
	private Vector2 _size;
	#endregion
	#endregion

	#region Methods
	#region Public
	public void OnPointerEnter(BaseEventData data)
	{
		if (BaseItem != null)
		{
			var pos = transform.position;
			Parameters.Instance.ToolTip.Show(pos, _size, BaseItem.Name, BaseItem.Description);
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
		_cloneMove.SetActive(true);

		_cloneMove.transform.SetParent(GetComponentInParent<Canvas>().transform);
		_cloneMove.transform.position = transform.position;

		ImageItem.gameObject.SetActive(ProduceClone);
	}
	public void OnPointerUp(BaseEventData data)
	{
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
				if (enter.Item == null || !enter.Item.FixedCell)
				{
					if (enter.Type == CellType.Recycle)
					{
						if (!press.Item.FixedCell)
							BaseInventory.RecycleItem(press);
                    }
					else
						// Если предмет может создавать свои копии, то создаём
						// в противном случаи меняем местами предметы.
						if (ProduceClone)
						{
							if (enter.Item == null)
								BaseInventory.CreateCloneItem(enter, press.Item);
						}
						else
							BaseInventory.SwapItemsInCell(press, enter);
				}
		}

		Destroy(_cloneMove);
	}
	public void OnDrag(BaseEventData data)
	{
		if (FixedCell && !ProduceClone)
			return;

		var pointer = data as PointerEventData;
		if (pointer != null && pointer.button == PointerEventData.InputButton.Left)
			_cloneMove.transform.position = pointer.position;
	}
	#endregion
	#region Private
	private void Start()
	{
		_size = GetComponent<RectTransform>().sizeDelta;
	}
	#endregion
	#endregion
}