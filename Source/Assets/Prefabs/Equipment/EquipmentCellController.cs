using UnityEngine;
using System.Collections;

public class EquipmentCellController : MonoBehaviour
{
	#region Properties
	#region Public
	public GameObject BackgroundImage;
	#endregion
	#region Private
	private CellController _cellController;
	#endregion
	#endregion

	#region Methods
	#region Public

	#endregion
	#region Private
	private void Start()
	{
		_cellController = GetComponent<CellController>();
		if (_cellController == null)
			Debug.Log(string.Format("У ячейке \"{0}\" отсутствует компонент \"{1}\".", gameObject.name, typeof(CellController)));
		else
			_cellController.OnChangeItem += ChangeItem;
    }

	private void OnEnable()
	{
		if (_cellController != null)
			_cellController.OnChangeItem += ChangeItem;
	}

	private void OnDisable()
	{
		if (_cellController != null)
			_cellController.OnChangeItem -= ChangeItem;
	}

	private void ChangeItem(ItemController item)
	{
		BackgroundImage.SetActive(item == null);
    }
	#endregion
	#endregion
}