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
        LevelInformation("This level will be an introduction level.\n\n" +
        "After completing this level you have learned:\n" +
        "- The controls and basic mechanics of the game.")]
    Tutorial,

    [SceneName("Test Difficulty Level"),
        LevelName("Is this a test?"),
        LevelInformation("This level will be a test level designed to test the pre-knowledge of players on Binary Search Trees. " +
        "After completing this level the player's performance will be determine the difficulty of the game going forward.")]
    Test,

    [SceneName("Easy Level 1"),
        LevelName("Level 1 - Easy"),
        LevelInformation("This level is designed to familiarize the player with only a root node.\n\n" +
        "After completing this level you have learned:\n" +
        "- What a root node is.")]
    Easy1,

    [SceneName("Easy Level 2"),
        LevelName("Level 2 - Easy"),
        LevelInformation("This level is designed to familiarize the player with a small tree consisting of a root node and the left child.\n\n" +
        "After completing this level you have learned:\n" +
        "- That a parent node can have 1 child node.\n" +
        "- That the left child node has a lower value than the parent node.\n" +
        "- That a child node always has a parent node.")]
    Easy2,

    [SceneName("Easy Level 3"),
        LevelName("Level 3 - Easy"),
        LevelInformation("This level is designed to familiarize the player with a small tree consisting of a root node and the right child.\n\n" +
        "After completing this level you have learned:\n" +
        "- That the right child node has a higher value than the parent node.")]
    Easy3,

    [SceneName("Easy Level 4"),
        LevelName("Level 4 - Easy"),
        LevelInformation("This level is designed to familiarize the player with a small tree consisting of a root node with 2 children.\n\n" +
        "After completing this level you have learned:\n" +
        "- That a parent can have max 2 children nodes.")]
    Easy4,

    [SceneName("Easy Level 5"),
        LevelName("Level 5 - Easy"),
        LevelInformation("This level is designed to familiarize the player with a small tree consisting of a root node with 2 children where the left child has also children.\n\n" +
        "After completing this level you have learned:\n" +
        "- That the left child node also is a subtree with all lower values than the root node.")]
    Easy5,

    [SceneName("Easy Level 6"),
        LevelName("Level 6 - Easy"),
        LevelInformation("This level is designed to familiarize the player with a small tree consisting of a root node with 2 children where the right child has also children.\n\n" +
        "After completing this level you have learned:\n" +
        "- That the right child node also is a subtree with all higher values than the root node.")]
    Easy6,

    [SceneName("Easy Level 7"),
        LevelName("Level 7 - Easy"),
        LevelInformation("This level is designed to familiarize the player with a small tree consisting of a root node with 2 children where the children also have children.\n\n" +
        "After completing this level you have learned:\n" +
        "- How to populate a Binary Search Tree with the correct values.")]
    Easy7,

    [SceneName("Hard Level 1"),
        LevelName("Level 8 - Hard"),
        LevelInformation("This level is designed to familiarize the player with a small tree consisting of a root node and the left child.\n\n" +
        "After completing this level you have learned:\n" +
        "- That a parent node can have 1 child node.\n" +
        "- That the left child node has a lower value than the parent node.\n" +
        "- That a child node always has a parent node.")]
    Hard1,

    [SceneName("Hard Level 2"),
        LevelName("Level 9 - Hard"),
        LevelInformation("This level is designed to familiarize the player with a small tree consisting of a root node and the right child.\n\n" +
        "After completing this level you have learned:\n" +
        "- That the right child node has a higher value than the parent node.")]
    Hard2,

    [SceneName("Hard Level 3"),
        LevelName("Level 10 - Hard"),
        LevelInformation("This level is designed to familiarize the player with a small tree consisting of a root node with 2 children.\n\n" +
        "After completing this level you have learned:\n" +
        "- That a parent can have max 2 children nodes.")]
    Hard3,

    [SceneName("Hard Level 4"),
        LevelName("Level 11 - Hard"),
        LevelInformation("This level is designed to familiarize the player with a small tree consisting of a root node with 2 children where the left child has also children. " +
        "The packages can be delivered at a wrong node, without getting a hint.\n\n" +
        "After completing this level you have learned:\n" +
        "- That the left child node also is a subtree with all lower values than the root node.")]
    Hard4,

    [SceneName("Hard Level 5"),
        LevelName("Level 12 - Hard"),
        LevelInformation("This level is designed to familiarize the player with a small tree consisting of a root node with 2 children where the right child has also children. " +
        "The packages can be delivered at a wrong node, without getting a hint.\n\n" +
        "After completing this level you have learned:\n" +
        "- That the right child node also is a subtree with all higher values than the root node.")]
    Hard5,

    [SceneName("Hard Level 6"),
        LevelName("Level 13 - Hard"),
        LevelInformation("This level is designed to familiarize the player with a small tree consisting of a root node with 2 children where the children also have children. " +
        "Some packages have already been delivered and need to be evaluated and possibly reordered.\n\n" +
        "After completing this level you have learned:\n" +
        "- Reorder and populate a Binary Search Tree with the correct values")]
    Hard6,

    [SceneName("Hard Level 7"),
        LevelName("Level 14 - Hard"),
        LevelInformation("This level is designed to familiarize the player with a small tree consisting of a root node with 2 children where the children also have children. " +
        "All packages have already been delivered and need to be evaluated and possibly reordered.\n\n" +
        "After completing this level you have learned:\n" +
        "- Reorder and populate a Binary Search Tree with the correct values")]
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