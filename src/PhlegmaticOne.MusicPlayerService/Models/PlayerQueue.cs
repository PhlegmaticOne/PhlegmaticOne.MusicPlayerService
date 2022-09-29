using System.Collections;

namespace PhlegmaticOne.MusicPlayerService.Models;

internal class PlayerQueue<T> : ICollection<T> where T : class
{
    private readonly List<T> _entities;
    private int _currentSongIndex;
    private bool _isQueueOver;
    internal PlayerQueue()
    {
        _entities = new();
        RepeatType = RepeatType.RepeatOff;
        ShuffleType = ShuffleType.ShuffleOff;
    }

    public int Count => _entities.Count;
    public bool IsReadOnly => false;
    internal RepeatType RepeatType { get; set; }
    internal ShuffleType ShuffleType { get; set; }
    internal T? Current
    {
        get => _isQueueOver ? null : _entities[_currentSongIndex];
        set
        {
            var index = _entities.IndexOf(value);
            if (index != -1)
            {
                _currentSongIndex = index;
            }
        }
    }
    public void Add(T entity) => _entities.Add(entity);

    public void AddRange(IEnumerable<T> entities)
    {
        var collection = entities.ToList();
        _entities.AddRange(collection);
    }

    public bool Remove(T entity)
    {
        var songIndex = _entities.IndexOf(entity);

        if (songIndex == -1)
        {
            return false;
        }

        if (songIndex < _currentSongIndex)
        {
            _currentSongIndex--;
        }
        _entities.RemoveAt(songIndex);
        return true;
    }
    public bool Contains(T item) => _entities.Contains(item);
    public void Clear() => _entities.Clear();
    public IEnumerator<T> GetEnumerator() => _entities.GetEnumerator();

    internal void MoveNext(QueueMoveType queueMoveType)
    {
        if (RepeatType == RepeatType.RepeatSong && queueMoveType == QueueMoveType.AccordingToRepeatType)
        {
            return;
        }
        if (ShuffleType == ShuffleType.ShuffleOn)
        {
            SetRandomIndex();
            return;
        }
        switch (RepeatType)
        {
            case RepeatType.RepeatOff:
                {
                    IncreaseQueueIndex();
                    break;
                }
            case RepeatType.RepeatQueue:
                {
                    IncreaseIndexAnyway();
                    break;
                }
            case RepeatType.RepeatSong:
                {
                    IncreaseIndexAnyway();
                    break;
                }
        }
    }

    internal void MovePrevious()
    {
        if (ShuffleType == ShuffleType.ShuffleOn)
        {
            SetRandomIndex();
            return;
        }
        _currentSongIndex = _currentSongIndex == 0 ? 0 : --_currentSongIndex;
    }

    private void IncreaseIndexAnyway()
    {
        IncreaseQueueIndex();
        if (_isQueueOver)
        {
            _currentSongIndex = 0;
            _isQueueOver = false;
        }
    }

    private void SetRandomIndex() => _currentSongIndex = Random.Shared.Next(0, _entities.Count);

    private void IncreaseQueueIndex()
    {
        if (_currentSongIndex < _entities.Count)
        {
            _isQueueOver = false;
        }

        if (_isQueueOver == false)
        {
            ++_currentSongIndex;
        }

        if (_currentSongIndex == _entities.Count)
        {
            _isQueueOver = true;
        }
    }



    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_entities).GetEnumerator();
    public void CopyTo(T[] array, int arrayIndex) => throw new NotImplementedException();
}