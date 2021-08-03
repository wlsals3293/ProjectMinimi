using UnityEditor;
using UnityEngine;

[System.Serializable]
public class SingleBlock : MonoBehaviour
{
    private Transform trans = null;
    public new Transform transform
    {
        get
        {
            if (trans == null)
            {
                trans = GetComponent<Transform>();
            }
            return trans;
        }
    }
    private Vector3 dir = Vector3.zero;

    [Space]
    [SerializeField] private float minHigh = 0f;
    [SerializeField] private float minHighTime = 1f;
    [Space]
    [SerializeField] private float maxHigh = 5f;
    [SerializeField] private float maxHighTime = 1f;

    [Space]
    [SerializeField] private float blockDirChangeDelay = 0.5f;
    [SerializeField] private float startDelay = 0.5f;

    private DirectionType dirType = DirectionType.Y;
    private Vector3 fallDirection = Vector3.up;

    private float _minHigh = 0f;
    private float _minHighTime = 0f;
    
    private float _maxHigh = 0f;
    private float _maxHighTime = 0f;

    private float _blockDirChangeDelay = 0f;
    private float _nextBlockDelay = 0f;


    private bool use = false;

    private bool down = false;
    private bool stop = false;
    private bool hit = false;

    private float timer = 0f;
    private float endTimer = 9999f;

    private LayerMask layerMask;
    private void Awake()
    {
        // 일괄처리시 빼야됨 그냥이렇게..
        Init(fallDirection, 0f, 0f, 0f, 0f, 0f, 0f, true);
    }

    private void Start()
    {
        layerMask = LayerMask.GetMask("Object", "Ground");
    }

    public void Init(
        Vector3 dir
        , float minHigh, float minHighTime, float maxHigh, float maxHighTime
        , float dirChangeDelay
        , float nextBlockDelay
        , bool single
        )
    {
        this.dir = dir;

        if (single == false)
        {
            //_minHigh = this.minHigh + minHigh;
            //_minHighTime = this.minHighTime + minHighTime;
            //_maxHigh = this.maxHigh + maxHigh;
            //_maxHighTime = this.maxHighTime + maxHighTime;
            //_blockDirChangeDelay = this.blockDirChangeDelay + dirChangeDelay;
            //_nextBlockDelay = this.nextBlockDelay + nextBlockDelay;

            _minHigh = minHigh;
            _minHighTime = minHighTime;
            _maxHigh = maxHigh;
            _maxHighTime = maxHighTime;
            _blockDirChangeDelay = dirChangeDelay;
            _nextBlockDelay = nextBlockDelay;

            SetLocalPosition(dir * _maxHigh, dirType);
            SetUse(false);
            ChangeEndTimer(_maxHighTime);
        }
        else
        {
            _minHigh = this.minHigh;
            _minHighTime = this.minHighTime;
            _maxHigh = this.maxHigh;
            _maxHighTime = this.maxHighTime;
            _blockDirChangeDelay = this.blockDirChangeDelay;
            _nextBlockDelay = this.startDelay;

            SetLocalPosition(dir * _maxHigh, dirType);
            ChangeEndTimer(_maxHighTime);
            Invoke("WaitBlockActive", _nextBlockDelay);
        }
    }

    private void WaitBlockActive()
    {
        SetUse(true);
    }

    public GameObject GetGameObject()
    {
        if (transform == null)
            return null;

        return transform.gameObject;
    }

    public void ChangeEndTimer(float time)
    {
        timer = 0f;
        endTimer = time;
    }

    public void SetUse(bool active)
    {
        use = active;

        timer = 0f;
        down = false;
        stop = false;
    }

    public void OnStop()
    {
        stop = true;
        ChangeEndTimer(_blockDirChangeDelay);
    }

    public void OnHit()
    {
        hit = true;
    }

