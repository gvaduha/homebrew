using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace gvaduha.Framework
{
    /// <summary>
    /// Called to process item
    /// </summary>
    /// <typeparam name="TItem">type of item</typeparam>
    public interface IItemProcessor<in TItem>
    {
        void Process(TItem item);
    }

    /// <summary>
    /// Process item by item using queue as input sync
    /// </summary>
    /// <typeparam name="TItem">type of processing item</typeparam>
    /// <typeparam name="TProcessor">class implementing IItemProcessor</typeparam>
    public class QueueBasedItemProcessor<TItem, TProcessor> : IDisposable where TProcessor : IItemProcessor<TItem>
    {
        private readonly ConcurrentQueue<TItem> _processingQueue = new ConcurrentQueue<TItem>();
        private readonly AutoResetEvent _runProcessSignal = new AutoResetEvent(false);
        private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();
        private readonly IItemProcessor<TItem> _processor;

        public QueueBasedItemProcessor(IItemProcessor<TItem> processor)
        {
            _processor = processor;

            Task.Run(() => ProcessLoop());
        }

        public void EnqueueItem(TItem item)
        {
            _processingQueue.Enqueue(item);
            _runProcessSignal.Set();
        }

        public int QueueSize => _processingQueue.Count;

        private void ProcessLoop()
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                _runProcessSignal.WaitOne();

                while (_processingQueue.TryDequeue(out var item))
                {
                    _processor.Process(item);
                }
            }
        }

        ~QueueBasedItemProcessor()
        {
            Dispose();
        }

        public void Dispose()
        {
            _cancellationToken.Cancel();
            _cancellationToken.Dispose();
            _runProcessSignal.Dispose();
        }
    }

    //public class QueueBasedItemProcessorTest
    //{
    //    class Sut : IItemProcessor<int>
    //    {
    //        private readonly List<int> _result = new List<int>();
    //        private readonly Random _rnd = new Random();

    //        public void Process(int item)
    //        {
    //            _result.Add(item);
    //            Thread.Sleep(_rnd.Next(100,1000));
    //        }
    //        public IEnumerable<int> GetResult() => _result;
    //    }

    //    [Theory]
    //    [InlineData(new int[] {0,1,2,3,4,5})]
    //    public void NotOrderedProcessAll(int[] data)
    //    {
    //        var sut = new Sut();
    //        var processor = new QueueBasedItemProcessor<int, Sut>(sut);

    //        Parallel.ForEach(data.ToList().AsParallel(),
    //            x =>
    //            {
    //                processor.EnqueueItem(x);
    //            });

    //        while (processor.QueueSize != 0)
    //            Thread.Sleep(1000);

    //        Assert.Equal(new HashSet<int>(data), new HashSet<int>(sut.GetResult()));
    //    }

    //    [Theory]
    //    [InlineData(new int[] {0,1,2,3,4,5})]
    //    public void OrderedProcessAll(int[] data)
    //    {
    //        var sut = new Sut();
    //        var processor = new QueueBasedItemProcessor<int, Sut>(sut);

    //        foreach (var x in data)
    //            processor.EnqueueItem(x);

    //        while (processor.QueueSize != 0)
    //            Thread.Sleep(1000);

    //        Assert.Equal(data.ToList(), sut.GetResult());
    //    }
    //}
}
