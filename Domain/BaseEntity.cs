using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceManager.Domain
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
