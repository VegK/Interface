using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory
{
	public class InventoryController : BaseInventory
	{
		public const string FILENAME_SAVE = "inventory.xml";

		#region Properties
		#region Public
		public CustomScrollRect ScrollRect;
		#endregion
		#region Private

		#endregion
		#endregion

		#region Methods
		#region Public

		#endregion
		#region Private
		protected override void Start()
		{
			base.Start();
			LoadInventory();
		}
		private void OnEnable()
		{
			ScrollRect.OnBeginScroll += BeginScroll;
			ScrollRect.OnEndScroll += EndScroll;

			var scrollbar = ScrollRect.GetVerticalScrollbar() as CustomScrollbar;
			if (scrollbar != null)
			{
				scrollbar.OnBeginScroll += BeginScroll;
				scrollbar.OnEndScroll += EndScroll;
			}
		}
		private void OnDisable()
		{
			ScrollRect.OnBeginScroll -= BeginScroll;
			ScrollRect.OnEndScroll -= EndScroll;

			var scrollbar = ScrollRect.GetVerticalScrollbar() as CustomScrollbar;
			if (scrollbar != null)
			{
				scrollbar.OnBeginScroll -= BeginScroll;
				scrollbar.OnEndScroll -= EndScroll;
			}
		}
		private void OnApplicationQuit()
		{
			var status = new InventoryStatus(Cells);
			status.Save();
		}
		/// <summary>
		/// Загрузить инвентарь.
		/// </summary>
		private void LoadInventory()
		{
			InventoryStatus status;
			if (!InventoryStatus.Load(out status))
				return;

			foreach (CellInfo info in status.Cells)
			{
				var cell = Cells.Find(c => c.Index == info.IndexCell);
				if (cell == null)
				{
					var str = "<color=red>Ошибка инвентаря</color>: не удалось найти ячейку с индексом <b>{0}</b>.";
					LogController.Instance.AddString(string.Format(str, info.IndexCell));
					continue;
				}

				var baseItem = LibraryController.Instance.GetItem(info.IndexItem);
				if (baseItem == null)
				{
					var str = "<color=red>Ошибка инвентаря</color>: не удалось найти предмет с индексом <b>{0}</b>.";
					LogController.Instance.AddString(string.Format(str, info.IndexItem));
					continue;
				}

				var item = CreateItem(baseItem);
				SetItemInCell(item, cell);
				info.MoveMoreParams(cell);
			}
		}
		private void BeginScroll(object sender, PointerEventData eventData)
		{
			Parameters.Instance.ToolTip.Hide();
			Parameters.Instance.ToolTip.FixedHide = true;
		}
		private void EndScroll(object sender, PointerEventData eventData)
		{
			Parameters.Instance.ToolTip.FixedHide = false;
		}
		#endregion
		#endregion
	}
}