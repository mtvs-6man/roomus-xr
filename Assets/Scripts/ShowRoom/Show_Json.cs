using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Show_Json : MonoBehaviour
{
    public static Show_Json Instance;
    public InputField loadInputField; 
    public Objects objects;
    ArrayJson arrayJson;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);

        arrayJson = new ArrayJson();
        arrayJson.datas = new List<SaveJsonInfo>();

        loadInputField.onSubmit.AddListener(LoadFile);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LoadFile(string roomName)
    {
        if (roomName.Length == 0)
            return;
        //mapData.txt를 불러오기
        string jsonData = File.ReadAllText(Application.dataPath + "/" + roomName + ".txt");
        //ArrayJson 형태로 Json을 변환
        ArrayJson arrayJson = JsonUtility.FromJson<ArrayJson>(jsonData);
        //ArrayJson의 데이터로 방 생성
        Destroy(GameObject.Find("Room"));
        GameObject newRoom = new GameObject("Room");
        GameObject newWalls = new GameObject("Walls");
        newRoom.transform.position = Vector3.zero;
        newRoom.transform.rotation = Quaternion.identity;
        newRoom.transform.localScale = Vector3.one;
        newWalls.transform.parent = newRoom.transform;
        newWalls.transform.position = Vector3.zero;
        newWalls.transform.rotation = Quaternion.identity;
        newWalls.transform.localScale = Vector3.one;
        Deco_RoomInit.Instance.MakeRoom(arrayJson.XSize, arrayJson.YSize, arrayJson.ZSize, arrayJson.balcony, newRoom.transform);
        //ArrayJson의 데이터를 가지고 오브젝트 생성
        for (int i = 0; i < arrayJson.datas.Count; i++)
        {
            SaveJsonInfo info = arrayJson.datas[i];
            LoadObject(info.idx, info.position, info.eulerAngle, info.localScale, newRoom.transform);
        }
    }
    void LoadObject(int idx, Vector3 position, Vector3 eulerAngle, Vector3 localScale, Transform room)
    {
        //해당 위치에 BlueCube를 생성해서 놓는다.
        foreach (GameObject go in objects.datas)
        {
            if (go.GetComponent<Deco_Idx>().Idx == idx)
            {
                GameObject obj = Instantiate(go);
                obj.transform.localPosition = position;
                obj.transform.localEulerAngles = eulerAngle;
                obj.transform.localScale = localScale;
                obj.transform.parent = room;
            }
        }
    }
}