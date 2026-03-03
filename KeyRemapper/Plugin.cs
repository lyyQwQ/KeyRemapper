using HarmonyLib;
using IPA;
using KeyRemapper.Configuration;
using KeyRemapper.Installers;
using SiraUtil.Zenject;
using IPALogger = IPA.Logging.Logger;

namespace KeyRemapper
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    [NoEnableDisable]
    public class Plugin
    {
        internal static IPALogger Log { get; private set; } = null!;

        private readonly Harmony _harmony = new("com.github.lyyQwQ.KeyRemapper");

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
            _harmony.PatchAll();
            Log.Info("KeyRemapper initialized.");
        }
    }
}
