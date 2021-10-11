using System;

namespace Business.Models
{
    public abstract class Entity
    {
        protected Entity()
        {
            Id = Guid.NewGuid();
            Active = true;
        }
        public Guid Id { get; set; }

        public bool Active { get; set; }
        public abstract bool IsActive();
    }
}