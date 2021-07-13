




public static class ResourcePath
{ 
    private const string PATH_PREFAB = "Prefabs/";

    private const string PATH_UI = "UI/";



    public static string GetPrefabPath(string filename, PrefabPath path)
    {
        switch (path)
        {
            case PrefabPath.Root:
                return string.Concat(PATH_PREFAB, filename);
            case PrefabPath.UI:
                return string.Concat(PATH_PREFAB, PATH_UI, filename);
            default:
                break;
        }
        return string.Concat(PATH_PREFAB, filename);
    }

}


/// <summary>
/// ���� ���̺�ȭ
/// </summary>
public static class PrefabNames
{
    public const string Minimi_Dump = "Minimi_dump";
    public const string Player = "Player";
    public const string BlockMinimi = "BlockMinimi";
}

