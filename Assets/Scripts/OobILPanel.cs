using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OobILPanel : MonoBehaviour
{
    public TMP_InputField inputField;

    public TextAsset initialTextAsset;

    // Start is called before the first frame update
    void Start()
    {
        inputField.text = initialTextAsset.text;
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
