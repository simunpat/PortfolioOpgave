using System.Linq.Expressions;
using PortfolioOpgave.Interfaces;

namespace PortfolioOpgave.Services
{
    public class Service<T> : IService<T> where T : class
    {
        protected readonly IRepository<T> _repository;

        public Service(IRepository<T> repository)
        {
            _repository = repository;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _repository.GetAll();
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _repository.Find(predicate);
        }

        public virtual T GetById(int id)
        {
            return _repository.GetById(id);
        }

        public virtual void Add(T entity)
        {
            _repository.Add(entity);
        }

        public virtual void Update(T entity)
        {
            _repository.Update(entity);
        }

        public virtual void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}