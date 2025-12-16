**🌐[ 中文 | [English](README_en.md) ]**

[📝更新日志](CHANGELOG.md)

[📦 Releases](https://github.com/JMC2002/ModTemplate)

# 代码模板
## 1. 关于csproj
从[ModTemplate.csproj](ModTemplate.csproj)中取得

只需要修改以下部分内容即可，配置好后本台电脑上每个新MOD直接粘贴都能用：
```xml
    <!-- 鸭科夫安装路径 -->
    <DuckovPath>D:\SteamLibrary\steamapps\common\Escape from Duckov</DuckovPath>

    <!-- Managed DLL目录 -->
    <DuckovManagedPath>$(DuckovPath)\Duckov_Data\Managed</DuckovManagedPath>

    <!-- Managed DLL目录 -->
    <DuckovModPath>$(DuckovPath)\Duckov_Data\Mods\</DuckovModPath>
    <!-- 项目根目录下的 Mods 文件夹 -->
    <ModLocalDir>$(ProjectDir)$(ModName)</ModLocalDir>
    <!-- 游戏 Mods 文件夹 -->
    <ModGameDir>$(DuckovModPath)$(ModName)</ModGameDir>
    <!-- 创意工坊文件夹 -->
    <SteamWorkShop>D:\SteamLibrary\steamapps\workshop\content\3167020\</SteamWorkShop>
    <!-- 存放版本信息的文件，提取形如Version = x.x.x的信息 -->
    <ModVersionFile>$(ProjectDir)Core\VersionInfo.cs</ModVersionFile>
    <Configurations>Debug;Release</Configurations>

  </PropertyGroup>
  ```

  如果想要依赖创意工坊的文件，就像这样：
```xml
    <Reference Include="JmcModLib">
      <HintPath>$(SteamWorkShop)3613297900\JmcModLib.dll</HintPath>
    </Reference>
```
特别地，对于`JmcModLib`，我这里引用的是本地版本，如果你需要引用这个库，请改成创意工坊版本（也就是下面注释掉的行）


  然后在你的项目文件夹中新建一个`Core`文件夹，并在其中创建一个`VersionInfo.cs`文件，内容如下：
  ```csharp
  
namespace ModTemplate.Core
{
    internal static class VersionInfo
    {
        internal const string Name = "ModTemplate";
        internal const string Version = "1.0.0";
    }
}
```
生成即可自动从中提取版本号和Mod名称，（文件路径和文件名可以修改上面的`ModVersionFile`实现）

生成会做这些事情：
- 从前面提到的文件提取版本号和Mod名称加上最上面的MOD名称、作者名称注入到DLL中
- 生成DLL
- 复制DLL到根目录下的`ModName`文件夹（默认与项目文件同名）下
- 增量复制根目录下的`ModName`文件夹到游戏安装目录的`Duckov_Data\Mods\ModName`文件夹下（没有会自动新建），因此建议在这里维护ini、头像之类的信息
- 一切完成后弹窗询问是否打开游戏

## 2. 关于无序依赖
如果你的MOD强依赖于其他MOD的DLL（不局限于JmcModLib），可以用本项目的方法取消加载顺序的需求，
- 首先复制[DependencyModLoader.cs](Core/DependencyModLoader.cs)到你的项目中，其中可通过`LOADER_VERSION`得到当前的加载器版本号，以便进行兼容性检查
- 然后将入口`ModBehaviour.cs`换成本项目的[ModBehaviour.cs](ModBehaviour.cs)，并修改其中的`DependencyNames`属性，添加你所依赖的MOD名称，这样就能确保在这些MOD加载完成后再加载你的MOD，从而避免因加载顺序不对导致的找不到类型等问题
- 同时，在[ModBehaviourImpl.cs](Core/ModBehaviourImpl.cs)中实现你本来的ModBehaviour逻辑
- 记得将命名空间重命名为自己的命名空间，以防命名冲突

这样操作后，会有如下效果：
- 你的MOD会在所依赖的MOD加载完成后再加载
- 若前置未安装，会在右下角弹一个红色的窗提示前置未安装，MOD会停止加载实际逻辑
- 若前置安装了但未勾选，会在右下角弹一个黄色的窗提示前置未启用
- 在前置已安装的情况下（无论是否勾选），MOD都会启动监听，等到前置全部加载完毕后才加载实际的逻辑DLL

关于弹窗的属性：
- 右下角弹窗可以通过点击关闭，在勾选前置后也会自动消失（如果没消失可能需要重新勾选一下前置）
- 弹窗会自动本地化操作
- 当前置依赖缺失（即红色弹窗）且填写了对应的创意工坊ID时，点击弹窗会跳转到创意工坊对应的订阅页面，方便用户订阅前置；其中一个依赖缺失时会用Steam直接打开，一个以上会在内部叠加层浏览器打开，非Steam用户会直接用浏览器打开
- 除非明确知道自己在干什么，否则不建议修改`DependencyModLoader.cs`的相关代码，不然可能会导致弹窗错乱

![弹窗示例](Pic/依赖检测演示.png)
![弹窗示例](Pic/依赖检测演示中.jpg)
![弹窗示例](Pic/依赖检测演示日.jpg)
![弹窗示例](Pic/依赖检测演示俄.jpg)

## 3. 交流与反馈
- 本项目附属于[JmcModLib](https://github.com/JMC2002/JmcModLib)（[创意工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=3613297900)），如有反馈或建议，除了在本项目Issue提出外，也可以在该项目创意工坊评论区提出
- 如果使用了本项目的依赖`DependencyModLoader`，尽量告知我一声，以便对后续可能存在的更新进行提醒。
- [QQ群点击入群（617674584）](http://qm.qq.com/cgi-bin/qm/qr?_wv=1027&k=Kii1sz9bmNmgEgnmsasbKn6h3etgSoQR&authKey=Hni0nbFlbd%2BfDZ1GoklCdtHw4r79SuEsHvz9Pi4qs020w1f8D2DHD8EEnNN1OXo6&noverify=0&group_code=617674584)
- [Discord链接](https://discord.gg/pnrpRmU2)