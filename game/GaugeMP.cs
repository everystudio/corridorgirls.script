using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeMP : MonoBehaviour {
	public EnergyBar energy_bar;

	public void Setup()
	{
		DataManagerGame.Instance.gameData.AddListener(Defines.KEY_MP_MAX, (string _strMpMax) =>
		{
			energy_bar.SetValueMax(int.Parse(_strMpMax));
		});

		DataManagerGame.Instance.gameData.AddListener(Defines.KEY_MP, (string _strMp) =>
		{
			energy_bar.SetValueCurrent(int.Parse(_strMp));
		});
	}
	
}
