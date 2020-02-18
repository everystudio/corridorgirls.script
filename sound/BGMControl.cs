using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SoundControlを作って継承するほうが良さそう

public class BGMControl : Singleton<BGMControl> {

	public AudioSource audio_source;
	public List<AudioClip> audio_clip_list = new List<AudioClip>();

	public void Stop()
	{
		audio_source.Stop();
	}
	public void Pause()
	{
		audio_source.Pause();
	}
	public void Play()
	{
		audio_source.Play();
	}

	public void Play(string _strName)
	{
		bool is_playing = audio_source.clip.name == _strName;
		if( is_playing)
		{
			return;
		}

		AudioClip clip = audio_clip_list.Find(p => p.name == _strName);
		if (clip != null)
		{
			audio_source.clip = clip;
			audio_source.Play();
		}
		else
		{
			Debug.LogError(string.Format("notfound:{0}", _strName));
		}

	}
}
