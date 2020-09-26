using BC11.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BC11.Entities
{
    public interface ITransactionPool<T> where T : ITransaction
    {
        void AddTransaction(T transaction);
        T GetTransaction();
    }

    public class TransactionPool<T> : ITransactionPool<T> where T : ITransaction
    {
        private readonly Queue<T> _queue;

        public TransactionPool()
        {
            _queue = new Queue<T>();
        }

        public void AddTransaction(T transaction)
        {
            _queue.Enqueue(transaction);
        }

        public T GetTransaction()
        {
            return _queue.Dequeue();
        }
    }
}
