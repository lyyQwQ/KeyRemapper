using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Components.Settings;
using IPA.Logging;
using KeyRemapper.Configuration;
using Zenject;

namespace KeyRemapper.UI.BindingTabs;

internal abstract class BindingTabBase : INotifyPropertyChanged
{
    [Inject]
    private readonly Logger _logger = null!;

    [Inject]
    protected readonly PluginConfig Config = null!;

    protected abstract ActionBinding Action { get; }

    protected bool Parsed { get; private set; }

    [UIComponent("BindingTable")]
    public CustomCellListTableData BindingTable { get; set; }

    [UIComponent("ButtonsDropDown")]
    public DropDownListSetting ButtonsDropDown { get; set; }

    [UIValue("Bindings")]
    public List<TableCell> Bindings { get; } = [];

    [UIValue("AvailableButtons")]
    public List<ControllerButton> AvailableButtons = [];

    [UIValue("CanAddBinding")]
    public bool CanAddBinding => AvailableButtons.Count > 0;

    [UIValue("CannotAddBinding")]
    public bool CannotAddBinding => AvailableButtons.Count == 0;

    [UIValue("NoBindings")]
    public bool NoBindings => Action.Bindings.Count == 0;

    [UIValue("SelectedButton")]
    public ControllerButton SelectedButton { get; set; } = ControllerButton.L_X;

    [UIValue("Enable")]
    public bool Enable
    {
        get => Action.Enabled;
        set => Action.Enabled = value;
    }

    [UIAction("#post-parse")]
    protected virtual void OnParsed()
    {
        _logger.Trace("Parsed");
        Parsed = true;
        RefreshBindings();
    }

    [UIAction("AddBinding")]
    public void AddBinding()
    {
        _logger.Debug($"Adding binding {SelectedButton}");
        Action.AddBinding(SelectedButton);
        RefreshBindings();
    }

    private void DeleteBinding(ControllerButton button)
    {
        _logger.Debug($"Deleting binding {button}");
        Action.RemoveBinding(button);
        RefreshBindings();
    }

    private void RefreshBindings()
    {
        var bindings = Action.Bindings;
        Bindings.Clear();
        Bindings.Capacity = bindings.Count;
        foreach (var binding in bindings)
        {
            Bindings.Add(new TableCell(this, binding));
        }

        if (Parsed)
        {
            // refresh table
            BindingTable.TableView.ReloadDataKeepingPosition();

            // update dropdown
            AvailableButtons.Clear();
            foreach (ControllerButton button in Enum.GetValues(typeof(ControllerButton)))
            {
                if (!Action.Contains(button))
                {
                    AvailableButtons.Add(button);
                }
            }

            SelectedButton = CanAddBinding ? AvailableButtons[0] : ControllerButton.L_X;
            ButtonsDropDown.Values = AvailableButtons;
            ButtonsDropDown.UpdateChoices();
            ButtonsDropDown.ReceiveValue();
            ButtonsDropDown.Interactable = CanAddBinding;
            NotifyPropertyChanged(nameof(CanAddBinding));
            NotifyPropertyChanged(nameof(CannotAddBinding));
            NotifyPropertyChanged(nameof(NoBindings));
        }
    }

    public string FormatButton(ControllerButton button) => button switch
    {
        ControllerButton.L_X => "Left A/X",
        ControllerButton.L_Y => "Left B/Y",
        ControllerButton.R_A => "Right A/X",
        ControllerButton.R_B => "Right B/Y",
        ControllerButton.L_Grip => "Left Grip",
        ControllerButton.R_Grip => "Right Grip",
        ControllerButton.L_Trigger => "Left Trigger",
        ControllerButton.R_Trigger => "Right Trigger",
        ControllerButton.L_Stick => "Left Joystick",
        ControllerButton.R_Stick => "Right Joystick",
        ControllerButton.L_Menu => "Left Menu",
        ControllerButton.R_Menu => "Right Menu",
        _ => button.ToString()
    };

    internal class TableCell(BindingTabBase parent, ControllerButton button)
    {
        [UIValue("Button")]
        public string Button => parent.FormatButton(button);

        [UIAction("DeleteBinding")]
        public void DeleteBinding()
        {
            parent.DeleteBinding(button);
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}