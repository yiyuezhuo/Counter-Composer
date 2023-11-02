using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CounterPrototype : MonoBehaviour
{
    public TMP_Text topText;
    public TMP_Text leftText;
    public TMP_Text rightText;
    public TMP_Text bottomLeftText;
    public TMP_Text bottomMiddleText;
    public TMP_Text bottomRightText;
    public Image backColorRect;
    public Image icon;
    public Image topRibbon;
    public Image bottomRibbon;
    public Image panel; // panel is an image using a "panel" looking source image

    static void SetText(TMP_Text text, string s)
    {
        if(s == "")
        {
            text.gameObject.SetActive(false);
            return;
        }
        text.gameObject.SetActive(true);
        text.text = s;
    }

    // APIs layer （maybe called by runtime script
    public void SetBottomLeftText(string s) => SetText(bottomLeftText, s);
    public void SetBottomMiddleText(string s) => SetText(bottomMiddleText, s);
    public void SetBottomRightText(string s) => SetText(bottomRightText, s);
    public void SetTopText(string s) => SetText(topText, s);
    public void SetLeftText(string s) => SetText(leftText, s);
    public void SetRightText(string s) => SetText(rightText, s);
    public void SetBackColorRect(Color color)
    {
        backColorRect.color = color;
    }

    public void SetIcon(Sprite sprite)
    {
        icon.sprite = sprite;
    }

    public void SetTopRibbonColor(Color color)
    {
        topRibbon.color = color;
    }

    public void SetBottomRibbonColor(Color color)
    {
        bottomRibbon.color = color;
    }
    public void SetPanelColor(Color color)
    {
        panel.color = color;
    }
    // End APIs layer


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
