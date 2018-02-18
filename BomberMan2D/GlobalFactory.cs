using BehaviourEngine;
using BomberMan2D.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMan2D
{
    public static class GlobalFactory
    {
        private static Dictionary<Type, Pool> pools;
        static GlobalFactory()
        {
            pools = new Dictionary<Type, Pool>();
        }

        public static void RegisterPool(Type type, Func<IPoolable> allocator)
        {
            if (pools.ContainsKey(type))
                throw new Exception("Pool already registered");

            pools.Add(type, new Pool(allocator));
        }

        public static IPoolable Get(Type type)
        {
            if (!pools.ContainsKey(type))
                throw new Exception("Pool isn't registered");
            return pools[type].Get();
        }

        public static void Recycle(Type type, IPoolable toRecycle)
        {
            if (!pools.ContainsKey(type))
                throw new Exception("Pool isn't registered");

            pools[type].Recycle(toRecycle);
        }

        private class Pool
        {
            private Queue<IPoolable> instances;
            private Func<IPoolable> allocator;
            public Pool(Func<IPoolable> allocator)
            {
                instances = new Queue<IPoolable>();
                this.allocator = allocator;
            }

            public IPoolable Get()
            {
                IPoolable toReturn;
                if (instances.Count == 0)
                {
                    toReturn = allocator.Invoke();
                }
                else
                {
                    toReturn = instances.Dequeue();
                }
                toReturn.OnGet();
                return toReturn;
            }

            public void Recycle(IPoolable toRecycle)
            {
                toRecycle.OnRecycle();
                instances.Enqueue(toRecycle);
            }
        }
    }
}
