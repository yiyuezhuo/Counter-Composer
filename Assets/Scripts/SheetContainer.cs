using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class SheetContainer : MonoBehaviour
{
    public GameObject counterPrefab;
    public GameObject sheetPrefab;
    public GameObject scrollableContent;

    VerticalLayoutGroup scrollableContentVerticalLayoutGroup;

    static Dictionary<string, Sprite> spriteLoadCache = new();

    public static Sprite LoadIcon(string name)
    {
        if(!spriteLoadCache.TryGetValue(name, out var sprite))
            sprite = spriteLoadCache[name] = Resources.Load<Sprite>($"Icons/{name}");
        return sprite;
    }

    // Start is called before the first frame update
    void Start()
    {
        Clear();

        scrollableContentVerticalLayoutGroup = scrollableContent.GetComponent<VerticalLayoutGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Clear()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }

    }

    class SideConfig
    {
        public Color panelColor;
        public Color backColorRectColor;
        public Color bottomRibbonColor;
    }

    public void Generate(OobData data, int width, int height)
    {
        Clear();

        GameObject currentSheetObj = null;
        var sheetCap = width * height;
        // var unitList = data.CollectUnit().ToList();

        Dictionary<string, Color> corspColorMap = new();

        var unitIdx = -1;

        Dictionary<string, SideConfig> sideConfigMap = new()
        {
            {"French Army", new()
            {
                panelColor = Color.blue,
                backColorRectColor = Color.white,
                bottomRibbonColor = new(0.15f, 0.4f, 0.87f)
            }},
            {"British Army", new()
            {
                panelColor = Color.red,
                backColorRectColor = Color.white,
                bottomRibbonColor = new(0.15f, 0.4f, 0.87f)
            }},
            {"Prussian Army", new()
            {
                panelColor = Color.black,
                backColorRectColor = Color.gray,
                bottomRibbonColor = new(0.15f, 0.4f, 0.87f)
            }}
        };

        // for(var i=0; i<unitList.Count; i++)
        foreach(var unit in data.CollectUnit())
        {
            Debug.Log(unit);

            var combat = 0;
            var movement = 0;
            Sprite icon;
            Color panelColor = Color.white;
            // Test Scripts (Fixed Pipeline)
            switch(unit.Category)
            {
                case UnitCategory.INFANTRY:
                    combat = unit.Strength / 100;
                    movement = 4;
                    icon = LoadIcon("infantry_nato");
                    break;
                case UnitCategory.CAVALRY:
                    combat = unit.Strength / 100;
                    movement = 6;
                    icon = LoadIcon("cavalry_nato");
                    break;
                case UnitCategory.ARTILLERY:
                    combat = unit.Strength / 20;
                    movement = 3;
                    icon = LoadIcon("artillery_nato");
                    break;
                default:
                    continue;
            }

            var sideConfig = sideConfigMap[unit.GetTopName()];

            unitIdx += 1;

            if(unitIdx % sheetCap == 0)
            {
                currentSheetObj = Instantiate(sheetPrefab, transform);
                var rt = currentSheetObj.GetComponent<RectTransform>();
                var gridLayoutGroup = currentSheetObj.GetComponent<GridLayoutGroup>();
                var sheetSize = gridLayoutGroup.cellSize * new Vector2(width, height);
                rt.sizeDelta = sheetSize;

                foreach(Transform t in currentSheetObj.transform)
                    Destroy(t.gameObject);
            }

            var counterObj = Instantiate(counterPrefab, currentSheetObj.transform);
            var counterPrototype = counterObj.GetComponent<CounterPrototype>();

            counterPrototype.SetBottomLeftText(combat.ToString());
            counterPrototype.SetBottomRightText(movement.ToString());
            counterPrototype.SetLeftText(SummaryName(unit.Name));
            counterPrototype.SetTopText(SummaryCommanderName(unit.CommanderName));
            counterPrototype.SetIcon(icon);
            counterPrototype.SetPanelColor(sideConfig.panelColor);
            counterPrototype.SetBackColorRect(sideConfig.backColorRectColor);
            counterPrototype.SetBottomRibbonColor(sideConfig.bottomRibbonColor);

            var corpsName = unit.GetCorpsName();
            Debug.Log(corpsName);
            if(!corspColorMap.TryGetValue(corpsName, out var corposColor))
                corposColor = corspColorMap[corpsName] = RandomDarkColor();
            counterPrototype.SetTopRibbonColor(corposColor);
        }

        static Color RandomDarkColor() => new Color(Random.Range(0f, 0.5f), Random.Range(0f, 0.5f), Random.Range(0f, 0.5f));

        // Force content size fitter update hack: https://forum.unity.com/threads/content-size-fitter-refresh-problem.498536/
        Canvas.ForceUpdateCanvases();
        scrollableContentVerticalLayoutGroup.enabled = false;
        scrollableContentVerticalLayoutGroup.enabled = true;
    }

    public static string SummaryCommanderName(string name)
    {
        return name.Trim().Split(" ")[^1];
    }

    static string SummaryName(string name)
    {
        return string.Join("\n", name.Split(" ").Select(SummaryWord));
    }

    static string SummaryWord(string word)
    {
        word = word.Trim();

        var match = Regex.Match(word, @"^(\d+)");
        if(match.Success)
            return match.Groups[0].Value;
        return word.Substring(0, 1);
    }

    // public void Generate(string s) => Generate(OobData.Load(s));
}
