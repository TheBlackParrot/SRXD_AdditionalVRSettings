using GameSystems.XR;
using HarmonyLib;
using Unity.Mathematics;

namespace AdditionalVRSettings.Patches;

[HarmonyPatch]
internal static class OffsetStabilizeTransformPatch
{
    [HarmonyPatch(typeof(XRTransformStabilizer), nameof(XRTransformStabilizer.StabilizeTransform))]
    [HarmonyPrefix]
    private static bool StabilizeTransformPatch(ref float3 targetPos, ref quaternion targetRot)
    {
        targetPos.x += Plugin.CameraOffsetX.Value;
        targetPos.y += Plugin.CameraOffsetY.Value;
        targetPos.z += Plugin.CameraOffsetZ.Value;
        
        // arcsin(1) = 1.570796326794897
        // 57.29859f is a magic number, but i'm assuming it's close to arcsin(1) for a reason. saw this a bunch during research
        
        // started by getting divisor values close enough to almost be the same between 0 and 360 for Plugin.CameraXRotation
        // (which was a divisor of 57f)
        // then set the value back to 0, zeroed out the eulerAngles of the stabilized camera object's euler angles,
        // and then set the value to 1 and took the X value of the eulerAngles and multiplied the divisor (57f) by it
        // giving the 57.29859f magic number
        
        // i barely got through high school algebra 2. and it's been over a decade since then LOL. i'm trying ;w;
        // i *want to* understand how to properly arrive at this number but. dum
        
        float3 rot = math.Euler(targetRot);
        if (Plugin.LockXRotationOnCamera.Value)
        {
            rot.x = Plugin.CameraXRotation.Value / 57.29859f;
        }
        else
        {
            rot.x += Plugin.CameraXRotation.Value / 57.29859f;
        }
        targetRot = quaternion.Euler(rot);

        return true;
    }
}