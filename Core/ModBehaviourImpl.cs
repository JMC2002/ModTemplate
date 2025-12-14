using ModTemplate.Patches;
using JmcModLib.Core;
using JmcModLib.Utils;

namespace ModTemplate.Core
{
    // 这个文件实现实际的ModBehaviour（即以前的ModBehaviour）的内容
    public class ModBehaviourImpl : Duckov.Modding.ModBehaviour
    {
        private readonly HarmonyHelper harmonyHelper = new($"{VersionInfo.Name}");
        private void OnEnable()
        {
        }
        private void OnDisable()
        {
            ModLogger.Info("Mod 即将禁用，配置已保存");
            harmonyHelper.OnDisable();
        }

        protected override void OnAfterSetup()
        {
            ModRegistry.Register(true, info, VersionInfo.Name, VersionInfo.Version)?
                       .RegisterL10n()
                       .RegisterLogger(uIFlags: LogConfigUIFlags.All)
                       .Done();
            harmonyHelper.OnEnable();
        }

        protected override void OnBeforeDeactivate()
        {
            ModLogger.Info("Mod 已禁用，配置已保存");
        }
    }
}