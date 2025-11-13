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
    internal static void Patch(ref Ray __result)
    {
        // the 100 is arbitrary
        __result.direction = __result.direction with
        {
            x = __result.direction.x + (Plugin.HandXAngleOffset.Value)/100,
            y = __result.direction.y + (Plugin.HandYAngleOffset.Value)/100,
            z = __result.direction.z + (Plugin.HandZAngleOffset.Value)/100
        };
    }
}