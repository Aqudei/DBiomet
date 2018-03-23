using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biomet.Models.Entities
{
    public abstract class EntityBase : IEquatable<EntityBase>
    {

        public int Id { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as EntityBase);
        }

        public bool Equals(EntityBase other)
        {
            return other != null &&
                   Id == other.Id;
        }

        public override int GetHashCode()
        {
            return 2108858624 + Id.GetHashCode();
        }

        public static bool operator ==(EntityBase base1, EntityBase base2)
        {
            return EqualityComparer<EntityBase>.Default.Equals(base1, base2);
        }

        public static bool operator !=(EntityBase base1, EntityBase base2)
        {
            return !(base1 == base2);
        }
    }
}
