using BepInEx.Configuration;
using SpinCore.Translation;
using SpinCore.UI;
using UnityEngine;

namespace AdditionalVRSettings;

public partial class Plugin
{
    public static ConfigEntry<bool> EnableControllerModels;
    public static ConfigEntry<bool> EnableLaserPointers;

    private void RegisterConfigEntries()
    {
        TranslationHelper.AddTranslation("AVRS_Name", nameof(AdditionalVRSettings));
        
        EnableControllerModels =
            Config.Bind("General", nameof(EnableControllerModels), true, "Enables controller models");
        TranslationHelper.AddTranslation("AVRS_EnableControllerModels", "Enable controller models");
        
        EnableLaserPointers =
            Config.Bind("General", nameof(EnableLaserPointers), true, "Enables laser pointers");
        TranslationHelper.AddTranslation("AVRS_EnableLaserPointers", "Enable laser pointers");
    }

    private static void CreateModPage()
    {
        CustomPage rootModPage = UIHelper.CreateCustomPage("ModSettings");
        rootModPage.OnPageLoad += RootModPageOnOnPageLoad;
        
        UIHelper.RegisterMenuInModSettingsRoot("AVRS_Name", rootModPage);
    }

    private static void RootModPageOnOnPageLoad(Transform rootModPageTransform)
    {
        CustomGroup modGroup = UIHelper.CreateGroup(rootModPageTransform, nameof(AdditionalVRSettings));
        UIHelper.CreateSectionHeader(modGroup, "ModGroupHeader", "AVRS_Name", false);
            
        #region EnableControllerModels
        CustomGroup enableControllerModelsGroup = UIHelper.CreateGroup(modGroup, "EnableControllerModelsGroup");
        enableControllerModelsGroup.LayoutDirection = Axis.Horizontal;
        UIHelper.CreateSmallToggle(enableControllerModelsGroup, nameof(EnableControllerModels),
            "AVRS_EnableControllerModels", EnableControllerModels.Value, value =>
            {
                EnableControllerModels.Value = value;
                UpdateControllerModelVisibility();
            });
        #endregion
        
        #region EnableLaserPointers
        CustomGroup enableLaserPointersGroup = UIHelper.CreateGroup(modGroup, "EnableLaserPointersGroup");
        enableLaserPointersGroup.LayoutDirection = Axis.Horizontal;
        UIHelper.CreateSmallToggle(enableLaserPointersGroup, nameof(EnableLaserPointers),
            "AVRS_EnableLaserPointers", EnableLaserPointers.Value, value =>
            {
                EnableLaserPointers.Value = value;
                UpdateLaserPointerVisibility();
            });
        #endregion
    }
}