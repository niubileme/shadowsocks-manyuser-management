using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SSMM.Cache
{
    public abstract class BaseCache<Key, T>
    {
        protected static object synObj = new object();
        private static ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();

        public Dictionary<Key, T> innerData = null;
        public Queue<Key> queue = new Queue<Key>();
        public int MaxNum = 10000;
        public BaseCache()
        {
            Load();
        }
        /// <summary>
        /// 加载Cache数据
        /// </summary>
        public void Load()
        {
            Dictionary<Key, T> temp = Init();
            lock (synObj)
            {
                innerData = temp;
                foreach (var item in temp)
                {
                    queue.Enqueue(item.Key);
                }
            }
        }
        /// <summary>
        /// 初始化数据
        /// </summary>
        public abstract Dictionary<Key, T> Init();

        /// <summary>
        /// 获取Cache数据
        /// </summary>
        private T GetCacheValue(Key key)
        {
            T result = default(T);
            try
            {
                rwLock.EnterReadLock();
                innerData.TryGetValue(key, out result);
            }
            finally
            {
                rwLock.ExitReadLock();
            }
            return result;
        }

        /// <summary>
        /// 获取Cache数据,不存在则自动去后台获取最新数据
        /// </summary>
        public T GetValue(Key key)
        {
            T result = GetCacheValue(key);
            if (result == null)
            {
                result = GetItem(key);
                if (result != null)
                {
                    UpdateValue(key, result);
                }
            }
            return result;
        }
        public abstract T GetItem(Key key);

        /// <summary>
        /// 不读Cache,直接获取后台最新数据
        /// </summary>
        public T GetOriginalValue(Key key)
        {
            return GetItem(key);
        }

        /// <summary>
        /// 添加或更新
        /// </summary>
        public void UpdateValue(Key key, T item)
        {
            rwLock.EnterWriteLock();
            try
            {
                if (innerData.ContainsKey(key))
                {
                    innerData[key] = item;
                }
                else
                {
                    if (queue.Count >= MaxNum)
                    {
                        var k = queue.Dequeue();
                        innerData.Remove(k);
                    }
                    innerData.Add(key, item);
                    queue.Enqueue(key);
                }
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 更新指定Cache
        /// </summary>
        /// <param name="key"></param>
        public T UpdateCacheValue(Key key)
        {
            T result = GetItem(key);
            if (result != null)
            {
                UpdateValue(key, result);
            }
            return result;
        }


        public int GetCount()
        {
            return innerData.Count;
        }


    }
}
