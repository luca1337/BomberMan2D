using System;
using System.Collections.Generic;

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

        private class Pool<K>
        {
            private Queue<K> instances;
            private Func<K> allocator;
            public Pool(Func<K> allocator)
            {
                instances = new Queue<K>();
                this.allocator = allocator;
            }

            public K Get(Action<K> onGet = null)
            {
                if (instances == null)
                    throw new Exception("Pool is not registered");

                K toReturn = instances.Count == 0 ? allocator.Invoke() : instances.Dequeue();

                onGet?.Invoke(toReturn);
                return toReturn;
            }

            public void Recycle(K toRecycle, Action<K> onRecycle = null)
            {
                if (instances == null)
                    throw new Exception("Pool is not registered");

                onRecycle?.Invoke(toRecycle);
                instances.Enqueue(toRecycle);
            }
        }
    }
}
