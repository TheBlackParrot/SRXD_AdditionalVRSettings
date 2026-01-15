using GameSystems.XR;
using HarmonyLib;
using Unity.Mathematics;

namespace AdditionalVRSettings.Patches;

[HarmonyPatch]
internal static class OffsetStabilizeTransformPatch
{
    [HarmonyPatch(typeof(XRTransformStabilizer), nameof(XRTransformStabilizer.StabilizeTransform))]
    [HarmonyPrefix]
    private static bool StabilizeTransformPatch(ref float3 targetPos)
    {
        targetPos.x += Plugin.CameraOffsetX.Value;
        targetPos.y += Plugin.CameraOffsetY.Value;
        targetPos.z += Plugin.CameraOffsetZ.Value;
        
        return true;
    }
}