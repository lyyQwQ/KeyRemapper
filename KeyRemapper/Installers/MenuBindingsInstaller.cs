using KeyRemapper.UI;
using KeyRemapper.UI.BindingTabs;
using Zenject;

namespace KeyRemapper.Installers;

public class MenuBindingsInstaller : Installer
{
    public override void InstallBindings()
    {
        // 绑定 UI 组件
        Container.BindInterfacesAndSelfTo<MenuFlowCoordinator>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<SettingsViewController>().FromNewComponentAsViewController().AsSingle();
        Container.BindInterfacesAndSelfTo<PauseTab>().FromNewComponentOnNewGameObject().AsSingle()
            .WhenInjectedInto<SettingsViewController>();
        Container.BindInterfacesAndSelfTo<RestartTab>().FromNewComponentOnNewGameObject().AsSingle()
            .WhenInjectedInto<SettingsViewController>();
    }
}