// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System.Linq;
// using Unity.VisualScripting;

// public class ObjectPoolingManager : MonoBehaviour
// {
//     public static ObjectPoolingManager instance;
//     public static List<PooledObjectInfo> objectPools = new List<PooledObjectInfo>();
//     private static GameObject ObjectPooledParent;


//     private void Awake()
//     {
//         instance = this;
//         ObjectPooledParent = new GameObject("ParentObject");
//     }
//     private void Update()
//     {
//         if (Input.GetMouseButtonDown(0))
//         {

//             foreach (PooledObjectInfo obj in objectPools)
//             {

//                 Debug.Log(obj.name);
//             }
//         }
//     }
//     public GameObject spawnGameObject(GameObject ObjectToSpawn, Vector3 Position, Quaternion Rotation)
//     {

//         PooledObjectInfo pool = objectPools.Find(p => p.name == ObjectToSpawn.name);


//         if (pool == null)
//         {
//             pool = new PooledObjectInfo() { name = ObjectToSpawn.name };
//             //  Debug.Log("OBject Added " + ObjectToSpawn.name);
//             objectPools.Add(pool);
//         }

//         GameObject SpawanbleObject = pool.gameObjects.FirstOrDefault();

//         if (SpawanbleObject == null)
//         {

//             SpawanbleObject = Instantiate(ObjectToSpawn, Position, Rotation);
//             SpawanbleObject.transform.SetParent(ObjectPooledParent.transform);


//         }
//         else
//         {

//             SpawanbleObject.transform.position = Position;
//             SpawanbleObject.transform.rotation = Rotation;
//             pool.gameObjects.Remove(SpawanbleObject);
//             SpawanbleObject.SetActive(true);

//         }

//         return SpawanbleObject;


//     }
//     public void ReturnObjectToPool(GameObject Obj)
//     {
//         string goName = Obj.name.Substring(0, Obj.name.Length - 7);
//         PooledObjectInfo pool = objectPools.Find(p => p.name == goName);
//         if (pool == null)

//             Debug.LogWarning("object not found in the pool  " + Obj.name);

//         else
//         {
//             Obj.SetActive(false);
//             pool.gameObjects.Add(Obj);
//         }

//     }


// }
// public class PooledObjectInfo
// {

//     public string name;
//     public List<GameObject> gameObjects = new List<GameObject>();
// }