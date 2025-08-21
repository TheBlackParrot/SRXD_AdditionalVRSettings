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
            controller.hideControllerModel = !EnableControllerModels.Value;
        }
    }

    private static void UpdateLaserPointerVisibility()
    {
        ActionBasedController[] controllers = FindObjectsByType<ActionBasedController>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        
        foreach (ActionBasedController controller in controllers)
        {
            LineRenderer renderer = controller.modelParent.GetComponentInChildren<LineRenderer>();
            if (renderer != null)
            {
                renderer.enabled = EnableLaserPointers.Value;
            }
        }
    }
}