# KeyRemapper

[English Readme](README.md)

一个用于重新映射控制器按键的 **Beat Saber** Mod。

---

## 当前可用功能

| 功能          | 说明                    |
|-------------|-----------------------|
| **Pause**   | 可把 _任意支持的手柄按钮_ 绑定为暂停键 |
| **Restart** | 可绑定 _任意支持的手柄按钮_ 来重启关卡 |

---

## 已支持的按键列表

| Token                        | 物理键           | Unity **CommonUsages** | 手柄侧   |
|------------------------------|---------------|------------------------|-------|
| `L_X` / `R_A`                | X (左) / A (右) | `primaryButton`        | 左 / 右 |
| `L_Y` / `R_B`                | Y (左) / B (右) | `secondaryButton`      | 左 / 右 |
| `L_Grip` / `R_Grip`          | 抓握键           | `gripButton`           | 左 / 右 |
| `L_Trigger` / `R_Trigger`    | 扳机二段          | `triggerButton`        | 左 / 右 |
| `L_Stick` / `R_Stick`        | 摇杆按下          | `primary2DAxisClick`   | 左 / 右 |
| `L_Menu` / `R_Menu`          | 菜单键           | `menuButton`           | 左 / 右 |

> **小贴士 ② – 关于扳机键**  
> 扳机（Trigger）在暂停菜单里同样会触发 **Continue** 按钮，  
> 所以**不推荐**把 `L_Trigger` / `R_Trigger` 绑定为暂停；  
> 如果一定要用，请只绑定一侧扳机，并避开菜单交互。

---

## 依赖

- BSIPA
- BSML
- SiraUtil

---

## 安装

1. 安装依赖
2. 从 [Releases](https://github.com/lyyQwQ/KeyRemapper/releases) 下载最新版
3. 将压缩文件解压进 Beat Saber 文件夹
4. 启动游戏后进入 **Mods -> KeyRemapper** 配置按键

---

## 配置

现在可以直接在游戏内配置按键映射：
1. 进入主菜单的 **Mods** 菜单
2. 选择 **KeyRemapper**
3. 使用下拉菜单添加或删除按键绑定
4. 按需切换 Pause / Restart 的行为
5. 更改会自动保存并立即生效

设置会自动保存到 `UserData/KeyRemapper.json`。

---

## 已知限制 & 提醒

1. **扳机键触发 Pause 会干扰菜单操作**  
   建议使用 ABXY / Grip / Stick / Menu 等键。

---

## 贡献

欢迎提交 Issue 或 Pull Request！如果添加新按键 / 新功能，请同步更新本表及示例。
