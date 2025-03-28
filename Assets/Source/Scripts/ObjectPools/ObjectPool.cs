using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Source.Scripts.ObjectPools
{
    public sealed class ObjectPool<T> where T : Object
    {
        private readonly List<T> _pool;
        private readonly T _prefab;

        public ObjectPool(T prefab)
        {
            _pool = new List<T>();
            _prefab = prefab;
        }
    
        public T Get()
        {
            var obj = _pool.FirstOrDefault(o => !o.GameObject().activeSelf);
            if (obj == null)
            {
                obj = GameObject.Instantiate(_prefab);
                _pool.Add(obj);
            }
        
            obj.GameObject().SetActive(true);
            return obj;
        }

        public Type GetObjectType()
        {
            var obj = _pool.FirstOrDefault();
            return obj != null ? obj.GetType() : null;
        }
    }
}