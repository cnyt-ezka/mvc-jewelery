using System.Collections.Generic;
using MVC.Base.Runtime.Abstract.Function;
using MVC.Base.Runtime.Abstract.Model;
using MVC.Base.Runtime.Concrete.Data.ValueObject;
using UnityEngine;

namespace MVC.Base.Runtime.Concrete.Model
{
    public class PoolModel : IPoolModel
    {
        private Dictionary<string, ObjectPoolVO> _poolVos = new();

        private Dictionary<string, Queue<GameObject>> _objectQueues = new();

        private Dictionary<string, List<GameObject>> _activeObjects = new();

        private GameObject _container;

        [PostConstruct]
        public void OnPostConstruct()
        {
            _container = new GameObject("PoolObjects");
            Object.DontDestroyOnLoad(_container);
        }

        public void Pool(string key, GameObject prefab, int count)
        {
            if (prefab.GetComponent<IPoolable>() == null)
            {
                Debug.LogError("You cant create " + prefab.name + ". IPoolable class is missing");
                return;
            }
            var vo = new ObjectPoolVO
            {
                Key = key,
                Count = count,
                Prefab = prefab
            };

            if (!_poolVos.ContainsKey(vo.Key))
                _poolVos.Add(vo.Key, vo);

            Queue<GameObject> queue;

            if (!_objectQueues.ContainsKey(vo.Key))
            {
                queue = new Queue<GameObject>();
                _objectQueues.Add(vo.Key, queue);
                _activeObjects.Add(vo.Key, new());         
            }
            else
            {
                queue = _objectQueues[vo.Key];
            }

            for (var i = 0; i < vo.Count; i++)
            {
                var newObj = GameObject.Instantiate(vo.Prefab,_container.transform);
                newObj.GetComponent<IPoolable>().PoolKey = key;
                newObj.name = vo.Key;
                newObj.SetActive(false);
                queue.Enqueue(newObj);
            }
        }

        public void RemoveAll()
        {
            foreach (var list in _activeObjects.Values)
            {
                foreach (var obj in list)
                {
                    Return(obj);
                }
            }
        }
        public void RemoveAll(string key)
        {
            if (!_activeObjects.ContainsKey(key))
            {
                Debug.LogWarning("There is no key in the pool : " + key);
                return;
            }
            foreach (var obj in _activeObjects[key])
            {
                Return(obj);
            }
        }


        public GameObject Get(string key,Transform parent)
        {
            var item = Get(key,true);
            item.transform.SetParent(parent, false);
            item.GetComponent<IPoolable>().OnGetFromPool();
            return item;
        }
        
        public GameObject Get(string key,bool withParent = false)
        {
            if (!_objectQueues.ContainsKey(key))
            {
                Debug.LogWarning("Not object in pool with key " + key);
                return null;
            }

            if (_objectQueues[key].Count == 0)
            {
                Debug.LogWarning("Not enough object in pool with key " + key + ". Instantiating.");
                ObjectPoolVO vo = _poolVos[key];
                Pool(vo.Key, vo.Prefab, 1);
            }

            var newObj = _objectQueues[key].Dequeue();
            _activeObjects[key].Add(newObj);
            newObj.SetActive(true);
            if(!withParent)
                newObj.GetComponent<IPoolable>().OnGetFromPool();
            return newObj;
        }

        public void Return(GameObject obj)
        {
            var returnObj = obj.GetComponent<IPoolable>();
            if (returnObj == null)
            {
                Debug.LogError("You cant destroy " + obj.name + ". IPoolable class is missing");
                return;
            }

            if (!obj.activeInHierarchy)
                return;

            returnObj.OnReturnToPool();
            obj.transform.SetParent(_container.transform);
            obj.SetActive(false);
            _objectQueues[returnObj.PoolKey].Enqueue(obj);
            _activeObjects[returnObj.PoolKey].Remove(obj);
        }

        public bool Has(string key)
        {
            return _poolVos.ContainsKey(key);
        }
    }
}