using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deco_PutObject : MonoBehaviour
{
    Transform room;

    public GameObject objFactory;
    GameObject obj;
    bool canPut = true;
    public Material can;
    public Material cant;
    Material origMat;
    Renderer objRenderer;
    Deco_ObjectCol objCol;

    // Start is called before the first frame update
    void Start()
    {
        room = GameObject.Find("Room").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Deco_ChangeView.Instance.viewState == Deco_ChangeView.ViewState.Second_Demen && objFactory.CompareTag("FloorObj"))
        {
            SecondPut();
        }
        else if (Deco_ChangeView.Instance.viewState == Deco_ChangeView.ViewState.Third_Demen)
        {
            ThirdPut();
        }
        else if (Deco_ChangeView.Instance.viewState == Deco_ChangeView.ViewState.First)
        {
            FirstPut();
        }
    }

    public void delObj()
    {
        if (obj)
        {
            Destroy(obj);
            obj = null;
        }
    }

    void SecondPut()
    {
        // Ű�� ������ ������Ʈ �̸����� ����
        if (Input.GetKeyDown(KeyCode.G))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 16f, LayerMask.GetMask("Floor")))
            {
                obj = Instantiate(objFactory);
                objRenderer = obj.GetComponentInChildren<Renderer>();
                obj.transform.parent = transform;
                objCol = obj.transform.GetChild(0).gameObject.AddComponent<Deco_ObjectCol>();
                origMat = objRenderer.material;
            }
        }
        // ������ �ִ� ���� ������Ʈ �̵�
        else if (Input.GetKey(KeyCode.G) && obj)
        {
            canPut = !objCol.IsCollide;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 16f, LayerMask.GetMask("Floor")))
                obj.transform.position = hit.point;
            else
            {
                canPut = false;
            }

            if (canPut)
                objRenderer.material = can;
            else
                objRenderer.material = cant;

            if (Input.GetKey(KeyCode.Q))
                obj.transform.Rotate(0, -100f * Time.deltaTime, 0);
            else if (Input.GetKey(KeyCode.E))
                obj.transform.Rotate(0, 100f * Time.deltaTime, 0);
        }
        // ��ġ ������ �� Ű�� ���� ����
        else if (Input.GetKeyUp(KeyCode.G) && canPut && obj)
        {
            objRenderer.material = origMat;
            obj.GetComponentInChildren<Collider>().isTrigger = false;
            obj.GetComponentInChildren<Rigidbody>().useGravity = true;
            obj.transform.parent = room;
            obj = null;
        }
        // ��ġ �Ұ��� �� �� Ű�� ���� ����
        else if (Input.GetKeyUp(KeyCode.G) && !canPut && obj)
        {
            Destroy(obj);
            obj = null;
        }
    }

    void ThirdPut()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 50f, LayerMask.GetMask("Floor")) && objFactory.CompareTag("FloorObj"))
            {
                obj = Instantiate(objFactory);
                objRenderer = obj.GetComponentInChildren<Renderer>();
                obj.transform.parent = transform;
                objCol = obj.transform.GetChild(0).gameObject.AddComponent<Deco_ObjectCol>();
                origMat = objRenderer.material;
            }
            else if (Physics.Raycast(ray, out hit, 50f, LayerMask.GetMask("Wall")) && objFactory.CompareTag("WallObj"))
            {
                obj = Instantiate(objFactory);
                objRenderer = obj.GetComponentInChildren<Renderer>();
                obj.transform.parent = transform;
                obj.transform.forward = hit.normal;
                objCol = obj.transform.GetChild(0).gameObject.AddComponent<Deco_ObjectCol>();
                origMat = objRenderer.material;
            }
        }
        else if (Input.GetKey(KeyCode.G) && obj)
        {
            canPut = !objCol.IsCollide;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 50f, LayerMask.GetMask("Floor")) && objFactory.CompareTag("FloorObj"))
            {
                obj.transform.position = hit.point;
                Vector3 angle = obj.transform.eulerAngles;
                angle.z = 0;
                angle.x = hit.normal.x;
                obj.transform.eulerAngles = angle;
            }
            else if (Physics.Raycast(ray, out hit, 50f, LayerMask.GetMask("Wall")) && objFactory.CompareTag("WallObj"))
            {
                obj.transform.position = hit.point;
                obj.transform.forward = hit.normal;
            }
            else
            {
                canPut = false;
            }

            if (canPut)
                objRenderer.material = can;
            else
                objRenderer.material = cant;

            if (objFactory.CompareTag("FloorObj"))
            {
                if (Input.GetKey(KeyCode.Q))
                    obj.transform.Rotate(0, -100f * Time.deltaTime, 0);
                else if (Input.GetKey(KeyCode.E))
                    obj.transform.Rotate(0, 100f * Time.deltaTime, 0);
            }
        }
        else if (Input.GetKeyUp(KeyCode.G) && canPut && obj)
        {
            objRenderer.material = origMat;
            obj.GetComponentInChildren<Collider>().isTrigger = false;
            obj.GetComponentInChildren<Rigidbody>().useGravity = true;
            obj.transform.parent = room;
            obj = null;
        }
        else if (Input.GetKeyUp(KeyCode.G) && !canPut && obj)
        {
            Destroy(obj);
            obj = null;
        }
    }

    void FirstPut()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 50f, LayerMask.GetMask("Floor")) && objFactory.CompareTag("FloorObj"))
            {
                obj = Instantiate(objFactory);
                objRenderer = obj.GetComponentInChildren<Renderer>();
                obj.transform.parent = transform;
                obj.transform.forward = -Camera.main.transform.forward;
                objCol = obj.transform.GetChild(0).gameObject.AddComponent<Deco_ObjectCol>();
                origMat = objRenderer.material;
            }
            else if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 50f, LayerMask.GetMask("Wall")) && objFactory.CompareTag("WallObj"))
            {
                obj = Instantiate(objFactory);
                objRenderer = obj.GetComponentInChildren<Renderer>();
                obj.transform.parent = transform;
                obj.transform.forward = hit.normal;
                objCol = obj.transform.GetChild(0).gameObject.AddComponent<Deco_ObjectCol>();
                origMat = objRenderer.material;
            }
        }
        else if (Input.GetKey(KeyCode.G) && obj)
        {
            canPut = !objCol.IsCollide;

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 50f, LayerMask.GetMask("Floor")) && objFactory.CompareTag("FloorObj"))
            {
                obj.transform.position = hit.point;
                Vector3 angle = obj.transform.eulerAngles;
                angle.z = 0;
                angle.x = hit.normal.x;
                obj.transform.eulerAngles = angle;
            }
            else if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 50f, LayerMask.GetMask("Wall")) && objFactory.CompareTag("WallObj"))
            {
                obj.transform.position = hit.point;
                obj.transform.forward = hit.normal;
            }
            else
            {
                canPut = false;
            }

            if (canPut)
                objRenderer.material = can;
            else
                objRenderer.material = cant;

            if (objFactory.CompareTag("FloorObj"))
            {
                if (Input.GetKey(KeyCode.Q))
                    obj.transform.Rotate(0, -100f * Time.deltaTime, 0);
                else if (Input.GetKey(KeyCode.E))
                    obj.transform.Rotate(0, 100f * Time.deltaTime, 0);
            }
        }
        else if (Input.GetKeyUp(KeyCode.G) && canPut && obj)
        {
            objRenderer.material = origMat;
            obj.GetComponentInChildren<Collider>().isTrigger = false;
            obj.GetComponentInChildren<Rigidbody>().useGravity = true;
            obj.transform.parent = room;
            obj = null;
        }
        else if (Input.GetKeyUp(KeyCode.G) && !canPut && obj)
        {
            Destroy(obj);
            obj = null;
        }
    }
}