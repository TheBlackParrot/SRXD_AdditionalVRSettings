using HarmonyLib;
using Unity.XR.CoreUtils;

namespace AdditionalVRSettings.Patches;

[HarmonyPatch]
internal class TryInitializeCameraPatch
{
    [HarmonyPatch(typeof(XROrigin), nameof(XROrigin.TryInitializeCamera))]
    [HarmonyPostfix]
    // ReSharper disable once InconsistentNaming
    private static void Patch(XROrigin __instance)
    {
        if (!__instance.m_CameraInitialized)
        {
            return;
        }
        
        Plugin.Log.LogDebug("TryInitializeCamera");
        Plugin.UpdateStuff();
    }
}