using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace TerrainGen
{
    public class ThreadedDataRequester : MonoBehaviour
    {
        private static ThreadedDataRequester _instance;

        private readonly Queue<ThreadInfo> _dataQueue = new Queue<ThreadInfo>();

        private void Awake()
        {
            _instance = FindObjectOfType<ThreadedDataRequester>();

        }


        public static void RequestData(Func<object> generateData,  Action<object> callBack)
        {
            ThreadStart threadStart = delegate { _instance.DataThread(generateData, callBack); };
            new Thread(threadStart).Start();

        }

        private void DataThread(Func<object> generateData, Action<object> callBack)
        {
            object data = generateData();
            lock (_dataQueue)
            {
                _dataQueue.Enqueue(new ThreadInfo(callBack, data));

            }
        }

    

        private void Update()
        {
            lock (_dataQueue)
            {
                if (_dataQueue.Count <= 0) return;
                for (int i = 0; i < _dataQueue.Count; i++)
                {
                    ThreadInfo threadInfo = _dataQueue.Dequeue();
                    threadInfo.Callback(threadInfo.Parameter);
                }
            }
        }

        private struct ThreadInfo
        {
            public readonly Action<object> Callback;
            public readonly object Parameter;

            public ThreadInfo(Action<object> callback, object parameter)
            {
                Callback = callback;
                Parameter = parameter;
            }
        }
    }
}
