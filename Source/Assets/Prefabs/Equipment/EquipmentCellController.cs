using UnityEngine;
using System.Collections;
using System;

namespace Equipment
{
	public class EquipmentCellController : CellController
	{
		#region Properties
		#region Public
		public ItemType TypeItem;
		public GameObject BackgroundImage;
		public GameObject AvailableImage;
		#endregion
		#region Private

		#endregion
		#endregion

		#region Methods
		#region Public
		/// <summary>
		/// Показать/скрыть выделение ячейки.
		/// </summary>
		/// <param name="value">Показать/скрыть.</param>
		public void SetAvailable(bool value)
		{
			AvailableImage.SetActive(value);
        }
		/// <summary>
		/// Показать/скрыть изображение заднего фона ячейки.
		/// </summary>
		/// <param name="value">Показать/скрыть.</param>
		public void SetBackground(bool value)
		{
			BackgroundImage.SetActive(value);
		}
		/// <summary>
		/// Проверить возможно ли положить предмет в ячейку.
		/// </summary>
		/// <param name="item">Предмет.</param>
		/// <returns>Можно положить.</returns>
		public override bool CheckSetItem(ItemController item)
		{
			if (item != null)
			{
				if (item.BaseItem == null)
					throw new NullReferenceException("Отсутствует ссылка на базовый предмет.");
				if (item.BaseItem.GetItemType() != TypeItem)
					return false;
			}
			return base.CheckSetItem(item);
		}
		/// <summary>
		/// Положить предмет в ячейку с определением соответствия типа ячейки и предмета.
		/// </summary>
		/// <param name="item">Предмет.</param>
		/// <returns>В случаи удачного действия возвращает true.</returns>
		public override bool SetItem(ItemController item)
		{
			if (!CheckSetItem(item))
				return false;
            return base.SetItem(item);
		}
		#endregion
		#region Private
		private void Awake()
		{
			AvailableImage.SetActive(false);
        }

		private void OnEnable()
		{
			OnChangeItem += ChangeItem;
		}

		private void OnDisable()
		{
			OnChangeItem -= ChangeItem;
		}

		private void ChangeItem(ItemController item)
		{
			BackgroundImage.SetActive(item == null);
		}
		#endregion
		#endregion
	}
}