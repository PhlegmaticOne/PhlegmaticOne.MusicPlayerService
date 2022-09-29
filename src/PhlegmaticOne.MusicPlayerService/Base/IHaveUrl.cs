namespace PhlegmaticOne.MusicPlayerService.Base;

/// <summary>
/// Contract for types that have path to it
/// </summary>
public interface IHaveUrl
{
    string LocalUrl { get; }
    string OnlineUrl { get; }
}