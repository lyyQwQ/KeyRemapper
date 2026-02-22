using BeatSaberMarkupLanguage.Attributes;
using KeyRemapper.Configuration;

namespace KeyRemapper.UI.BindingTabs;

internal class PauseTab : BindingTabBase
{
    private PauseBinding PauseAction => Config.Actions.Pause;

    protected override ActionBinding Action => PauseAction;

    [UIValue("BlockBuiltIn")]
    public bool BlockBuiltIn
    {
        get => PauseAction.BlockBuiltIn;
        set => PauseAction.BlockBuiltIn = value;
    }
}