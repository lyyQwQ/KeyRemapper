using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using KeyRemapper.UI.BindingTabs;
using Zenject;

namespace KeyRemapper.UI;

[HotReload(RelativePathToLayout = @"Views\SettingsView.bsml")]
[ViewDefinition("KeyRemapper.UI.Views.SettingsView.bsml")]
internal class SettingsViewController : BSMLAutomaticViewController
{
    [Inject]
    [UIValue("PauseTab")]
    public PauseTab PauseTab { get; private set; } = null!;

    [Inject]
    [UIValue("RestartTab")]
    public RestartTab RestartTab { get; private set; } = null!;
}
