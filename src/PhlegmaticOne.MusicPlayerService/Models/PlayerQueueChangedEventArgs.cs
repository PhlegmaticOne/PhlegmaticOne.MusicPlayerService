namespace PhlegmaticOne.MusicPlayerService.Models;

/// <summary>
/// Playe queue changed event args
/// </summary>
public class PlayerQueueChangedEventArgs<T> : EventArgs where T : class
{
    public IEnumerable<T> Entities { get; }
    public PlayerQueueChangedType CollectionChangedType { get; }

    public PlayerQueueChangedEventArgs(IEnumerable<T> entities, PlayerQueueChangedType collectionChangedType)
    {
        Entities = entities;
        CollectionChangedType = collectionChangedType;
    }
}