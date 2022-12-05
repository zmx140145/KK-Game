﻿using System.Collections;
using System.Collections.Generic;
using MyUnitTools;
using UnityEngine;
using System;
public class GameObjectPoolSystem : SingletonBase<GameObjectPoolSystem>
{
    public class Relay
    {
    public Relay(float time,float Long)
     {
        this.time=time;
        this.Long=Long;
     }
      public float time;
      public float Long;
    }
    private PriortyQueue<Relay> RelayTimeQue=new PriortyQueue<Relay>((x,y)=>y.Long.CompareTo(x.Long));
    
    private float relayTime=0f;
    public Relay RelayTime{
        get{
            if(RelayTimeQue.Count>0)
            {
  Relay r=RelayTimeQue.Peek();
  return r;
            }
          else
          {
 return null;
          }
        }
        set
        {
            relayTime+=value.Long;
            RelayTimeQue.Enqueue(value);
            Time.timeScale=value.time;
        }
    }
    public void RemoveRelayTime()
    {
      RelayTimeQue.Dequeue();
      if(RelayTimeQue.Count>0)
      {
        Time.timeScale=RelayTimeQue.Peek().time;
      }
      else
      {
        Time.timeScale=1f;
      }
    }
    public bool isSlowFrame=>relayTime>0;
    [SerializeField,Header("预制体")] private List<GameObjectAssets> _assetsList = new List<GameObjectAssets>();
    [SerializeField] private Transform poolObjectParent;
    private Dictionary<string, Queue<GameObject>> pools = new Dictionary<string, Queue<GameObject>>();

    private void Awake()
    {
        InitPool();
    }
private void Update() {
  
    if(relayTime<=0)
    {
     relayTime=0f;
    }
    else
    {
          relayTime-=Time.deltaTime;
    }
}
    private void Start()
    {

    }

    private void InitPool()
    {
        if (_assetsList.Count == 0) return;


        //首先遍历外面配置的资源
        for (int i = 0; i < _assetsList.Count; i++)
        {
            //检查列表第一个元素的内容是否已经在池子里面了，没有的话就创建一个
            if (!pools.ContainsKey(_assetsList[i].assetsName))
            {
                pools.Add(_assetsList[i].assetsName, new Queue<GameObject>());

                if (_assetsList[i].prefab.Length == 0) return;

                //创建完毕后，遍历这个对象的总数，比如总算5，那么就创建5个，然后存进字典
                for (int j = 0; j < _assetsList[i].count; j++)
                {
                    GameObject temp_Gameobject = Instantiate(_assetsList[i].prefab[UnityEngine.Random.Range(0, _assetsList[i].prefab.Length)]);
                    temp_Gameobject.transform.SetParent(poolObjectParent);
                    temp_Gameobject.transform.position = Vector3.zero;
                    temp_Gameobject.transform.rotation = Quaternion.identity;
                    pools[_assetsList[i].assetsName].Enqueue(temp_Gameobject);
                    temp_Gameobject.SetActive(false);
                }
            }
        }
    }

    public GameObject TakeGameObject(string objectName)
    {
        if (!pools.ContainsKey(objectName)) return null;
        GameObject dequeueObject = pools[objectName].Dequeue();
        pools[objectName].Enqueue(dequeueObject);
        dequeueObject.SetActive(true);
        return dequeueObject;
    }

    public void TakeGameobject(string objectName, Vector3 position, Quaternion rotation)
    {
        if (!pools.ContainsKey(objectName)) return;

        GameObject dequeueObject = pools[objectName].Dequeue();
        pools[objectName].Enqueue(dequeueObject);
        dequeueObject.SetActive(true);
        dequeueObject.transform.position = position;
        dequeueObject.transform.rotation = rotation;
        dequeueObject.GetComponent<IPool>().SpawnObject();
    }

    public void TakeGameobject(string objectName, Vector3 position, Quaternion rotation,Transform user)
    {
        if (!pools.ContainsKey(objectName)) return;

        GameObject dequeueObject = pools[objectName].Dequeue();
        pools[objectName].Enqueue(dequeueObject);
        dequeueObject.SetActive(true);
        dequeueObject.transform.position = position;
        dequeueObject.transform.rotation = rotation;
        dequeueObject.GetComponent<IPool>().SpawnObject(user);
    }
    
    public void RecyleGameObject(GameObject gameObject)
    {
        gameObject.transform.position = Vector3.zero;
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.SetActive(false);
    }

    [System.Serializable]
    private class GameObjectAssets
    {
        public string assetsName;
        public int count;
        public GameObject[] prefab;
    }
}
