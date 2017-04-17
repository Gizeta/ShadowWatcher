# ShadowWatcher
Shadowverse 辅助工(wán)具(jù)

## 如何编译
1. 编译出系统可运行的 `mono-assembly-injector.exe` 文件，放入 `ShadowWatcher/injector/` 文件夹中
2. 从游戏目录复制 `Assembly-CSharp.dll` 及 `UnityEngine.dll` 文件，放入 `Observer/Libraries/` 文件夹中
3. 启动 `Visual Studio` 生成可执行文件

## 如何使用
1. 启动 `Shadowverse` 程序
2. 启动 `ShadowWatcher` 程序
3. 点击 `Attach/Detach` 按钮，等待注入完成

## 目前已实现的功能
1. 记录对手出牌数量
2. 记录自己牌库剩余

## 说明事项
1. 本程序会监听网络接收信息及玩家操作回调，对于是否为外挂的问题，请自行判断。
2. Debug 版本为了调试方便，会抓取并显示一些常规游玩中获取不到的信息，请谨慎分发 Debug 版本的二进制文件。
3. 如果有好的建议、意见或界面设计，欢迎提交 Issue 或 Pull Request。

## 感谢
* [mono-assembly-injector](https://github.com/gamebooster/mono-assembly-injector)
* [Blackbone](https://github.com/DarthTon/Blackbone)
