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

	public void Initialize(DataCardParam _card )
	{
		data_card = _card;
		//m_imgFrame.color = MasterCard.GetCardColor(_card.card_type);
		//m_txtType.text = ((MasterCard.CARD_TYPE)_card.card_type).ToString();
		//m_txtPower.text = _card.power.ToString();

		for( int i = 0; i < symbol_list.Count; i++)
		{
			int symbol_id = _card.master.GetSymbolId(i);

			if( 0 < symbol_id)
			{
				MasterCardSymbolParam symbol = DataManager.Instance.masterCardSymbol.list.Find(p => p.card_symbol_id == symbol_id);
				symbol_list[i].gameObject.SetActive(true);
				symbol_list[i].sprite = SpriteManager.Instance.Get(symbol.sprite_name);
			}
			else
			{
				symbol_list[i].gameObject.SetActive(false);
				if( i == 3)
				{
					m_goSymbolLine2.SetActive(false);
				}
			}
		}








		m_txtType.text = _card.master.label;
		m_txtPower.text = _card.master.power.ToString();



		//m_imgTypeIcon.sprite = SpriteManager.Instance.Get(MasterCard.GetIconSpriteName(_card.card_type));
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
