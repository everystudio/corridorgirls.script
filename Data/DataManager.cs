using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : DataManagerBase<DataManager> {

	public static readonly string CONFIG_SS = "18bT3K48n7wOjPwQP-zn1QdUs_JoNKVKZpE6pb-I6PUQ";
	public static readonly string SS_QUEST = "1ls0u6TFbOisVM5nOw6cIP1Y4nK0mDtdgRPaOKmlzGvU";

	public TextAssetHolder data_holder;

	public MasterStage masterStage = new MasterStage();
	public MasterCorridor masterCorridor = new MasterCorridor();

	public MasterGimic masterGimic = new MasterGimic();
	public MasterCard masterCard = new MasterCard();


	public DataCorridor dataCorridor = new DataCorridor();
	public DataCard dataCard = new DataCard();

	public bool Initialized = false;

	public override void Initialize()
	{
		base.Initialize();
		masterGimic.OnRecieveData.AddListener(OnRecievedMasterDungeonStep);
		masterGimic.SpreadSheet(CONFIG_SS, "gimic");

		masterCard.OnRecieveData.AddListener(OnRecievedMasterCard);
		masterCard.SpreadSheet(CONFIG_SS, "card");

		masterStage.Load(data_holder.Get("master_stage"));
		masterCorridor.Load(data_holder.Get("master_corridor"));

		dataCard.SetSaveFilename("data_card");
		dataCard.LoadMulti("data_card");

		Initialized = true;

	}

	private void OnRecievedMasterDungeonStep(List<MasterGimicParam> arg0)
	{
	}

	private void OnRecievedMasterCard(List<MasterCardParam> arg0)
	{
		foreach(MasterCardParam param in arg0)
		{
			//Debug.Log(param.card_id);
		}
	}


}
