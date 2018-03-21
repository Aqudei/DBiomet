using Biomet.Models.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biomet.Repositories
{
    public abstract class RepositoryBase
    {
        protected readonly BiometContext Context;

        protected RepositoryBase(BiometContext context)
        {
            Context = context;
        }
    }
}
