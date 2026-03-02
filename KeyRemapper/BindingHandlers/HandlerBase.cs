using System;
using KeyRemapper.Configuration;
using KeyRemapper.InputManagers;
using SiraUtil.Logging;
using Zenject;

namespace KeyRemapper.BindingHandlers;

internal abstract class HandlerBase : IInitializable, IDisposable
{
    [Inject]
    private readonly IInputManager _inputManager = null!;

    protected abstract ActionBinding BindingConfig { get; }

    protected abstract void Handle();

    void IInitializable.Initialize()
    {
        _inputManager.ButtonPressed += OnButtonPressed;
    }

    void IDisposable.Dispose()
    {
        _inputManager.ButtonPressed -= OnButtonPressed;
    }

    private void OnButtonPressed(ControllerButton button)
    {
        if (BindingConfig.Contains(button))
        {
            Handle();
        }
    }
}