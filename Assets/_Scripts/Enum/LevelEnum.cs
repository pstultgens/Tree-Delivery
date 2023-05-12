using System;

public enum LevelEnum
{
    [LevelName("Main Menu")]
    MainMenu,
    [LevelName("Tutorial Level")]
    Tutorial,
    [LevelName("Test Level")]
    Test,
    [LevelName("Level 1 - Easy")]
    Easy1,
    [LevelName("Level 2 - Easy")]
    Easy2,
    [LevelName("Level 3 - Easy")]
    Easy3,
    [LevelName("Level 4 - Easy")]
    Easy4,
    [LevelName("Level 5 - Easy")]
    Easy5,
    [LevelName("Level 6 - Easy")]
    Easy6,
    [LevelName("Level 7 - Easy")]
    Easy7,
    [LevelName("Level 1 - Hard")]
    Hard1,
    [LevelName("Level 2 - Hard")]
    Hard2,
    [LevelName("Level 3 - Hard")]
    Hard3,
    [LevelName("Level 4 - Hard")]
    Hard4,
    [LevelName("Level 5 - Hard")]
    Hard5,
    [LevelName("Level 6 - Hard")]
    Hard6,
    [LevelName("Level 7 - Hard")]
    Hard7
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