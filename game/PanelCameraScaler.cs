using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelCameraScaler : MonoBehaviour {

	[SerializeField]
	private Slider slider;

	public Button m_btnScale;

	[SerializeField]
	private Camera target_camera;

	private const float CameraSizeMin = 3.0f;
	private const float CameraSizeMax = 10.0f;

	private bool push_toggle;
	public void OnPush()
	{
		if(push_toggle == false)
		{
			iTween.ScaleTo(slider.gameObject,
				iTween.Hash(
					"x", 1,
					"y", 1,
					"z", 1,
					"time", 0.3f,
					"isLocal", true
					)
				);

		}
		else
		{
			iTween.ScaleTo(slider.gameObject,
				iTween.Hash(
					"x", 0,
					"y", 0,
					"z", 0,
					"time", 0.3f,
					"isLocal", true
					)
				);
		}
		push_toggle = !push_toggle;
	}


	public float volume
	{
		set
		{
			//Debug.Log(target_camera.orthographicSize);

			float scale = Mathf.Lerp(CameraSizeMin, CameraSizeMax, value);
			//mixer.SetFloat(group_name, Mathf.Lerp(Defines.SOUND_VOLME_MIN, Defines.SOUND_VOLUME_MAX, value));
			target_camera.orthographicSize = scale;
		}
	}

	void Start()
	{
		push_toggle = false;
		slider.transform.localScale = Vector3.zero;
	}

	void OnEnable()
	{
		float temp = (target_camera.orthographicSize - CameraSizeMin) / (CameraSizeMax - CameraSizeMin);
		slider.value = temp;
		//slider.value = DataManager.Instance.user_data.ReadFloat(group_name);
	}
}
