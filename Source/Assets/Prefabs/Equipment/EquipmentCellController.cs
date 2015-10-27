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
			if (AvailableImage == null)
				return;
			AvailableImage.SetActive(value);
		}
		/// <summary>
		/// Показать/скрыть изображение заднего фона ячейки.
		/// </summary>
		/// <param name="value">Показать/скрыть.</param>
		public void SetBackground(bool value)
		{
			if (BackgroundImage == null)
				return;
			BackgroundImage.SetActive(value);
		}
		/// <summary>
		/// Проверить возможно ли положить предмет в ячейку.
		/// </summary>
		/// <param name="item">Предмет.</param>
		/// <returns>Можно положить.</returns>
		public override bool CheckPutItem(ItemController item)
		{
			if (item != null)
			{
				if (item.BaseItem == null)
					throw new NullReferenceException("Отсутствует ссылка на базовый предмет.");
				if (item.BaseItem.GetItemType() != TypeItem)
					return false;
			}
			return base.CheckPutItem(item);
		}
		/// <summary>
		/// Положить предмет в ячейку с определением соответствия типа ячейки и предмета.
		/// </summary>
		/// <param name="item">Предмет.</param>
		/// <returns>В случаи удачного действия возвращает true.</returns>
		public override bool PutItem(ItemController item)
		{
			if (!CheckPutItem(item))
				return false;
			return base.PutItem(item);
		}
		#endregion
		#region Private
		protected override void Awake()
		{
			base.Awake();
			if (AvailableImage == null)
				return;
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