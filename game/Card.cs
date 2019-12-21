using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour {

	[SerializeField]
	private Image m_imgFrame;
	[SerializeField]
	private TextMeshProUGUI m_txtType;
	[SerializeField]
	private TextMeshProUGUI m_txtPower;
	[SerializeField]
	private Image m_imgTypeIcon;
	public Animator m_animator;

	public GameObject m_goSymbolLine1;
	public GameObject m_goSymbolLine2;
	public List<Image> symbol_list;

	public Button m_btn;
	public UnityEventInt OnClickCard = new UnityEventInt();

	public DataCardParam data_card;

	public List<MasterCardSymbolParam> card_symbol_list = new List<MasterCardSymbolParam>();

	public void Initialize(MasterCardParam _master)
	{
		if( data_card == null)
		{
			data_card = new DataCardParam();
			data_card.master = _master;
		}

		card_symbol_list.Clear();

		for (int i = 0; i < symbol_list.Count; i++)
		{
			//Debug.Log(_card.card_id);
			//Debug.Log(_card.master);
			int symbol_id = _master.GetSymbolId(i);

			if (0 < symbol_id)
			{
				MasterCardSymbolParam symbol = DataManager.Instance.masterCardSymbol.list.Find(p => p.card_symbol_id == symbol_id);
				symbol_list[i].gameObject.SetActive(true);
				symbol_list[i].sprite = SpriteManager.Instance.Get(symbol.sprite_name);

				card_symbol_list.Add(symbol);
			}
			else
			{
				symbol_list[i].gameObject.SetActive(false);
				if (i == 3)
				{
					m_goSymbolLine2.SetActive(false);
				}
			}
		}
		m_txtType.text = _master.label;
		m_txtPower.text = _master.power.ToString();
	}

	public void Initialize(DataCardParam _card )
	{
		data_card = _card;
		Initialize(data_card.master);
	}



	void Start()
	{
		m_btn.onClick.AddListener(() =>
		{
			if (data_card != null)
			{
				OnClickCard.Invoke(data_card.card_serial);
			}
		});
	}


	public void Deleted()
	{
		Destroy(gameObject);
	}




}
