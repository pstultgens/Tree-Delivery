using System;

[Serializable]
public enum LevelEnum
{
    [SceneName("Main Menu"),
        LevelName("Main Menu"),
        LevelInformation("")]
    MainMenu,

    [SceneName("Tutorial"),
        LevelName("Tutorial Level"),
        LevelInformation("This level will be an introduction level. To explain the controls and basic mechanics of the game, including delivering packages and avoiding obstacles.")]
    Tutorial,

    [SceneName("Test Difficulty Level"),
        LevelName("Is this a test?"),
        LevelInformation("This level will be a test level designed to test the pre-knowledge of players on Binary Search Trees. The player's performance on this level will determine the difficulty of the game going forward.")]
    Test,

    [SceneName("Easy Level 1"),
        LevelName("Level 1 - Easy"),
        LevelInformation("This level is designed to defining only a root node.")]
    Easy1,

    [SceneName("Easy Level 2"),
        LevelName("Level 2 - Easy"),
        LevelInformation("This level is designed to familiarize the player with a small tree consisting of a root node and the left child.")]
    Easy2,

    [SceneName("Easy Level 3"),
        LevelName("Level 3 - Easy"),
        LevelInformation("This level is designed to familiarize the player with a small tree consisting of a root node and the right child.")]
    Easy3,

    [SceneName("Easy Level 4"),
        LevelName("Level 4 - Easy"),
        LevelInformation("This level is designed to familiarize the player with a small tree consisting of a root node with 2 children.")]
    Easy4,

    [SceneName("Easy Level 5"),
        LevelName("Level 5 - Easy"),
        LevelInformation("This level is designed to familiarize the player with a small tree consisting of a root node with 2 children where the left child has also children.")]
    Easy5,

    [SceneName("Easy Level 6"),
        LevelName("Level 6 - Easy"),
        LevelInformation("This level is designed to familiarize the player with a small tree consisting of a root node with 2 children where the right child has also children.")]
    Easy6,

    [SceneName("Easy Level 7"),
        LevelName("Level 7 - Easy"),
        LevelInformation("This level is designed to familiarize the player with a small tree consisting of a root node with 2 children where the children also have children.")]
    Easy7,

    [SceneName("Hard Level 1"),
        LevelName("Level 8 - Hard"),
        LevelInformation("This level is designed to familiarize the player with a small tree consisting of a root node and the left child.")]
    Hard1,

    [SceneName("Hard Level 2"),
        LevelName("Level 9 - Hard"),
        LevelInformation("This level is designed to familiarize the player with a small tree consisting of a root node and the right child.")]
    Hard2,

    [SceneName("Hard Level 3"),
        LevelName("Level 10 - Hard"),
        LevelInformation("This level is designed to familiarize the player with a small tree consisting of a root node with 2 children.")]
    Hard3,

    [SceneName("Hard Level 4"),
        LevelName("Level 11 - Hard"),
        LevelInformation("This level is designed to familiarize the player with a small tree consisting of a root node with 2 children where the left child has also children. The packages can be delivered at a wrong node, without getting a hint.")]
    Hard4,

    [SceneName("Hard Level 5"),
        LevelName("Level 12 - Hard"),
        LevelInformation("This level is designed to familiarize the player with a small tree consisting of a root node with 2 children where the right child has also children. The packages can be delivered at a wrong node, without getting a hint.")]
    Hard5,

    [SceneName("Hard Level 6"),
        LevelName("Level 13 - Hard"),
        LevelInformation("This level is designed to familiarize the player with a small tree consisting of a root node with 2 children where the children also have children. The packages spawn after an other has been delivered and the packages can be delivered at a wrong node, without getting a hint.")]
    Hard6,

    [SceneName("Hard Level 7"),
        LevelName("Level 14 - Hard"),
        LevelInformation("This level is designed to familiarize the player with a small tree consisting of a root node with 2 children where the children also have children. The packages spawn after an other has been delivered and the packages can be delivered at a wrong node, without getting any hints at all.")]
    Hard7
}

public class SceneName : Attribute
{
    public string Name;

    public SceneName(string name)
    {
        this.Name = name;
    }
}

public class LevelName : Attribute
{
    public string Name;

    public LevelName(string name)
    {
        this.Name = name;
    }
}

public class LevelInformation : Attribute
{
    public string Information;

    public LevelInformation(string information)
    {
        this.Information = information;
    }
}

public static class EnumExtensions
{
    public static string GetSceneName(this Enum value)
    {
        SceneName[] members = (SceneName[])value.GetType().GetField(value.ToString())
            .GetCustomAttributes(typeof(SceneName), false);

        if (members.Length > 0)
        {
            return members[0].Name;
        }

        return value.ToString();
    }

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

    public static string GetInformation(this Enum value)
    {
        LevelInformation[] members = (LevelInformation[])value.GetType().GetField(value.ToString())
            .GetCustomAttributes(typeof(LevelInformation), false);

        if (members.Length > 0)
        {
            return members[0].Information;
        }

        return value.ToString();
    }
}