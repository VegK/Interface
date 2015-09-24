using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ItemController : MonoBehaviour
{
	#region Properties
	#region Public
	public Image ImageItem;
	#endregion
	#region Private
	private GameObject _cloneMove;
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
		_cloneMove = Instantiate(ImageItem.gameObject);
		_cloneMove.SetActive(true);

		_cloneMove.transform.SetParent(GetComponentInParent<Canvas>().transform);
		_cloneMove.transform.position = transform.position;

		ImageItem.gameObject.SetActive(false);
	}
	public void OnPointerUp(BaseEventData data)
	{
		_cloneMove.SetActive(false);
        ImageItem.gameObject.SetActive(true);

		var inputModule = EventSystem.current.currentInputModule as CustomStandaloneInputModule;
		if (inputModule != null)
		{
			CellController enter, press;
			inputModule.GetDropData(out enter, out press);
			LibraryController.Instance.SwapItemsInCell(enter, press);
		}

		Destroy(_cloneMove);
	}
	public void OnDrag(BaseEventData data)
	{
		var pointer = data as PointerEventData;
		if (pointer != null)
			_cloneMove.transform.position = pointer.position;
	}
	#endregion
	#region Private

	#endregion
	#endregion
}