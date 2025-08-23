using HarmonyLib;
using Unity.XR.CoreUtils;

namespace AdditionalVRSettings.Patches;

[HarmonyPatch]
internal class XROriginOnEnablePatch
{
    [HarmonyPatch(typeof(XROrigin), "OnEnable")]
    [HarmonyPostfix]
    private static void OnEnablePatch()
    {
        // no arguments, we just want to update stuff
        Plugin.UpdateStuff();
    }
}