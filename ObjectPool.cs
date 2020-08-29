using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kyon;

namespace kyon
{
	public class ObjectPool
	{
		public interface IPoolableObject
        {
        }

        public static Dictionary<IPoolableObject, List<IPoolableObject>> poolList = new Dictionary<IPoolableObject, List<IPoolableObject>>();

        public static T Rent<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component, IPoolableObject
        {
            T pooledObject = default;

            foreach(KeyValuePair<IPoolableObject, List<IPoolableObject>> pool in poolList)
            {
                if (pool.Key == prefab)
                {
                    pooledObject = GetObject(prefab, pool.Value, position, rotation);

                    pooledObject.transform.position = position;
                    pooledObject.transform.rotation = rotation;

                    return pooledObject;
                }
            }

            List<IPoolableObject> newPool = new List<IPoolableObject>();
            pooledObject = Object.Instantiate(prefab, position, rotation);
            newPool.Add(pooledObject);
            poolList.Add(prefab, newPool);

            return pooledObject;
        }

        private static T GetObject<T>(T prefab, List<IPoolableObject> pool, Vector3 position, Quaternion rotation) where T : Component, IPoolableObject
        {
            T pooledObject = default;

            bool isGetObj = false;
            foreach (T obj in pool)
            {
                if (!obj.gameObject.activeSelf)
                {
                    pooledObject = obj;
                    obj.gameObject.SetActive(true);
                    isGetObj = true;
                    break;
                }
            }

            if (!isGetObj)
            {
                pooledObject = Object.Instantiate(prefab, position, rotation);
                pool.Add(pooledObject);
            }

            return pooledObject;
        }
    }
}