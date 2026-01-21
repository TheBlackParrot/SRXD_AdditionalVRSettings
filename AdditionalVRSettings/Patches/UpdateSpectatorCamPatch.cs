using GameSystems.XR;
using HarmonyLib;

namespace AdditionalVRSettings.Patches;

[HarmonyPatch]
internal static class UpdateSpectatorCamPatch
{
    [HarmonyPatch(typeof(XRPlayerRig), nameof(XRPlayerRig.UpdateSpectatorCam))]
    [HarmonyPostfix]
    // ReSharper disable once InconsistentNaming
    private static void Patch(XRPlayerRig __instance)
    {
        if (!Plugin.LockXRotationOnCamera.Value)
        {
            return;
        }
        
        __instance.cameraTransformWithoutRoll.localEulerAngles =
            __instance.cameraTransformWithoutRoll.localEulerAngles with { x = 0 };
    }
}