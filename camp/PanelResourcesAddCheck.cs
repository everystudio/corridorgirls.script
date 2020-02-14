using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Advertisements;

public class PanelResourcesAddCheck : MonoBehaviour {
	public InfoHeaderCamp infoHeader;

	public GameObject m_goRootFood;
	public GameObject m_goRootMana;
	public GameObject m_goRootGem;

	public TextMeshProUGUI m_txtTitle;
	public TextMeshProUGUI m_txtMessage;

	public TextMeshProUGUI m_txtYesLabel;
	public TextMeshProUGUI m_txtNoLabel;

	public Button m_btnYes;
	public Button m_btnNo;

	public TextMeshProUGUI m_txtLabelFood;
	public TextMeshProUGUI m_txtLabelMana;
	public TextMeshProUGUI m_txtLabelGem;

	public void AddFood()
	{
		gameObject.SetActive(true);
		m_goRootFood.SetActive(true);
		m_goRootMana.SetActive(false);
		m_goRootGem.SetActive(true);

		m_txtMessage.text = "ジェムを消費して食料を増やします。よろしいですか？";

		int food = DMCamp.Instance.gameData.ReadInt(Defines.KeyFood);
		int gem = DMCamp.Instance.gameData.ReadInt(Defines.KeyGem);

		m_txtLabelFood.text = string.Format("{0} → {1}", food, food + Defines.BUY_FOOD_FROM_GEM);

		m_txtLabelGem.text = string.Format("{0} → {1}", gem, gem - Defines.BUY_FOOD_COST_GEM);
		if (Defines.BUY_FOOD_COST_GEM <= gem)
		{
			m_txtYesLabel.text = "OK";
			m_btnYes.interactable = true;
		}
		else
		{
			m_txtYesLabel.text = "<color=red>ジェム不足</color>";
			m_btnYes.interactable = false;
		}

		m_btnYes.onClick.RemoveAllListeners();
		m_btnNo.onClick.RemoveAllListeners();
		m_btnYes.onClick.AddListener(() =>
		{
			gameObject.SetActive(false);

			DMCamp.Instance.gameData.AddInt(Defines.KeyFood , Defines.BUY_FOOD_FROM_GEM);
			DMCamp.Instance.gameData.AddInt(Defines.KeyGem , -1 * Defines.BUY_FOOD_COST_GEM);

			infoHeader.AddFood(Defines.BUY_FOOD_FROM_GEM);
			infoHeader.AddGem(-1 * Defines.BUY_FOOD_COST_GEM);
		});
		m_btnNo.onClick.AddListener(() =>
		{
			gameObject.SetActive(false);
		});


	}
	public void AddMana()
	{
		gameObject.SetActive(true);
		m_goRootFood.SetActive(false);
		m_goRootMana.SetActive(true);
		m_goRootGem.SetActive(true);

		m_txtMessage.text = "ジェムを消費してマナを増やします。よろしいですか？";

		int mana = DMCamp.Instance.gameData.ReadInt(Defines.KeyMana);
		int gem = DMCamp.Instance.gameData.ReadInt(Defines.KeyGem);

		m_txtLabelMana.text = string.Format("{0} → {1}", mana, mana + Defines.BUY_MANA_FROM_GEM);

		m_txtLabelGem.text = string.Format("{0} → {1}", gem, gem - Defines.BUY_MANA_COST_GEM);
		if (Defines.BUY_MANA_COST_GEM <= gem)
		{
			m_txtYesLabel.text = "OK";
			m_btnYes.interactable = true;
		}
		else
		{
			m_txtYesLabel.text = "<color=red>ジェム不足</color>";
			m_btnYes.interactable = false;
		}

		m_btnYes.onClick.RemoveAllListeners();
		m_btnNo.onClick.RemoveAllListeners();
		m_btnYes.onClick.AddListener(() =>
		{
			gameObject.SetActive(false);

			DMCamp.Instance.gameData.AddInt(Defines.KeyMana, Defines.BUY_MANA_FROM_GEM);
			DMCamp.Instance.gameData.AddInt(Defines.KeyGem, -1 * Defines.BUY_MANA_COST_GEM);

			infoHeader.AddMana(Defines.BUY_MANA_FROM_GEM);
			infoHeader.AddGem(-1 * Defines.BUY_MANA_COST_GEM);

		});
		m_btnNo.onClick.AddListener(() =>
		{
			gameObject.SetActive(false);
		});



	}
	public void AddGem()
	{
		gameObject.SetActive(true);
		m_goRootFood.SetActive(false);
		m_goRootMana.SetActive(false);
		m_goRootGem.SetActive(true);

		m_txtMessage.text = "動画広告を視聴してジェムを獲得します。よろしいですか？";

		int gem = DMCamp.Instance.gameData.ReadInt(Defines.KeyGem);

		m_txtLabelGem.text = string.Format("{0} → {1}", gem, gem + Defines.ADD_GEM_FROM_UNITYADS);
		if (Advertisement.IsReady() )
		{
			m_txtYesLabel.text = "OK";
			m_btnYes.interactable = true;
		}
		else
		{
			m_txtYesLabel.text = "<color=red>現在視聴不可</color>";
			m_btnYes.interactable = false;
		}


		m_btnYes.onClick.RemoveAllListeners();
		m_btnNo.onClick.RemoveAllListeners();
		m_btnYes.onClick.AddListener(() =>
		{
			gameObject.SetActive(false);
			Advertisement.Show();
			DMCamp.Instance.gameData.AddInt(Defines.KeyGem, Defines.ADD_GEM_FROM_UNITYADS);
			infoHeader.AddGem(Defines.ADD_GEM_FROM_UNITYADS);

		});
		m_btnNo.onClick.AddListener(() =>
		{
			gameObject.SetActive(false);
		});


	}


}
