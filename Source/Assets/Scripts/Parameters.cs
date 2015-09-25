using UnityEngine;
using System.Collections;
using System;

public class Parameters : MonoBehaviour
{
	public const string ITEMS_PATH = "Items\\";

	#region Properties
	#region Public
	public GameObject PrefabCell;
	public GameObject PrefabItem;

	public ToolTipController ToolTip;

	public static Parameters Instance
	{
		get
		{
			if (_instance == null)
				throw new Exception(string.Format("На сцене отсутствует объект с компонентом \"{0}\".", typeof(Parameters)));
			return _instance;
		}
	}
	#endregion
	#region Private
	private static Parameters _instance;
	#endregion
	#endregion

	#region Methods
	#region Public

	#endregion
	#region Private
	private void Awake()
	{
		_instance = this;

		var ctrlCell = PrefabCell.GetComponent<CellController>();
		if (ctrlCell == null)
			throw new Exception(string.Format("Отсутствует компонент \"{0}\" у префаба клетки.", typeof(CellController)));

		var ctrlItem = PrefabItem.GetComponent<ItemController>();
		if (ctrlItem == null)
			throw new Exception(string.Format("Отсутствует компонент \"{0}\" у префаба предмета.", typeof(ItemController)));

		ToolTip.gameObject.SetActive(false);
    }
	#endregion
	#endregion
}