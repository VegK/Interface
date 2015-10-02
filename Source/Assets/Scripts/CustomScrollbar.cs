using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
	public class CustomScrollbar : Scrollbar, IEndDragHandler
	{
		public event ScrollPointerHandler OnBeginScroll;
		public event ScrollPointerHandler OnEndScroll;

		public override void OnBeginDrag(PointerEventData eventData)
		{
			base.OnBeginDrag(eventData);
			if (OnBeginScroll != null)
				OnBeginScroll(this, eventData);
		}

		public virtual void OnEndDrag(PointerEventData eventData)
		{
			if (OnEndScroll != null)
				OnEndScroll(this, eventData);
		}

		public delegate void ScrollPointerHandler(object sender, PointerEventData eventData);
	}
}
