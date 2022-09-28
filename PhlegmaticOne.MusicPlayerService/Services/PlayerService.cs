using System.Collections;
using PhlegmaticOne.MusicPlayers.Base;
using PhlegmaticOne.MusicPlayerService.Base;
using PhlegmaticOne.MusicPlayerService.Models;

namespace PhlegmaticOne.MusicPlayerService.Services;

internal class PlayerService<T> : IPlayerService<T> where T : class, IHaveUrl
{
    private T? _currentEntity;
    private readonly PlayerQueue<T> _playerQueue;

    public PlayerService(IPlayer player)
    {
        Player = player;
        Player.SongEnded += PlayerOnSongEnded;
        _playerQueue = new();
    }

    public event EventHandler<T>? CurrentEntityChanged;
    public event EventHandler<PlayerQueueChangedEventArgs<T>>? EntitiesChanged;

    public int Count => _playerQueue.Count;
    public bool IsReadOnly => false;
    public RepeatType RepeatType { get => _playerQueue.RepeatType; set => _playerQueue.RepeatType = value; }
    public ShuffleType ShuffleType { get => _playerQueue.ShuffleType; set => _playerQueue.ShuffleType = value; }

    public T? CurrentEntityInPlayer
    {
        get => _currentEntity;
        set
        {
            _currentEntity = value;
            InvokeOnEntityChanged();
        }
    }

    public IPlayer Player { get; }
    public void SetAndPlay(T? entity)
    {
        CurrentEntityInPlayer = entity;

        if (entity is not null)
        {
            _playerQueue.Current = entity;
            Player.Stop();
            Player.Play(ChooseFilePath(entity));
        }
    }

    public void MoveNext(QueueMoveType queueMoveType)
    {
        if (_playerQueue.Any())
        {
            _playerQueue.MoveNext(queueMoveType);
            SetAndPlay(_playerQueue.Current);
        }
    }

    public void MovePrevious()
    {
        if (_playerQueue.Any())
        {
            _playerQueue.MovePrevious();
            SetAndPlay(_playerQueue.Current);
        }
    }

    public void Add(T item)
    {
        _playerQueue.Add(item);
        InvokeOnQueueChanged(new[] { item }, PlayerQueueChangedType.Added);
    }
    public void AddRange(IEnumerable<T> entities)
    {
        var haveUrls = entities.ToList();
        _playerQueue.AddRange(haveUrls);
        InvokeOnQueueChanged(haveUrls, PlayerQueueChangedType.Added);
    }

    public void Clear()
    {
        InvokeOnQueueChanged(_playerQueue, PlayerQueueChangedType.Removed);
        _playerQueue.Clear();
    }

    public bool Contains(T item) => _playerQueue.Contains(item);

    public bool Remove(T item)
    {
        var isRemoved = _playerQueue.Remove(item);

        if (isRemoved)
        {
            InvokeOnQueueChanged(new[] { item }, PlayerQueueChangedType.Removed);
        }

        return isRemoved;
    }
    public void CopyTo(T[] array, int arrayIndex) => _playerQueue.CopyTo(array, arrayIndex);

    public void Dispose()
    {
        Player.SongEnded -= PlayerOnSongEnded;
        Player.Dispose();
    }

    public IEnumerator<T> GetEnumerator() => _playerQueue.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_playerQueue).GetEnumerator();
    private static string ChooseFilePath(T entity) =>
        string.IsNullOrEmpty(entity.LocalUrl) ? entity.OnlineUrl : entity.LocalUrl;

    private void InvokeOnQueueChanged(IEnumerable<T> entities, PlayerQueueChangedType collectionChangedType) =>
        EntitiesChanged?.Invoke(this, new PlayerQueueChangedEventArgs<T>(entities, collectionChangedType));
    private void InvokeOnEntityChanged() => CurrentEntityChanged?.Invoke(this, _currentEntity);
    private void PlayerOnSongEnded(object? sender, EventArgs e) => MoveNext(QueueMoveType.AccordingToRepeatType);
}