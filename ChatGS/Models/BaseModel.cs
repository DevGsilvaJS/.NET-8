using System.Collections.Generic;

namespace ChatGS.Models
{
    public class BaseModel<T>
    {
        public List<T> _entitiesToSave = new List<T>();

        public void AddListaSalvar(T entity)
        {
            _entitiesToSave.Add(entity);
        }

        public IEnumerable<T> GetEntitiesToSave()
        {
            return _entitiesToSave;
        }

        public void ClearEntitiesToSave()
        {
            _entitiesToSave.Clear();
        }
    }
}
