using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : DataManagerBase<DataManager> {

	public static readonly string CONFIG_SS = "18bT3K48n7wOjPwQP-zn1QdUs_JoNKVKZpE6pb-I6PUQ";
	public static readonly string SS_QUEST = "1ls0u6TFbOisVM5nOw6cIP1Y4nK0mDtdgRPaOKmlzGvU";

	public static readonly string SS_ID = "1NVMQClQVzejSE3nE3qsgNA2wMSEdM2l892afiGYj83c";
	public static readonly string SS_TEST = "1bPRYINRf64UOumP6ExJGBb4pHN4qPlM-fUJk7VGAEpg";

	public TextAssetHolder data_holder;

	public MasterStage masterStage = new MasterStage();
	public MasterCorridor masterCorridor = new MasterCorridor();
	public MasterCorridorEvent masterCorridorEvent = new MasterCorridorEvent();

	public MasterCard masterCard = new MasterCard();
	public MasterCardSymbol masterCardSymbol = new MasterCardSymbol();

	public MasterCharaCard masterCharaCard = new MasterCharaCard();

	public MasterItem masterItem = new MasterItem();
	public MasterStageItem masterStageItem = new MasterStageItem();
	public MasterStageCard masterStageCard = new MasterStageCard();

	public MasterSkill masterSkill = new MasterSkill();

	public DataCorridor dataCorridor = new DataCorridor();
	public DataCard dataCard = new DataCard();

	public DataUnit dataUnit = new DataUnit();

	public bool Initialized = false;

	public override void Initialize()
	{
		base.Initialize();
		StartCoroutine(init_network());
	}


	private IEnumerator init_network()
	{
		yield return StartCoroutine(masterStage.SpreadSheet(SS_ID, "stage", () => { }));
		yield return StartCoroutine(masterCorridor.SpreadSheet(SS_ID, "corridor", () => { }));
		yield return StartCoroutine(masterCorridorEvent.SpreadSheet(SS_ID, "corridor_event" , ()=> { }));

		yield return StartCoroutine(masterCard.SpreadSheet(SS_ID, "card", () => {
			/*
			foreach( MasterCardParam card in masterCard.list)
			{
				Debug.Log(string.Format("{0}:{1}", card.card_id, card.label));
			}
			*/
		}));
		yield return StartCoroutine(masterCardSymbol.SpreadSheet(SS_ID, "card_symbol" , ()=> { }));

		yield return StartCoroutine(masterCharaCard.SpreadSheet(SS_ID, "chara_card", () => { }));

		yield return StartCoroutine(masterItem.SpreadSheet(SS_ID, "item" , ()=> { }));
		yield return StartCoroutine(masterStageItem.SpreadSheet(SS_ID, "stage_item" , ()=> { }));
		yield return StartCoroutine(masterStageCard.SpreadSheet(SS_ID, "stage_card" , ()=> { }));

		yield return StartCoroutine(masterSkill.SpreadSheet(SS_ID, "skill" , ()=> { }));

		/*
		foreach ( MasterItemParam item in masterItem.list)
		{
			Debug.Log(item.name);
		}
		*/

		yield return null;

		dataCard.SetSaveFilename("data_card");
		dataCard.LoadMulti("data_card");

		yield return StartCoroutine(dataUnit.SpreadSheet(SS_TEST, "unit", () => { }));

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
