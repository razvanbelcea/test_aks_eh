using System;

namespace eathappy.order.domain.Types
{
    public abstract class BaseEntity<TKey> : IBaseEntity
    where TKey : IEquatable<TKey>
    {
        public virtual TKey Id { get; set; }
        object IBaseEntity.Id
        {
            get => Id;
            set => Id = (TKey)Convert.ChangeType(value, typeof(TKey));
        }
    }
}
