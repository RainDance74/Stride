namespace Stride.Domain.Entities;

public class IUser<TKey> : IHasKey<TKey?>
{
    TKey? IHasKey<TKey?>.Id { get; set; }
}
