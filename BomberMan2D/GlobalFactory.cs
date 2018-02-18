using BehaviourEngine;
using BomberMan2D.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMan2D
{
    public static class GlobalFactory<T>
    {
        private static Dictionary<Type, Pool<T>> pools;
        static GlobalFactory()
        {
            pools = new Dictionary<Type, Pool<T>>();
        }

        public static void RegisterPool(Type type, Func<T> allocator)
        {
            if (pools.ContainsKey(type))
                throw new Exception("Pool already registered");

            pools.Add(type, new Pool<T>(allocator));
        }

        public static T Get(Type type)
        {
            if (!pools.ContainsKey(type))
                throw new Exception("Pool isn't registered");
            return pools[type].Get();
        }

        public static void Recycle(Type type, T toRecycle)
        {
            if (!pools.ContainsKey(type))
                throw new Exception("Pool isn't registered");

            pools[type].Recycle(toRecycle);
        }

        private class Pool<T>
        {
            private Queue<T> instances;
            private Func<T> allocator;
            public Pool(Func<T> allocator)
            {
                instances = new Queue<T>();
                this.allocator = allocator;
            }

            public T Get(Action<T> onGet = null)
            {
                if (instances == null)
                    throw new Exception("Pool is not registered");

                T toReturn = instances.Count == 0 ? allocator.Invoke() : instances.Dequeue();

                onGet?.Invoke(toReturn);
                return toReturn;
            }

            public void Recycle(T toRecycle, Action<T> onRecycle = null)
            {
                if (instances == null)
                    throw new Exception("Pool is not registered");

                onRecycle?.Invoke(toRecycle);
                instances.Enqueue(toRecycle);
            }
        }
    }
}
