using BeatSaberMarkupLanguage.MenuButtons;
using BGLib.Polyglot;
using HMUI;
using IPA.Logging;
using Zenject;

namespace KeyRemapper.UI;

internal class MenuFlowCoordinator : FlowCoordinator
{
    [Inject]
    private readonly Logger _logger = null!;
    
    [Inject]
    private readonly SettingsViewController _view = null!;
    
    [Inject]
    private readonly MainFlowCoordinator _mainFlowCoordinator = null!;
    
    [Inject]
    private readonly MenuButtons _menuButtons = null!;

    private readonly MenuButton _menuButton;

    public MenuFlowCoordinator() => _menuButton = new MenuButton(Localization.Get("KEYREMAPPER_TITTLE"),
        Localization.Get("KEYREMAPPER_MENU_BUTTON_HINT"), OnMenuButtonClick);

    private void Start()
    {
        _menuButtons.RegisterButton(_menuButton);
        _logger.Info("Key Remapper button registered.");
    }

    protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
    {
        if (firstActivation)
        {
            SetTitle(Localization.Get("KEYREMAPPER_TITTLE"));
            showBackButton = true;
            ProvideInitialViewControllers(_view);
        }
    }
    
    private void OnMenuButtonClick()
    {
        _mainFlowCoordinator.PresentFlowCoordinator(this);
    }

    protected override void BackButtonWasPressed(ViewController topViewController)
    {
        base.BackButtonWasPressed(topViewController);
        _mainFlowCoordinator.DismissFlowCoordinator(this);
    }
}