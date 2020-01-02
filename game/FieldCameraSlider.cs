using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldCameraSlider : MonoBehaviour {
	[SerializeField]
	private UnityEngine.UI.Slider slider;

	[SerializeField]
	private Camera target_camera;

	public string group_name;

	public float volume
	{
		set
		{
			Debug.Log(target_camera.orthographicSize);
			//mixer.SetFloat(group_name, Mathf.Lerp(Defines.SOUND_VOLME_MIN, Defines.SOUND_VOLUME_MAX, value));
		}
	}

	void OnEnable()
	{
		//slider.value = DataManager.Instance.user_data.ReadFloat(group_name);
	}
}
