using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MiniStatusBox : MonoBehaviour
{
    [SerializeField] Image _characterIcon;
    [SerializeField] TextMeshProUGUI _CharacterName;
    [SerializeField] TextMeshProUGUI _DesignationName;
    [SerializeField] Slider _hpBar;
    [SerializeField] TextMeshProUGUI _txtattackPower;
    [SerializeField] TextMeshProUGUI _txtDefencePower;
    
    public void InitSetData(Sprite charIcon, string charName, string designName, int att, int def)
    {
        _characterIcon.sprite = charIcon;
        _CharacterName.text = charName;
        if (designName.Length > 0)
            _DesignationName.text = designName;
        else
            _DesignationName.gameObject.SetActive(false);
        _hpBar.value = 1f;
        _txtattackPower.text = att.ToString();
        _txtDefencePower.text = def.ToString();
    }

    public void SetDesignationName(string name)
    {
        _DesignationName.gameObject.SetActive(true);
        _DesignationName.text = name;
    }

    public void SetHPRate(float rate)
    {
        _hpBar.value = rate;
    }
    public void SetStatValue(int att, int def)
    {
        _txtattackPower.text = att.ToString();
        _txtDefencePower.text = def.ToString();
    }
    public void SetAttValue(int att)
    {
        _txtattackPower.text = att.ToString();
    }
    public void SetDefValue(int def)
    {
        _txtDefencePower.text = def.ToString();
    }

    public void Enables(bool isOn)
    {
        gameObject.SetActive(isOn);
    }
}
