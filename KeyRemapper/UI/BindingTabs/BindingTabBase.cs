using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Components.Settings;
using BeatSaberMarkupLanguage.Parser;
using BGLib.Polyglot;
using KeyRemapper.Configuration;
using UnityEngine;
using Zenject;
using Logger = IPA.Logging.Logger;

namespace KeyRemapper.UI.BindingTabs;

internal abstract class BindingTabBase : MonoBehaviour, INotifyPropertyChanged
{
    private const string VIEW_DEFINITION = "KeyRemapper.UI.Views.BindingTab.bsml";
    
    [Inject]
    private readonly Logger _logger = null!;

    [Inject]
    protected readonly PluginConfig Config = null!;

    [Inject]
    private readonly BSMLParser _parser = null!;

    protected abstract ActionBinding Action { get; }

    protected bool Parsed { get; private set; }

    [UIComponent("BindingTable")]
    public CustomCellListTableData BindingTable { get; set; } = null!;

    [UIComponent("ButtonsDropDown")]
    public DropDownListSetting ButtonsDropDown { get; set; } = null!;

    [UIValue("ComponentRoot")]
    public Transform ComponentRoot => transform;

    [UIValue("ShowBlockBuiltIn")]
    public virtual bool ShowBlockBuiltIn => false;

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

    public BSMLParserParams CreateView(Transform parent)
    {
        transform.SetParent(parent, false);
        var bsml = Utilities.GetResourceContent(Assembly.GetExecutingAssembly(), VIEW_DEFINITION);
        return _parser.Parse(bsml, gameObject, this);
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
        ControllerButton.L_X => Localization.Get("KEYREMAPPER_CONTROLLER_BUTTON_L_X"),
        ControllerButton.L_Y => Localization.Get("KEYREMAPPER_CONTROLLER_BUTTON_L_Y"),
        ControllerButton.R_A => Localization.Get("KEYREMAPPER_CONTROLLER_BUTTON_R_A"),
        ControllerButton.R_B => Localization.Get("KEYREMAPPER_CONTROLLER_BUTTON_R_B"),
        ControllerButton.L_Grip => Localization.Get("KEYREMAPPER_CONTROLLER_BUTTON_L_GRIP"),
        ControllerButton.R_Grip => Localization.Get("KEYREMAPPER_CONTROLLER_BUTTON_R_GRIP"),
        ControllerButton.L_Trigger => Localization.Get("KEYREMAPPER_CONTROLLER_BUTTON_L_TRIGGER"),
        ControllerButton.R_Trigger => Localization.Get("KEYREMAPPER_CONTROLLER_BUTTON_R_TRIGGER"),
        ControllerButton.L_Stick => Localization.Get("KEYREMAPPER_CONTROLLER_BUTTON_L_STICK"),
        ControllerButton.R_Stick => Localization.Get("KEYREMAPPER_CONTROLLER_BUTTON_R_STICK"),
        ControllerButton.L_Menu => Localization.Get("KEYREMAPPER_CONTROLLER_BUTTON_L_MENU"),
        ControllerButton.R_Menu => Localization.Get("KEYREMAPPER_CONTROLLER_BUTTON_R_MENU"),
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