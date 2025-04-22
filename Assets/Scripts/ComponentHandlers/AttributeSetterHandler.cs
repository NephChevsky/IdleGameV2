using Assets.Scripts.Engines;
using Assets.Scripts.Models;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttributeSetterHandler : MonoBehaviour
{
    public TMP_Text NameText;
    public TMP_Text ValueText;
    public Button Minus;
    public Button Plus;

    private AffixType AffixType;

	public void SetName(AffixType affix)
    {
        AffixType = affix;
		NameText.text = affix.ToString();
        Minus.onClick.AddListener(LevelDown);
		Plus.onClick.AddListener(LevelUp);
	}

    public void SetValue(int value)
    {
        ValueText.text = value.ToString();
    }

    public void LevelDown()
    {
        if (GameEngine.AffectedAttributePoints[AffixType] > 0)
        {
			GameEngine.AffectedAttributePoints[AffixType]--;
            SetValue(GameEngine.AffectedAttributePoints[AffixType]);
		}
    }

    public void LevelUp()
    {
		if (GameEngine.AffectedAttributePoints.Sum(x => x.Value) < MapEngine.Player.Level - 1)
		{
			GameEngine.AffectedAttributePoints[AffixType]++;
			SetValue(GameEngine.AffectedAttributePoints[AffixType]);
		}
	}
}
