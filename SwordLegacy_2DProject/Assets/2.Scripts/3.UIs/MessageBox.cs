using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefinedEnums;
using TMPro;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textMessage;
    [SerializeField] Image _msgBG;
    eMessageBoxKind _kind;
    float delayTime;
    float time;

    //Timer
    int timer = 1;
    float timerdelay;

    //HorizAct
    bool _isIn = false;
    float MinLeft;
    void Update()
    {
        time += Time.deltaTime;
        if (time >= delayTime)
        {
            gameObject.SetActive(false);
            time = 0;
            _isIn = false;
        }

        switch (_kind)
        {
            case eMessageBoxKind.Timer:
                if(time >= timer)
                {
                    if (timer > delayTime)
                        timer = 1;

                    timerdelay--;
                    _textMessage.text = timerdelay.ToString();
                    timer++;
                }
                break;
            case eMessageBoxKind.HorizAct:
               /* float MinLeft = 2000 / delayTime;
                _textMessage.rectTransform.offsetMin -= new Vector2(1, 0) *  (Time.deltaTime * MinLeft);*/
               if(_isIn)
                {
                    _textMessage.rectTransform.anchoredPosition = Vector2.MoveTowards(_textMessage.rectTransform.anchoredPosition, new Vector2(-_msgBG.rectTransform.rect.width, _textMessage.rectTransform.anchoredPosition.y), MinLeft * Time.deltaTime);
                }
               else
                {
                    _textMessage.rectTransform.anchoredPosition = new Vector2(0, _textMessage.rectTransform.anchoredPosition.y);
                }
                break;
            case eMessageBoxKind.VertzAct:
                
                break;
        }
    }
    public void OpenMessageBox(string msg, eMiniMessageBoxType boxType, eMessageBoxKind kind = eMessageBoxKind.Normal,float delay = 3)
    {
        gameObject.SetActive(true);
        _textMessage.text = msg;
        _kind = kind;
        delayTime = delay;
        switch (kind)
        {
            case eMessageBoxKind.Timer:
                _textMessage.text = delay.ToString();
                timerdelay = (int)delay;
                break;
            case eMessageBoxKind.HorizAct:
/*                _textMessage.rectTransform.offsetMin = new Vector2(1000, 0);*/

                _isIn = true;
                _textMessage.rectTransform.anchoredPosition = new Vector2(_msgBG.rectTransform.rect.width, _textMessage.rectTransform.anchoredPosition.y);
                MinLeft = (_msgBG.rectTransform.rect.width * 2) / delayTime;
                break;
            case eMessageBoxKind.VertzAct:
                _isIn = true;
                delayTime = (delay - 2) / 2;
                break;
        }
    }

    public void CloseMessageBox()
    {
        gameObject.SetActive(false);
    }

}
