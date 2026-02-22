using IPA;
using SiraUtil.Zenject;
using IPALogger = IPA.Logging.Logger;
using KeyRemapper.Installers;
using KeyRemapper.Configuration;

namespace KeyRemapper
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    [NoEnableDisable]
    public class Plugin
    {
        internal static IPALogger Log { get; private set; } = null!;

        [Init]
        public Plugin(IPALogger logger, IPA.Config.Config conf, Zenjector zenjector)
        {
            Log = logger;
            var config = PluginConfig.Initialize(conf);
            logger.Debug("Config loaded.");
            zenjector.Install<AppInstaller>(Location.App, config);
            zenjector.Install<GameplayInstaller>(Location.Player);
            zenjector.Install<MenuBindingsInstaller>(Location.Menu);
            zenjector.UseLogger(logger);
            Log.Info("KeyRemapper initialized.");
        }
    }
}
