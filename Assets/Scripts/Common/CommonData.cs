




public static class ResourcePath
{ 
    private const string PATH_PREFAB = "Prefabs/";

    // 폴더가 나뉠때는 파라미터 추가해서 enum 작업
    public static string GetPrefabPath(string filename)
    {
        return string.Concat(PATH_PREFAB, filename);
    }
    
}


/// <summary>
/// 원랜 테이블화
/// </summary>
public static class PrefabNames
{
    public const string Minimi_Dump = "Minimi_dump";
    public const string Player = "Player";
    public const string BlockMinimi = "BlockMinimi";
}

