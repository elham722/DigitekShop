using System;

namespace DigitekShop.Domain.Entities.Common
{
    public abstract class BaseEntity
    {
        public int Id { get;  set; }
        public DateTime CreatedAt { get;  set; }
        public DateTime? UpdatedAt { get;  set; }
        public bool IsDeleted { get;  set; }
        public DateTime? DeletedAt { get;  set; }

        protected BaseEntity()
        {
            CreatedAt = DateTime.UtcNow;
            IsDeleted = false;
        }

        protected void SetUpdated()
        {
            UpdatedAt = DateTime.UtcNow;
        }

        public virtual void Delete()
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            SetUpdated();
        }

        public virtual void Restore()
        {
            IsDeleted = false;
            DeletedAt = null;
            SetUpdated();
        }

        public bool IsActive() => !IsDeleted;
    }
}
