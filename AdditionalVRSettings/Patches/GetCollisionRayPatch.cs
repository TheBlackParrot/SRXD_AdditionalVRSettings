using System.Diagnostics.CodeAnalysis;
using GameSystems.XR;
using HarmonyLib;
using UnityEngine;

namespace AdditionalVRSettings.Patches;

[HarmonyPatch]
internal class GetCollisionRayPatch
{
    [HarmonyPatch(typeof(XRHand), nameof(XRHand.GetCollisionRay))]
    [HarmonyPostfix]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal static void Patch(XRHand __instance, Transform rigTransform, ref Ray __result)
    {
        // the 100 is arbitrary
        Vector3 angle = __instance.transform.name switch
        {
            "LeftHand" => new Vector3(Plugin.LeftHandXAngleOffset.Value, Plugin.LeftHandYAngleOffset.Value, Plugin.LeftHandZAngleOffset.Value) / 100f,
            "RightHand" => new Vector3(Plugin.RightHandXAngleOffset.Value, Plugin.RightHandYAngleOffset.Value, Plugin.RightHandZAngleOffset.Value) / 100f,
            _ => Vector3.zero
        };
        
        Vector3 offset = rigTransform.TransformDirection(angle);
        __result.direction += offset;
    }
}