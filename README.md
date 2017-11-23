# ShadowWatcher
Shadowverse 辅助工(wán)具(jù)

## 如何编译
1. 从游戏目录复制 `Assembly-CSharp.dll` 及 `UnityEngine.dll` 文件，放入 `Observer/Libraries/` 文件夹中
2. 启动 `Visual Studio` 生成可执行文件

## 如何使用
1. 启动 `Shadowverse` 程序
2. 启动 `ShadowWatcher` 程序
3. 点击 `Attach/Detach` 按钮，等待注入完成

## 目前已实现的功能
1. 记录对手出牌数量
2. 记录自己牌库剩余
3. 回放中显示对手手牌
4. 导出/加载回放
5. 浏览衍生卡牌及部分异画卡牌
6. 导入卡组时优先使用珍稀卡牌
7. 对战回合倒计时
8. 卡牌列表键盘按键过滤
9. 加载卡面mod文件

## 说明事项
1. 本程序会监听网络接收信息及玩家操作回调，对于是否为外挂的问题，请自行判断。
2. 游戏每次更新可能会涉及代码接口的变动，请在确保兼容新版本的情况下使用本程序。
3. 如果有好的建议、意见或界面设计，欢迎提交 Issue 或 Pull Request。

## 如何有效地报告bug
请提供可稳定复现的操作步骤。
如果有游戏流程卡住或游戏崩溃的情况，请同时附上 `steamapps\common\Shadowverse\Shadowverse_Data\output_log.txt` 中相关的错误信息。

## 感谢
* [Olivia](https://github.com/Mattish/Olivia)
* [mono-assembly-injector](https://github.com/gamebooster/mono-assembly-injector)
* [Blackbone](https://github.com/DarthTon/Blackbone)