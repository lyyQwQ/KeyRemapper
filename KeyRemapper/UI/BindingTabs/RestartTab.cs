using KeyRemapper.Configuration;

namespace KeyRemapper.UI.BindingTabs;

internal class RestartTab : BindingTabBase
{
    protected override ActionBinding Action => Config.Actions.Restart;
}