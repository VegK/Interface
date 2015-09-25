using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToolTipController : MonoBehaviour
{
	#region Properties
	#region Public
	public Text Title;
	public Text Description;

	/// <summary>
	/// Если true, то ToolTip не будет показыватся.
	/// </summary>
	public bool FixedHide { get; set; }
	#endregion
	#region Private
	private Vector2 _sizeToolTip;
	#endregion
	#endregion

	#region Methods
	#region Public
	public void OnPointerEnter(BaseEventData data)
	{
		Hide();
	}
	/// <summary>
	/// Показать панель описания предмета.
	/// </summary>
	/// <param name="title">Название предмета.</param>
	/// <param name="description">Описание предмета.</param>
	public void Show(Vector2 position, string title, string description)
	{
		if (FixedHide)
			return;
		CancelInvoke("HideToolTip");

		position.x += _sizeToolTip.x / 2;
		position.y -= _sizeToolTip.y / 2;
		transform.position = position;

		Title.text = title;
		Description.text = description;
		gameObject.SetActive(true);
	}
	/// <summary>
	/// Скрыть панель описания предмета.
	/// </summary>
	public void Hide()
	{
		Hide(0);
	}
	/// <summary>
	/// Скрыть панель описания предмета.
	/// </summary>
	/// <param name="time">Количество секунд через которое ToolTip будет скрыт.</param>
	public void Hide(float time)
	{
		Invoke("HideToolTip", time);
	}
	#endregion
	#region Private
	private void Awake()
	{
		_sizeToolTip = gameObject.GetComponent<RectTransform>().sizeDelta;
    }
	private void HideToolTip()
	{
		gameObject.SetActive(false);
	}
	#endregion
	#endregion
}