using System.Collections;
using System.Collections.Generic;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

public enum UnitLevel
{
    ARMY = 9,
    CORPS = 8,
    DIVISION = 7,
    BRIGADE = 6,
    REGIMENT = 5,
    BATTALION = 4, // BATAILLON
    SQUADRON = 3,
    BATTERY = 2,
    MISC = 1
}

public enum UnitCategory
{
    INFANTRY = 1,
    CAVALRY = 2,
    ARTILLERY = 3,
    MISC = 4, // wagon, Train
    MIXED = 5,
    UNKNOWN = 6
}

public class UnitData
{
    public string Name{get; set;}
    public string CommanderName{get; set;}
    public int Strength{get; set;}
    public int Guns{get; set;}
    public UnitLevel Level{get; set;}
    public UnitCategory Category{get; set;}
    public List<UnitData> Units{get; set;}

    public UnitData Parent;

    public override string ToString()
    {
        var unitStr = Units == null ? "" : $", Children:[{Units.Count}]";
        var siblingStr = Parent == null ? "" : $", Sibling:[{Parent.Units.Count}]";
        return $"UnitData({Name}, {CommanderName}, {Strength}, {Level}, {Category}{unitStr}{siblingStr})";
    }

    public string GetCorpsName()
    {
        var pt = this;
        while(pt != null)
        {
            if(pt.Level == UnitLevel.CORPS)
                break;
            pt = pt.Parent;
        }
        List<string> names = new();
        while(pt != null)
        {
            names.Add(pt.Name);
            pt = pt.Parent;
        }
        names.Reverse();
        return string.Join("/", names);
    }

    

    public string GetTopName()
    {
        var pt = this;
        while(pt.Parent != null)
            pt = pt.Parent;
        return pt.Name;
    }
}


public class OobData
{
    public List<UnitData> armies;

    public static IEnumerable<UnitData> CollectUnit(List<UnitData> units)
    {
        foreach(var unit in units)
        {
            yield return unit;
            if(unit.Units != null)
            {
                foreach(var _unit in CollectUnit(unit.Units))
                {
                    yield return _unit;
                }
            }   
        }
    }

    public IEnumerable<UnitData> CollectUnit() => CollectUnit(armies);

    public string Describe(UnitData unit)
    {
        List<string> names = new(){};
        var pt = unit;
        while(true)
        {
            names.Add(pt.Name);

            if(pt.Parent != null)
                pt = pt.Parent;
            else
                break;
        }
        names.Reverse();
        var nameAgg = string.Join("||", names);

        return $"{nameAgg}({unit.Category}): {unit.CommanderName} : {unit.Strength}";
    }

    public static OobData Load(string s)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)  // see height_in_inches in sample yml 
            .Build();

        var armies = deserializer.Deserialize<List<UnitData>>(s);

        Setup(armies);

        return new(){armies=armies};
    }

    static void Setup(List<UnitData> units)
    {
        foreach(var unit in units)
        {
            if(unit.Units != null)
            {
                Setup(unit.Units);
                
                foreach(var _unit in unit.Units)
                    _unit.Parent = unit;
            }   
        }
    }
}
