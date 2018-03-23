using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biomet.Events
{
    public class CrudEvent<T> where T : Models.Entities.EntityBase
    {
        public enum CrudActionEnum
        {
            Created, Deleted, Updated
        }

        public T Entity { get; set; }
        public CrudActionEnum CrudAction { get; set; }

        public CrudEvent(T entity, CrudActionEnum crudAction)
        {
            Entity = entity;
            CrudAction = crudAction;
        }
    }
}
