using Exiled.Events.EventArgs.Server;
using ExiledKitsPlugin.Classes;

namespace ExiledKitsPlugin.Handlers;

public class RoundRestartHandle
{
    public void OnRoundEnded(RoundEndedEventArgs roundEndedEventArgs)
    {
        if (Plugin.Instance.Config.ResetCooldownsOnRoundEnd)
        {
            Plugin.Instance.KitCooldownManager = new KitCooldownManager();
        }
    }
}