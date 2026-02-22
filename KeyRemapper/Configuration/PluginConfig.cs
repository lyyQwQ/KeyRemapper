using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using IPA.Config.Stores;
using IPA.Config.Stores.Attributes;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]

namespace KeyRemapper.Configuration;

internal class PluginConfig
{
    private static PluginConfig? _instance;

    public static PluginConfig Initialize(IPA.Config.Config conf)
    {
        if (_instance != null) return _instance;
        // 生成或加载设置文件 → 存到静态 _instance
        _instance = conf.Generated<PluginConfig>();
        return _instance;
    }

    public virtual int Version { get; protected set; } = 2;

    public virtual ActionSettings Actions { get; protected set; } = new();

    public virtual void Changed()
    {
        Plugin.Log.Trace("PluginConfig Changed");
        // BSIPA 会重写此方法，调用时会触发保存
        // 设置重载时也会调用此方法
        Actions.Pause.Reloaded();
        Actions.Restart.Reloaded();
    }
}

internal class ActionSettings
{
    public virtual PauseBinding Pause { get; protected set; } = new();

    public virtual ActionBinding Restart { get; protected set; } = new();

    public void Reset()
    {
        Pause.Reset();
        Restart.Reset();
    }
}

internal class ActionBinding
{
    [UseConverter]
    [SerializedName("Bindings")]
    protected virtual HashSet<ControllerButton> BindingsInternal { get; set; } = [];

    public virtual bool Enabled { get; set; } = true;

    //创建只读防止外部修改
    //并防止设置重载时发生并发修改 （取决于 BSIPA 是否会生成新集合实例）
    [Ignore]
    public IReadOnlyList<ControllerButton> Bindings { get; private set; } = Array.Empty<ControllerButton>();

    public void AddBinding(ControllerButton binding)
    {
        BindingsInternal.Add(binding);
        Changed(); // 触发保存
    }

    public void RemoveBinding(ControllerButton binding)
    {
        BindingsInternal.Remove(binding);
        Changed(); // 触发保存
    }

    public virtual void Reset()
    {
        BindingsInternal.Clear();
        Changed(); // 触发保存
    }

    public virtual void SetBindings(IEnumerable<ControllerButton> bindings)
    {
        BindingsInternal.Clear();
        foreach (var binding in bindings)
            BindingsInternal.Add(binding);
        Changed(); // 触发保存
    }

    public bool Contains(ControllerButton binding)
    {
        return BindingsInternal.Contains(binding);
    }
    
    protected virtual void Changed()
    {
        // BSIPA 会重写此方法，调用时会触发保存
        // 设置重载时不会调用此方法，BSIPA只会调用最外层对象的 Changed
        Plugin.Log.Trace("ActionBinding Changed");
        Reloaded();
    }

    public void Reloaded()
    {
        Bindings = BindingsInternal.ToList(); // 重新生成只读列表
    }
}

internal class PauseBinding : ActionBinding
{
    public virtual bool BlockBuiltIn { get; set; } = false;

    public override void Reset()
    {
        BlockBuiltIn = false;
        base.Reset();
    }
}

public enum ControllerButton
{
    L_X,
    L_Y,
    R_A,
    R_B,
    L_Grip,
    R_Grip,
    L_Trigger,
    R_Trigger,
    L_Stick,
    R_Stick,
    L_Menu,
    R_Menu
}