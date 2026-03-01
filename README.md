# TurnBasedSystem-CS

通用回合制系统 Godot 4.x C# 插件 — 四维组合架构，支持 8 种回合制类型。

## 特性

- **四维组合**: Order × Budget × Resolution × Phase，自由搭配
- **8 种预设模板**: Classic, Faction, Initiative, ATB, PressTurn, Simultaneous, Resource, Tick
- **信号驱动**: Godot 原生信号 + async/await 回调
- **零依赖**: 纯 C#，无第三方库

## 支持的回合制类型

| 模板 | 代表游戏 | 说明 |
|------|----------|------|
| Classic | 象棋/围棋 | 双方严格交替 |
| Faction | 火焰纹章/XCOM | 阵营全体行动 |
| Initiative | 博德之门3/FFX | 速度先攻制 |
| ATB | FF ATB | 半即时制 |
| PressTurn | 女神异闻录 | 弱点连锁制 |
| Simultaneous | 文明/Into the Breach | 同时行动制 |
| Resource | 杀戮尖塔/神界原罪2 | 资源驱动制 |
| Tick | Nethack/不思议迷宫 | Roguelike 滴答制 |

## 安装

### Godot Asset Library

在 Godot 编辑器中搜索 "TurnBasedSystem" 并安装。

### 手动安装

将 `addons/TurnBasedSystem/` 复制到项目的 `addons/` 目录，在 Project Settings → Plugins 中启用。

## 快速开始

```csharp
using TurnBasedSystem;

// 1. 创建节点并选择模板
var turnSystem = new TurnSystemNode();
turnSystem.Configure(TurnTemplates.Faction(2));
AddChild(turnSystem);

// 2. 注册阵营
turnSystem.RegisterFaction(playerFaction);
turnSystem.RegisterFaction(enemyFaction);

// 3. 设置回调
turnSystem.OnPlayerInput = async (ctx, step) => {
    // 等待玩家输入，返回 ITurnAction
    return await WaitForPlayerMove();
};
turnSystem.OnAIDecision = async (ctx, step) => {
    // AI 决策，返回行动列表
    return ComputeAIMoves();
};

// 4. 执行回合
await turnSystem.ExecuteRound();
```

## 四维架构

| 维度 | 接口 | 职责 |
|------|------|------|
| Order | `ITurnOrderResolver` | 谁先行动 |
| Budget | `IActionBudget` | 能做几次 |
| Resolution | `IResolutionPolicy` | 何时结算 |
| Phase | `ITurnPhaseSequence` | 阶段流程 |

## 目录结构

```
addons/TurnBasedSystem/
├── Core/           # 核心接口和编排器
├── Order/          # 7 种行动顺序策略
├── Budget/         # 4 种行动预算策略
├── Resolution/     # 3 种结算策略
├── Phase/          # 5 种阶段序列
└── Templates/      # 8 种预设模板
```

## License

MIT
