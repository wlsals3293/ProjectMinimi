using UnityEngine;

[CreateAssetMenu(fileName = "NewMountObjectConfig", menuName = "Config/MountObjectConfig", order = 1)]
public class MountObjectConfig : ScriptableObject
{
    [Tooltip("��ǥ ��ġ���� �����ϴ� �ð�. ���� Ŭ���� ������ �̵�.")]
    public float mountSmoothTime = 0.5f;

    [Tooltip("�յ� �� �ִ� ���·� ��ȯ�Ǵ� �ð�.")]
    public float mountBlendTime = 2f;

    [Tooltip("�� �ִ� ���� ����.")]
    public float floatingHeight = 1.1f;

    [Tooltip("���Ʒ��� �պ��ϴ� �Ÿ�.")]
    public float floatingRange = 0.2f;

    [Tooltip("�պ� �ӵ�.")]
    public float floatingSpeed = 1f;

    [Tooltip("ȸ�� �ӵ�.")]
    public Vector3 rotationSpeed = new Vector3(0f, 20f, 0f);
}