    public void Update()
    {
        if (use)
        {
            Vector3 movePos = transform.localPosition;
            if (down)
            {
                if (CheckOverEndTimer())
                {
                    hit = false;

                    if (stop)
                    {
                        down = !down;
                        stop = false;
                        ChangeEndTimer(_maxHighTime);
                    }
                    else
                    {
                        OnStop();
                    }
                }
                else
                {
                    if (hit)
                        return;

                    movePos = dir * Mathf.Lerp(_maxHigh, _minHigh, timer / _minHighTime);

                    if (GetLocalPosition(movePos, dirType) > GetLocalPosition(transform.localPosition, dirType))
                    {
                        switch (dirType)
                        {
                            case DirectionType.X:
                                movePos.x = GetLocalPosition(transform.localPosition, dirType);
                                break;
                            case DirectionType.Y:
                                movePos.y = GetLocalPosition(transform.localPosition, dirType);
                                break;
                            case DirectionType.Z:
                                movePos.z = GetLocalPosition(transform.localPosition, dirType);
                                break;
                            case DirectionType.All:
                                break;
                            default:
                                break;
                        }
                    }

                    SetLocalPosition(movePos, dirType);
                }
            }
            else
            {
                if (CheckOverEndTimer())
                {
                    hit = false;

                    if (stop)
                    {
                        down = !down;
                        stop = false;
                        ChangeEndTimer(_minHighTime);
                    }
                    else
                    {
                        OnStop();
                    }
                }
                else
                {
                    if (hit)
                        return;

                    movePos = dir * Mathf.Lerp(_minHigh, _maxHigh, timer / _maxHighTime);
                    if (GetLocalPosition(movePos, dirType) < GetLocalPosition(transform.localPosition, dirType))
                    {
                        switch (dirType)
                        {
                            case DirectionType.X:
                                movePos.x = GetLocalPosition(transform.localPosition, dirType);
                                break;
                            case DirectionType.Y:
                                movePos.y = GetLocalPosition(transform.localPosition, dirType);
                                break;
                            case DirectionType.Z:
                                movePos.z = GetLocalPosition(transform.localPosition, dirType);
                                break;
                            case DirectionType.All:
                                break;
                            default:
                                break;
                        }
                    }
                    SetLocalPosition(movePos, dirType);
                }
            }
        }
    }

    public float GetLocalPosition(Vector3 pos, DirectionType type = DirectionType.All)
    {
        switch (type)
        {
            case DirectionType.X:
                return pos.x;
            case DirectionType.Y:
                return pos.y;
            case DirectionType.Z:
                return pos.z;
            case DirectionType.All:
            default:
                break;
        }

        return 0f;
    }

    

    public void SetLocalPosition(Vector3 pos, DirectionType type = DirectionType.All)
    {
        if (stop == false)
        {
            switch (type)
            {
                case DirectionType.X:
                    transform.localPosition = new Vector3(pos.x, transform.localPosition.y, transform.localPosition.z);
                    break;
                case DirectionType.Y:
                    transform.localPosition = new Vector3(transform.localPosition.x, pos.y, transform.localPosition.z);
                    break;
                case DirectionType.Z:
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, pos.z);
                    break;
                case DirectionType.All:
                    transform.localPosition = pos;
                    break;
                default:
                    break;
            }

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == Tags.Minimi)
        { 
            OnHit();
        }
        //else if (collision.collider.tag == Tags.obj)
        //{
        //    OnHit();
        //}

        
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.tag == Tags.Player)
        {
            Collider[] buffers = new Collider[16];

            int count = Physics.OverlapCapsuleNonAlloc
                (
                new Vector3(collision.transform.position.x, collision.transform.position.y + 1.5f, collision.transform.position.z),
                new Vector3(collision.transform.position.x, collision.transform.position.y + 0.4f, collision.transform.position.z),
                0.5f,
                buffers,
                layerMask,
                QueryTriggerInteraction.Ignore
                );

            if(count >= 2)
            {
                collision.gameObject.GetComponent<PlayerController>().ChangeState(PlayerState.Dead);
            }
        }
    }

    private void SetTimerEnd()
    {
        timer = endTimer;
    }

    public bool CheckOverEndTimer()
    {
        timer += Time.deltaTime;

        return endTimer <= timer;
    }
}