using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundVolumeSlider : MonoBehaviour {


	[SerializeField]
	private UnityEngine.Audio.AudioMixer mixer;
	[SerializeField]
	private UnityEngine.UI.Slider slider;

	public string group_name;

	public bool is_camp;

	private string key_name
	{
		get
		{
			string key = "";
			if (group_name == "SE")
			{
				key = Defines.KEY_SOUNDVOLUME_SE;
			}
			else if (group_name == "BGM")
			{
				key = Defines.KEY_SOUNDVOLUME_BGM;
			}
			return key;
		}
	}

	public float volume
	{
		set
		{
			mixer.SetFloat(group_name, Mathf.Lerp(Defines.SOUND_VOLME_MIN, Defines.SOUND_VOLUME_MAX, value));

			if (is_camp)
			{
				DMCamp.Instance.user_data.Write(key_name, value.ToString());
			}
			else
			{
				DataManagerGame.Instance.user_data.Write(key_name, value.ToString());
			}
		}
	}
	
	void OnEnable()
	{
		if (is_camp)
		{
			slider.value = DMCamp.Instance.user_data.ReadFloat(key_name);
			Debug.Log(slider.value);
		}
		else {
			slider.value = DataManagerGame.Instance.user_data.ReadFloat(key_name);
		}
	}
}
