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
    private readonly PauseTab PauseTab = null!;

    [Inject]
    [UIValue("RestartTab")]
    private readonly RestartTab RestartTab = null!;

    protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
    {
        if (firstActivation)
        {
            // 延迟加载子视图
            PauseTab.CreateView(transform);
            RestartTab.CreateView(transform);
        }

        base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);
    }
}
