using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeMP : MonoBehaviour {
	public EnergyBar energy_bar;

	public void Setup()
	{
		DataManager.Instance.dataQuest.AddListener("mp_max", (string _strMpMax) =>
		{
			energy_bar.SetValueMax(int.Parse(_strMpMax));
		});

		DataManager.Instance.dataQuest.AddListener("mp", (string _strMp) =>
		{
			energy_bar.SetValueCurrent(int.Parse(_strMp));
		});

	}
}
