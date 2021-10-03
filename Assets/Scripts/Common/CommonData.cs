using System.Text;




public static class ResourcePath
{ 
    private const string PATH_PREFAB = "Prefabs/";
    private const string PATH_CAMERA = "Camera/";
    private const string PATH_UI = "UI/";
    private const string PATH_RAINY_CLOUD = "RainyCloud/";


    public static string GetPrefabPath(string filename, PrefabPath path)
    {
        switch (path)
        {
            case PrefabPath.Root:
                return string.Concat(PATH_PREFAB, filename);
            case PrefabPath.Camera:
                return string.Concat(PATH_PREFAB, PATH_CAMERA, filename);
            case PrefabPath.UI:
                return string.Concat(PATH_PREFAB, PATH_UI, filename);
            case PrefabPath.RainyCloud:
                return string.Concat(PATH_PREFAB, PATH_RAINY_CLOUD, filename);
            default:
                break;
        }
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

