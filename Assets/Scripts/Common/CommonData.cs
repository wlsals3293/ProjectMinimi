




public static class ResourcePath
{ 
    private const string PATH_PREFAB = "Prefabs/";

    // ������ �������� �Ķ���� �߰��ؼ� enum �۾�
    public static string GetPrefabPath(string filename)
    {
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

