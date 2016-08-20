using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace AutoFlushQueue
{
    public class AutoFlushQueue
    {
        private System.Timers.Timer _timer;
        private bool _isFull = false;
        private int _autoFlushIntervalInSeconds;
        private int _autoFlushMaximumSize;
        public Queue<int> Queue { get; set; }
        private Queue<int> _bufferQueue;
        public Action<List<int>> OnQueueIsFull { get; set; }

        public bool IsFull
        {
            get
            {
                return _isFull;
            }
            private set { PerformActionOnQueueFull(value); }
        }
        private void PerformActionOnQueueFull(bool value)
        {
            if (value)
            {
                var _allValuesFromQueue = Queue.ToList<int>();
                Queue.Clear();
                while (_bufferQueue.Count > 0)
                {
                    Enqueue(_bufferQueue.Dequeue());
                }
                OnQueueIsFull(_allValuesFromQueue);
            }
        }

        public AutoFlushQueue(int autoFlushMaximumSize, int autoFlushInterval)
        {
            _autoFlushMaximumSize = autoFlushMaximumSize;
            Queue = new Queue<int>();
            _bufferQueue = new Queue<int>();
            IsFull = false;
            _autoFlushIntervalInSeconds = autoFlushInterval;
            SetUpTimer();
        }

        private void SetUpTimer()
        {
            _timer = new System.Timers.Timer(_autoFlushIntervalInSeconds*1000);
            _timer.AutoReset = true;
            _timer.Elapsed += HandleTimer;
            _timer.Enabled = true;
        }

        private void HandleTimer(Object source, ElapsedEventArgs e)
        {
            IsFull = true;
            Console.WriteLine("In timer");
        }

        public void Enqueue(int number)
        {
            if (IsFull)
                _bufferQueue.Enqueue(number);
            else
            {
                Queue.Enqueue(number);
                if (Queue.Count == _autoFlushMaximumSize)
                {
                    IsFull = true;
                }
            }
        }
    }
}
