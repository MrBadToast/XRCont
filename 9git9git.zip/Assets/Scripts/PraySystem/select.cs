using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class select : MonoBehaviour
{

    static private select instance;
    static public select Instance { get { return instance; } }

    //public ScriptableObject playerProperty;

    //�ð��� ������ �� �������� ��Ȯ��
    private float nextActionTime = 0.0f;
    //�ð��̸� �׸������� ���ؽ� ����Ʈ
    public List<Vector2> points;
    //�ð��̿� �浹�� ������Ʈ�� �����ϴ� ����Ʈ
    public List<GameObject> adds;
    //�����̿� ����� ������Ʈ�� �����ϱ� ���� ������Ʈ
    //��ô�� ��ġ ������ ����
    public GameObject mousePos;
    //���콺 ���� ���� ��
    public enum MouseState { idle = 0, lasso, grab, shot }
    public MouseState MState;

    public GameObject Player;
    public GameObject prefabs;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        MState = MouseState.idle;
    }

    void Update()
    {

        switch (MState)
        {
            case MouseState.idle:
                MouseStateIdle();
                break;
            case MouseState.lasso:
                MouseStatelasso();
                break;
            case MouseState.grab:
                MouseStateGrab();
                break;
            case MouseState.shot:
                MouseStateShot();
                break;
            default:
                break;
        }

        if (!ClapsManager.Instance.IsClapZone(getMousePos()) && MState != MouseState.idle)
        {
            MState = MouseState.idle;
            points.Clear();
            if (adds.Count > 0)
            {
                Debug.Log("IsClapZone");
                foreach (var item in adds)
                {
                    Destroy(item);
                }
                adds.Clear();
            }
        }

    }
    private void MouseStateIdle()
    {
        if (Input.GetMouseButtonDown(0))
            MState = MouseState.lasso;
    }
    private void MouseStatelasso()
    {
        if (Input.GetMouseButton(0))
        {
            if (Time.time > nextActionTime)
            {
                nextActionTime = Time.time + 0.1f;
                points.Add(getMousePos());
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            mousePos.transform.position = getMousePos();
            this.gameObject.AddComponent<PolygonCollider2D>();
            Invoke("DestroyPolygonCollider2D", 0.1f);
            GetComponent<PolygonCollider2D>().SetPath(0, points);
            GetComponent<PolygonCollider2D>().isTrigger = true;
            points.Clear();
            MState = MouseState.idle;
        }
    }
    private void MouseStateGrab()
    {
        //����Ʈ ũ�Ⱑ 0 �� ��� ���õȰ��� �ƹ��͵� ����
        //Ȥ�ø� ���� 
        if (adds.Count <= 0)
        {
            MState = MouseState.idle;
        }

        //����� ������Ʈ�� ���콺 Ŀ���� ���� �ٴ�
        mousePos.transform.position = getMousePos();

        //���õ� ������Ʈ���� ���콺�������� ����
        Vector3 temp = new Vector3(0, 0, 0);

        //������Ʈ���� �߽����� ������
        foreach (var item in adds)
        {
            temp += item.transform.localPosition;
        }
        temp /= adds.Count;
        //�߽����� �������� ������Ʈ ����
        foreach (var item in adds)
        {
            item.transform.localPosition = new Vector3(0, 0, 0) + item.transform.localPosition - temp;
        }

        //����
        //����� ������Ʈ�� ����
        if (Input.GetMouseButtonDown(1)&& Player.GetComponent<PlatformerPlayerBehavior>().UseAbility(1))
        {
            foreach (var item in adds)
            {
                item.GetComponent<BoxCollider2D>().enabled = true;
                item.GetComponent<PlatformEffector2D>().enabled = true;

                SelectableVisual tempVis;
                if (item.TryGetComponent(out tempVis)) tempVis.ToggleSelectionVisual(false);

                item.GetComponent<FlatformState>().FState = FlatformState.flatformState.copied;
            }

            mousePos.transform.DetachChildren();
            adds.Clear();
            
            mousePos.transform.rotation = Quaternion.Euler(0, 0, 0);
            MState = MouseState.idle;
        }
        //����� ������Ʈ�� ��Ŭ������ ȸ��
        else if (Input.GetMouseButtonDown(2) && PlayerProperty.PrayRotate)
        {
            mousePos.transform.Rotate(new Vector3(0, 0, -45));
            ResetPlatformEffectorsRot();
        }
        //����� ������Ʈ �߻� �غ�
        else if (Input.GetMouseButtonDown(0) && PlayerProperty.PrayThrow&& Player.GetComponent<PlatformerPlayerBehavior>().UseAbility(1))
        {
            points.Clear();
            points.Add(getMousePos());
            points.Add(getMousePos());
            MState = MouseState.shot;
        }

        if (PlayerProperty.PrayRotate)
        {
            float wheelInput = Input.GetAxis("Mouse ScrollWheel");

            if (wheelInput > 0)
            {
                mousePos.transform.Rotate(new Vector3(0, 0, wheelInput * 20));
                ResetPlatformEffectorsRot();
            }
            else if (wheelInput < 0)
            {
                mousePos.transform.Rotate(new Vector3(0, 0, wheelInput * 20));
                ResetPlatformEffectorsRot();
            }
        }
      



    }
    private void MouseStateShot()
    {
        if (Input.GetMouseButton(0))
        {
            points[1] = points[0] + (points[0] - (Vector2)getMousePos());
        }
        else if (Input.GetMouseButtonUp(0))
        {
            points.Clear();
            foreach (var item in adds)
            {
                item.GetComponent<PlatformEffector2D>().enabled = false;
                Rigidbody2D temp = item.GetComponent<Rigidbody2D>();
                temp.bodyType = RigidbodyType2D.Dynamic;
                temp.AddForce((mousePos.transform.position - getMousePos()).normalized
                    * Vector2.Distance(mousePos.transform.position, getMousePos()) * 5, ForceMode2D.Impulse);

                item.GetComponent<FlatformState>().FState = FlatformState.flatformState.fired;

                //Destroy(item, 1.5f);
            }
            mousePos.transform.DetachChildren();
            adds.Clear();
            MState = MouseState.idle;
        }
    }
    private Vector3 getMousePos()
    {
        Vector3 screenMousePos;
        screenMousePos = Input.mousePosition;
        screenMousePos.z = transform.position.z - Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(screenMousePos);
    }

    private void DestroyPolygonCollider2D()
    {
        Destroy(GetComponent<PolygonCollider2D>());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {

            GameObject temp = Instantiate(prefabs, other.transform.position, other.transform.rotation, mousePos.transform).gameObject;
            adds.Add(temp);

            temp.GetComponent<BoxCollider2D>().enabled = false;
            temp.GetComponent<PlatformEffector2D>().enabled = false;

            SelectableVisual tempVis;
            if (temp.TryGetComponent(out tempVis)) tempVis.ToggleSelectionVisual(true);

            MState = MouseState.grab;
            temp.GetComponent<FlatformState>().FState = FlatformState.flatformState.selected;
        }
    }

    private void ResetPlatformEffectorsRot()
    {
        foreach (var t in adds)
        {
            t.GetComponent<PlatformEffector2D>().rotationalOffset = -mousePos.transform.localRotation.eulerAngles.z;
        }
    }
}
