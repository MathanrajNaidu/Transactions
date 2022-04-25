using Domain.Interfaces;
using Domain.Transactions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private ApplicationDbContext context;

        public TransactionRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<Transaction> GetTransactions()
        {
            return context.Transactions.ToList();
        }

        public Transaction GetTransactionByID(int id)
        {
            return context.Transactions.Find(id);
        }

        public void InsertTransaction(Transaction student)
        {
            context.Transactions.Add(student);
        }

        public void DeleteTransaction(int studentID)
        {
            Transaction student = context.Transactions.Find(studentID);
            context.Transactions.Remove(student);
        }

        public void UpdateTransaction(Transaction student)
        {
            context.Entry(student).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
