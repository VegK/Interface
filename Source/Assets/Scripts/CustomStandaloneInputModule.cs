using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomStandaloneInputModule : StandaloneInputModule
{
	/// <summary>
	/// Получить начальный и конечный объект при перемещении.
	/// </summary>
	/// <typeparam name="T">Тип объекта.</typeparam>
	/// <param name="enter">Куда перемещаем. Объект который оказался под курсором после перемещения.</param>
	/// <param name="press">Откуда перемещаем. Объект который был под курсором в начале перемещения.</param>
	public void GetDropData<T>(out T enter, out T press)
	{
		enter = default(T);
		press = default(T);

		var mouse = GetMousePointerEventData();
		var state = mouse.GetButtonState(PointerEventData.InputButton.Left);
		var data = state.eventData.buttonData;

		var raycastResults = new List<RaycastResult>();
		RaycastResult firstHit;

		eventSystem.RaycastAll(data, raycastResults);
		firstHit = raycastResults.Find(r => r.gameObject.GetComponent<T>() != null);
		if (firstHit.gameObject != null)
			enter = firstHit.gameObject.GetComponent<T>();

		var pointer = new PointerEventData(eventSystem);
		pointer.position = data.pressPosition;
		eventSystem.RaycastAll(pointer, raycastResults);
		firstHit = raycastResults.Find(r => r.gameObject.GetComponent<T>() != null);
		if (firstHit.gameObject != null)
			press = firstHit.gameObject.GetComponent<T>();
	}
}