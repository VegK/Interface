using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Shine : MonoBehaviour
{
	#region Properties
	#region Public
	public float FrequencyBegin = 1f;
	public float FrequencyEnd = 3f;
	public Color ColorShine = Color.red;
	public float SpeedShine = 1f;

	[HideInInspector]
	public float Frequency = 0;
	#endregion
	#region Private
	private Image _image;
	private Color _baseColor;
	private float _timeStartShine;
	private Color _startColor;
	private Color _finishColor;
	#endregion
	#endregion

	#region Methods
	#region Public

	#endregion
	#region Private
	private void OnEnable()
	{
		if (Frequency == 0)
			Frequency = UnityEngine.Random.Range(FrequencyBegin, FrequencyEnd);

		_image = GetComponent<Image>();
		if (_image != null)
		{
			_baseColor = _image.color;
			StartCoroutine(ShineCoroutine());
		}
	}
	private void OnDisable()
	{
		StopAllCoroutines();
		_image.color = _baseColor;
	}
	private IEnumerator ShineCoroutine()
	{
		for (;;)
		{
			if (_image.color == _baseColor)
			{
				yield return new WaitForSeconds(Frequency);
				_timeStartShine = Time.time - Time.deltaTime;
				_startColor = _baseColor;
				_finishColor = ColorShine;
			}
			else if (_image.color == ColorShine)
			{
				_timeStartShine = Time.time - Time.deltaTime;
				_startColor = ColorShine;
				_finishColor = _baseColor;
			}

			var t = (Time.time - _timeStartShine) * SpeedShine;
			_image.color = Color.Lerp(_startColor, _finishColor, t);
			yield return null;
		}
	}
	#endregion
	#endregion
}