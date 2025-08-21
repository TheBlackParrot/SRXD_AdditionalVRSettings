using BepInEx;
using BepInEx.Logging;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// ActionBasedController class is obsolete
#pragma warning disable CS0618

namespace AdditionalVRSettings;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public partial class Plugin : BaseUnityPlugin
{
    private static ManualLogSource _logger;

    private void Awake()
    {
        _logger = Logger;
        
        RegisterConfigEntries();
        CreateModPage();
        
        _logger.LogInfo("Plugin loaded");
    }

    private void OnEnable()
    {
        UpdateControllerModelVisibility();
        UpdateLaserPointerVisibility();
    }

    private static void UpdateControllerModelVisibility()
    {
        ActionBasedController[] controllers = FindObjectsByType<ActionBasedController>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (ActionBasedController controller in controllers)
        {
            Transform wandModel = controller.model.Find("XRControllerWand/VRwand");
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
            Transform stickRay = controller.model.Find("XRControllerWand/Armature/Bone/Stick Ray Prefab(Clone)");
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
}