using Duckov.Modding;
using ModTemplate.Core;
using UnityEngine;

namespace ModTemplate   // 重命名为自己的命名空间
{
    public class ModBehaviour : DependencyModLoader
    {
        protected override ModDependency[] GetDependencies()
        {
            // 这里是所有的前置依赖项，在加载完前置后才会加载实际的脚本
            return
            [
                //new ModDependency("JmcModLib", 3613297900),
                //new ModDependency("缺失测试1", 3589079671),
                //new ModDependency("缺失测试2", 3591875771),
            ];
        }

        // 挂载实际业务脚本
        protected override MonoBehaviour CreateImplementation(ModManager master, ModInfo info)
        {
            // 为了防止有人订阅这个MOD，不做任何事
            return default!;

            // 挂载组件
            var impl = this.gameObject.AddComponent<ModBehaviourImpl>();

            impl.Setup(master, info);

            return impl;
        }
    }
}
