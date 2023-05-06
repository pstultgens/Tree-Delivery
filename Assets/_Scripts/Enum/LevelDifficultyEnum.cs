using System;

public enum LevelDifficultyEnum
{
    [LevelName("Tutorial Level")]
    Tutorial,
    [LevelName("Test Level")]
    Test,
    [LevelName("Easy Level 1")]
    Easy1,
    [LevelName("Hard Level 1")]
    Hard1,
    [LevelName("Easy Level 2")]
    Easy2,
    [LevelName("Hard Level 2")]
    Hard2,
    [LevelName("Easy Level 3")]
    Easy3,
    [LevelName("Hard Level 3")]
    Hard3,
    [LevelName("Easy Level 4")]
    Easy4,
    [LevelName("Hard Level 4")]
    Hard4,
    [LevelName("Easy Level 5")]
    Easy5,
    [LevelName("Hard Level 5")]
    Hard5
}

public class LevelName : Attribute 
{
    public string Name;

    public LevelName(string name)
    {
        this.Name = name;
    }
}

public static class EnumExtensions
{
    public static string GetName(this Enum value)
    {
        LevelName[] members = (LevelName[])value.GetType().GetField(value.ToString())
            .GetCustomAttributes(typeof(LevelName), false);

        if (members.Length > 0)
        {
            return members[0].Name;
        }

        return value.ToString();
    }
}