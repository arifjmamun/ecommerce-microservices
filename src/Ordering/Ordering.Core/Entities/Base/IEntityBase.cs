namespace Ordering.Core.Entities.Base
{
    public interface IEntityBase<Tid>
    {
        Tid Id { get; }
    }
}