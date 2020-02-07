﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManagerGame : DataManagerBase<DataManagerGame> {

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

	public MasterChara masterChara = new MasterChara();
	public MasterCharaCard masterCharaCard = new MasterCharaCard();

	public MasterItem masterItem = new MasterItem();
	public MasterStageEvent masterStageEvent = new MasterStageEvent();
	public MasterStageItem masterStageItem = new MasterStageItem();
	public MasterStageCard masterStageCard = new MasterStageCard();
	public MasterStageMission masterStageMission = new MasterStageMission();
	public MasterStageEnemy masterStageEnemy = new MasterStageEnemy();

	public MasterSkill masterSkill = new MasterSkill();
	public MasterSkillEffect masterSkillEffect = new MasterSkillEffect();

	public MasterMission masterMission = new MasterMission();
	public MasterMissionDetail masterMissionDetail = new MasterMissionDetail();

	public DataKvs dataQuest = new DataKvs();
	public DataCorridor dataCorridor = new DataCorridor();
	public DataCard dataCard = new DataCard();
	public DataUnit dataUnit = new DataUnit();
	public DataItem dataItem = new DataItem();
	public DataSkill dataSkill = new DataSkill();

	public bool Initialized = false;

	public override void Initialize()
	{
		base.Initialize();
		StartCoroutine(init_network());
	}

	private bool m_bIsAging;
	public bool IsAging
	{
		get
		{
			return m_bIsAging;
		}
	}

	public bool GetAgingState()
	{
		m_bIsAging = config.HasKey(Defines.KEY_AGING);
		return IsAging;
	}


	private IEnumerator init_network()
	{
		Debug.Log(config.ReadInt("stage_id"));
		GetAgingState();

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

		yield return StartCoroutine(masterChara.SpreadSheet(SS_ID, "chara", () => { }));
		yield return StartCoroutine(masterCharaCard.SpreadSheet(SS_ID, "chara_card", () => { }));

		yield return StartCoroutine(masterItem.SpreadSheet(SS_ID, "item" , ()=> { }));
		yield return StartCoroutine(masterStageEvent.SpreadSheet(SS_ID, "stage_event" , ()=> { }));
		yield return StartCoroutine(masterStageItem.SpreadSheet(SS_ID, "stage_item" , ()=> { }));
		yield return StartCoroutine(masterStageCard.SpreadSheet(SS_ID, "stage_card" , ()=> { }));
		yield return StartCoroutine(masterStageMission.SpreadSheet(SS_ID, "stage_mission" , ()=> { }));
		yield return StartCoroutine(masterStageEnemy.SpreadSheet(SS_ID, "stage_enemy" , ()=> { }));

		yield return StartCoroutine(masterSkill.SpreadSheet(SS_ID, "skill" , ()=> { }));
		yield return StartCoroutine(masterSkillEffect.SpreadSheet(SS_ID, "skill_effect" , ()=> { }));

		yield return StartCoroutine(masterMission.SpreadSheet(SS_ID, "mission" , ()=> { }));
		yield return StartCoroutine(masterMissionDetail.SpreadSheet(SS_ID, "mission_detail" , ()=> { }));

		/*
		foreach ( MasterItemParam item in masterItem.list)
		{
			Debug.Log(item.name);
		}
		*/

		yield return null;

		//dataCard.SetSaveFilename("data_card");
		//dataCard.LoadMulti("data_card");
		dataUnit.SetSaveFilename(Defines.FILENAME_UNIT_GAME);
		dataSkill.SetSaveFilename("camp_skill");

		if (false == dataUnit.LoadMulti())
		{
			yield return StartCoroutine(dataUnit.SpreadSheet(SS_TEST, "unit", () => { }));
		}
		if (false == dataSkill.LoadMulti())
		{
			yield return StartCoroutine(dataSkill.SpreadSheet(SS_TEST, "skill", () => { }));
		}
		yield return StartCoroutine(dataQuest.SpreadSheet(SS_TEST, "quest", () => { }));
		yield return StartCoroutine(dataItem.SpreadSheet(SS_TEST, "item", () => { }));



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

	public int GetMp()
	{
		return DataManagerGame.Instance.dataQuest.ReadInt(Defines.KEY_MP);
	}

	public void MpHeal(int _iHeal)
	{
		int mp_max = DataManagerGame.Instance.dataQuest.ReadInt(Defines.KEY_MP_MAX);
		int mp_current = DataManagerGame.Instance.dataQuest.ReadInt(Defines.KEY_MP);

		if (mp_max < mp_current + _iHeal)
		{
			DataManagerGame.Instance.dataQuest.WriteInt(Defines.KEY_MP, mp_max);
		}
		else {
			DataManagerGame.Instance.dataQuest.AddInt(Defines.KEY_MP, _iHeal);
		}

	}


}
