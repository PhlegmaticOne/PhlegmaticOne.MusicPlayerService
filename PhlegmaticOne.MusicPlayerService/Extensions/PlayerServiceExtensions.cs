using Microsoft.Extensions.DependencyInjection;
using PhlegmaticOne.MusicPlayers.Base;
using PhlegmaticOne.MusicPlayerService.Base;
using PhlegmaticOne.MusicPlayerService.Services;

namespace PhlegmaticOne.MusicPlayerService.Extensions;

public static class PlayerServiceExtensions
{
    /// <summary>
    /// Register player service in DI container
    /// </summary>
    /// <typeparam name="T">Type that has path to itself</typeparam>
    /// <param name="serviceCollection">DI container</param>
    /// <returns>Next registering player service configuration</returns>
    public static PlayerServiceExtensionsHelper AddPlayerService<T>(this IServiceCollection serviceCollection) where T : class, IHaveUrl
    {
        serviceCollection.AddSingleton<IPlayerService<T>, PlayerService<T>>();
        return new PlayerServiceExtensionsHelper(serviceCollection);
    }
}

public class PlayerServiceExtensionsHelper
{
    private readonly IServiceCollection _serviceCollection;

    public PlayerServiceExtensionsHelper(IServiceCollection serviceCollection) =>
        _serviceCollection = serviceCollection;
    /// <summary>
    /// Specifies Player type to use in player service
    /// </summary>
    /// <typeparam name="T">Player type</typeparam>
    /// <returns>DI container with registered player service</returns>
    public IServiceCollection UsingPlayer<T>() where T : class, IPlayer
    {
        _serviceCollection.AddSingleton<IPlayer, T>();
        return _serviceCollection;
    }
}
