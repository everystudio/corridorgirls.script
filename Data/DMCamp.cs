using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DMCamp : DataManagerBase<DMCamp> {

	public static readonly string SS_ID = "1NVMQClQVzejSE3nE3qsgNA2wMSEdM2l892afiGYj83c";
	public static readonly string SS_TEST = "1bPRYINRf64UOumP6ExJGBb4pHN4qPlM-fUJk7VGAEpg";

	public CsvKvs gameData = new CsvKvs();

	public MasterStage masterStage = new MasterStage();

	public MasterChara masterChara = new MasterChara();
	public MasterCharaCard masterCharaCard = new MasterCharaCard();


	public MasterCard masterCard = new MasterCard();
	public MasterCardSymbol masterCardSymbol = new MasterCardSymbol();

	public MasterItem masterItem = new MasterItem();
	public MasterStageItem masterStageItem = new MasterStageItem();
	public MasterStageCard masterStageCard = new MasterStageCard();

	public MasterSkill masterSkill = new MasterSkill();


	public MasterCampItem masterCampItem = new MasterCampItem();
	public MasterCampShop masterCampShop = new MasterCampShop();


	public DataCard dataCard = new DataCard();
	public DataUnit dataUnitCamp = new DataUnit();
	public DataUnit dataUnitGame = new DataUnit();
	public DataItem dataItem = new DataItem();
	public DataSkill dataSkill = new DataSkill();
	public DataStage dataStage = new DataStage();

	public DataCampItem dataCampItem = new DataCampItem();

	[HideInInspector]
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
		GetAgingState();

		gameData.SetSaveFilename(Defines.FILENAME_GAMEDATA);
		if( false == gameData.Load())
		{
			// なんか初期化する必要あるなら
		}

		// master
		yield return StartCoroutine(masterChara.SpreadSheet(SS_ID, "chara", () => { }));
		yield return StartCoroutine(masterCharaCard.SpreadSheet(SS_ID, "chara_card", () => { }));

		yield return StartCoroutine(masterStage.SpreadSheet(SS_ID, "stage", () => { }));
		yield return StartCoroutine(masterCard.SpreadSheet(SS_ID, "card", () => { }));
		yield return StartCoroutine(masterCardSymbol.SpreadSheet(SS_ID, "card_symbol", () => { }));

		yield return StartCoroutine(masterItem.SpreadSheet(SS_ID, "item", () => { }));
		yield return StartCoroutine(masterStageItem.SpreadSheet(SS_ID, "stage_item", () => { }));
		yield return StartCoroutine(masterStageCard.SpreadSheet(SS_ID, "stage_card", () => { }));

		yield return StartCoroutine(masterSkill.SpreadSheet(SS_ID, "skill", () => { }));
		yield return StartCoroutine(masterCampItem.SpreadSheet(SS_ID, "campitem", () => { }));
		yield return StartCoroutine(masterCampShop.SpreadSheet(SS_ID, "campshop", () => { }));
		
		// data
		dataUnitCamp.SetSaveFilename(Defines.FILENAME_UNIT_CAMP);
		dataUnitGame.SetSaveFilename(Defines.FILENAME_UNIT_GAME);
		dataSkill.SetSaveFilename("camp_skill");

		if( false == dataUnitCamp.LoadMulti())
		{
			yield return StartCoroutine(dataUnitCamp.SpreadSheet(SS_TEST, "unit", () => { }));
		}
		if( false == dataSkill.LoadMulti())
		{
			yield return StartCoroutine(dataSkill.SpreadSheet(SS_TEST, "skill", () => { }));
		}
		yield return StartCoroutine(dataItem.SpreadSheet(SS_TEST, "item", () => { }));

		if( false == dataCampItem.LoadMulti())
		{
			yield return StartCoroutine(dataCampItem.SpreadSheet(SS_TEST, "campitem", () => { }));
		}

		dataStage.SetSaveFilename(Defines.FILENAME_DATA_STAGE);
		if( false == dataStage.LoadMulti())
		{
			// まだデータなし
		}

		int serial = 1;
		List<DataUnitParam> unit_param_list = dataUnitCamp.list.FindAll(p => p.unit == "chara" && p.position != "none");
		foreach (DataUnitParam unit in unit_param_list)
		{
			List<MasterCharaCardParam> card_list = masterCharaCard.list.FindAll(p => p.chara_id == unit.chara_id);
			foreach (MasterCharaCardParam c in card_list)
			{
				DataCardParam dc = new DataCardParam();

				dc.chara_id = c.chara_id;
				dc.card_id = c.card_id;
				dc.card_serial = serial;
				dc.status = (int)DataCard.STATUS.DECK;
				serial += 1;

				dataCard.list.Add(dc);
			}
		}


		Initialized = true;

	}
}
