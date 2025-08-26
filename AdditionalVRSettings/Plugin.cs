using BepInEx;
using BepInEx.Logging;
using GameSystems.XR;
using HarmonyLib;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// ActionBasedController class is obsolete
#pragma warning disable CS0618

namespace AdditionalVRSettings;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency("srxd.raoul1808.spincore", "1.1.2")]
public partial class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource Log = null!;
    private static Harmony _harmony = null!;

    private void Awake()
    {
        Log = Logger;
        _harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        
        _harmony.PatchAll();
        
        RegisterConfigEntries();
        CreateModPage();
        
        Log.LogInfo("Plugin loaded");
    }

    private void OnEnable()
    {
        UpdateStuff();
    }

    private void OnDestroy()
    {
        _harmony.UnpatchSelf();
    }

    internal static void UpdateStuff()
    {
        UpdateControllerModelVisibility();
        UpdateLaserPointerVisibility();
        UpdateWorldParticlesVisibility();
        UpdateSpectatorCameraSmoothing();
    }

    private static void UpdateControllerModelVisibility()
    {
        ActionBasedController[] controllers = FindObjectsByType<ActionBasedController>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        
        foreach (ActionBasedController controller in controllers)
        {
            Transform? wandModel = controller.model?.Find("XRControllerWand/VRwand");
            if (wandModel == null)
            {
                continue;
            }

            if (wandModel.TryGetComponent(out MeshRenderer wandMeshRenderer))
            {
                wandMeshRenderer.enabled = EnableControllerModels.Value;
            }
        }
    }

    private static void UpdateLaserPointerVisibility()
    {
        ActionBasedController[] controllers = FindObjectsByType<ActionBasedController>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        
        foreach (ActionBasedController controller in controllers)
        {
            Transform? stickRay = controller.model?.Find("XRControllerWand/Armature/Bone/Stick Ray Prefab(Clone)");
            if (stickRay == null)
            {
                continue;
            }
            
            if (stickRay.TryGetComponent(out LineRenderer renderer))
            {
                renderer.enabled = EnableLaserPointers.Value;
            }
        }
    }

    private static void UpdateWorldParticlesVisibility()
    {
        GameObject? particlesObject = GameObject.Find("Menu Particles Object");
        if (particlesObject == null)
        {
            return;
        }
        
        particlesObject.SetActive(EnableWorldParticles.Value);
    }

    private static void UpdateSpectatorCameraSmoothing()
    {
        XROrigin? xrOrigin = FindObjectOfType<XROrigin>();
        XRTransformStabilizer? stabilizer = xrOrigin?.CameraFloorOffsetObject.transform.Find("Spectator Cam Stable")?.GetComponent<XRTransformStabilizer>();
        
        if (stabilizer == null)
        {
            return;
        }

        stabilizer.positionStabilization = CameraPositionSmoothing.Value;
        stabilizer.angleStabilization = CameraAngleSmoothing.Value;
    }
}