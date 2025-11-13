using GameSystems.XR;
using HarmonyLib;
using UnityEngine;

namespace AdditionalVRSettings.Patches;

[HarmonyPatch]
internal class GetCollisionRayPatch
{
    [HarmonyPatch(typeof(XRHand), nameof(XRHand.GetCollisionRay))]
    [HarmonyPostfix]
    // ReSharper disable once InconsistentNaming
    internal static void Patch(Transform rigTransform, ref Ray __result)
    {
        // the 100 is arbitrary
        Vector3 offset = rigTransform.TransformDirection(
            (Plugin.HandXAngleOffset.Value) / 100,
            (Plugin.HandYAngleOffset.Value) / 100,
            (Plugin.HandZAngleOffset.Value) / 100);
        
        __result.direction = __result.direction with
        {
            x = __result.direction.x + offset.x,
            y = __result.direction.y + offset.y,
            z = __result.direction.z + offset.z
        };
    }
}