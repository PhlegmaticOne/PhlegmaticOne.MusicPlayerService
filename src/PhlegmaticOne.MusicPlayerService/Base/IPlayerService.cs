using PhlegmaticOne.MusicPlayers.Base;
using PhlegmaticOne.MusicPlayerService.Models;

namespace PhlegmaticOne.MusicPlayerService.Base;

/// <summary>
/// Contract for player services
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IPlayerService<T> : ICollection<T>, IDisposable where T : class, IHaveUrl
{
    /// <summary>
    /// Invoked when entity in player service changed
    /// </summary>
    event EventHandler<T> CurrentEntityChanged;
    /// <summary>
    /// Invoked when entities in plyer service changed
    /// </summary>
    event EventHandler<PlayerQueueChangedEventArgs<T>> EntitiesChanged;

    /// <summary>
    /// Repeat tracks type
    /// </summary>
    RepeatType RepeatType { get; set; }
    /// <summary>
    /// Shuffle tracks type
    /// </summary>
    ShuffleType ShuffleType { get; set; }
    /// <summary>
    /// Current entity in player service
    /// </summary>
    T? CurrentEntityInPlayer { get; set; }
    /// <summary>
    /// Player
    /// </summary>
    IPlayer Player { get; }

    /// <summary>
    /// Sets and plays file
    /// </summary>
    /// <param name="entity">File containing path</param>
    void SetAndPlay(T entity);
    /// <summary>
    /// Moves to next track according to QueueMoveType
    /// </summary>
    /// <param name="queueMoveType"></param>
    void MoveNext(QueueMoveType queueMoveType);
    /// <summary>
    /// Moves to previous track
    /// </summary>
    void MovePrevious();
    /// <summary>
    /// Adds range of entities in player service
    /// </summary>
    /// <param name="entities"></param>
    void AddRange(IEnumerable<T> entities);
}