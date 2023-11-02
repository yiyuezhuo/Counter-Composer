using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TMP_InputField oobSource;
    public SheetContainer sheetContainer;
    public TMP_InputField widthSource;
    public TMP_InputField heightSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Generate()
    {
        var oobData = OobData.Load(oobSource.text);
        var width = int.Parse(widthSource.text);
        var height = int.Parse(heightSource.text);

        sheetContainer.Generate(oobData, width, height);
    }
}
