using HarmonyLib;
using JmcModLib.Utils;
using ModTemplate.Core;
using System.Reflection;

namespace ModTemplate.Patches
{
    public class HarmonyHelper
    {
        private string PatchId;
        private string PatchTag => $"{VersionInfo.Name}.{PatchId}";
        private Harmony? _harmony;

        public HarmonyHelper(string patchId) => PatchId = patchId;

        public void OnEnable()
        {
            _harmony = new Harmony(PatchTag);
            _harmony.PatchAll(Assembly.GetExecutingAssembly());


            ModLogger.Info($"Harmony 补丁{PatchId}已加载");
        }

        public void OnDisable()
        {
            _harmony?.UnpatchAll(PatchTag);

            ModLogger.Info($"Harmony 补丁{PatchId}已卸载");
        }
    }
}
