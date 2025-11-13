using System.Globalization;
using BepInEx.Configuration;
using SpinCore.Translation;
using SpinCore.UI;
using UnityEngine;

namespace AdditionalVRSettings;

public partial class Plugin
{
    public static ConfigEntry<bool> EnableControllerModels = null!;
    public static ConfigEntry<bool> EnableLaserPointers = null!;
    public static ConfigEntry<bool> EnableWorldParticles = null!;
    public static ConfigEntry<bool> EnableBasePlatform = null!;
    
    public static ConfigEntry<float> CameraPositionSmoothing = null!;
    public static ConfigEntry<float> CameraAngleSmoothing = null!;

    public static ConfigEntry<float> LeftHandXAngleOffset = null!;
    public static ConfigEntry<float> LeftHandYAngleOffset = null!;
    public static ConfigEntry<float> LeftHandZAngleOffset = null!;
    public static ConfigEntry<float> RightHandXAngleOffset = null!;
    public static ConfigEntry<float> RightHandYAngleOffset = null!;
    public static ConfigEntry<float> RightHandZAngleOffset = null!;

    private void RegisterConfigEntries()
    {
        TranslationHelper.AddTranslation("AVRS_Name", nameof(AdditionalVRSettings));
        
        EnableControllerModels =
            Config.Bind("General", nameof(EnableControllerModels), true, "Enables controller models");
        TranslationHelper.AddTranslation("AVRS_EnableControllerModels", "Enable controller models");
        
        EnableLaserPointers =
            Config.Bind("General", nameof(EnableLaserPointers), true, "Enables laser pointers");
        TranslationHelper.AddTranslation("AVRS_EnableLaserPointers", "Enable laser pointers");
        
        EnableWorldParticles =
            Config.Bind("General", nameof(EnableWorldParticles), true, "Enables world particles");
        TranslationHelper.AddTranslation("AVRS_EnableWorldParticles", "Enable world particles");
        
        EnableBasePlatform =
            Config.Bind("General", nameof(EnableBasePlatform), true, "Enables platform underneath you");
        TranslationHelper.AddTranslation("AVRS_EnableBasePlatform", "Enable base platform");

        TranslationHelper.AddTranslation("AVRS_Smoothing", "Smoothing");
        
        CameraPositionSmoothing =
            Config.Bind("Smoothing", nameof(CameraPositionSmoothing), 0.15f, "Spectator Camera position smoothing");
        TranslationHelper.AddTranslation("AVRS_CameraPositionSmoothing", "Spectator Camera position smoothing factor");
        
        CameraAngleSmoothing =
            Config.Bind("Smoothing", nameof(CameraAngleSmoothing), 20f, "Spectator Camera angle smoothing");
        TranslationHelper.AddTranslation("AVRS_CameraAngleSmoothing", "Spectator Camera angle smoothing factor");
        
        TranslationHelper.AddTranslation("AVRS_ControllerOffsets", "Controller Angle Offsets");
        
        LeftHandXAngleOffset =
            Config.Bind("ControllerOffsets", nameof(LeftHandXAngleOffset), 0f, "X angle offset for the left controller");
        LeftHandYAngleOffset =
            Config.Bind("ControllerOffsets", nameof(LeftHandYAngleOffset), 0f, "Y angle offset for the left controller");
        LeftHandZAngleOffset =
            Config.Bind("ControllerOffsets", nameof(LeftHandZAngleOffset), 0f, "Z angle offset for the left controller");
        RightHandXAngleOffset =
            Config.Bind("ControllerOffsets", nameof(RightHandXAngleOffset), 0f, "X angle offset for the right controller");
        RightHandYAngleOffset =
            Config.Bind("ControllerOffsets", nameof(RightHandYAngleOffset), 0f, "Y angle offset for the right controller");
        RightHandZAngleOffset =
            Config.Bind("ControllerOffsets", nameof(RightHandZAngleOffset), 0f, "Z angle offset for the right controller");
        TranslationHelper.AddTranslation("AVRS_LeftHandAngleOffset", "Left controller (X, Y, Z)");
        TranslationHelper.AddTranslation("AVRS_RightHandAngleOffset", "Right controller (X, Y, Z)");
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
        
        #region EnableWorldParticles
        CustomGroup enableWorldParticlesGroup = UIHelper.CreateGroup(modGroup, "EnableWorldParticlesGroup");
        enableWorldParticlesGroup.LayoutDirection = Axis.Horizontal;
        UIHelper.CreateSmallToggle(enableWorldParticlesGroup, nameof(EnableWorldParticles),
            "AVRS_EnableWorldParticles", EnableWorldParticles.Value, value =>
            {
                EnableWorldParticles.Value = value;
                UpdateWorldParticlesVisibility();
            });
        #endregion
        
        #region EnableBasePlatform
        CustomGroup enableBasePlatformGroup = UIHelper.CreateGroup(modGroup, "EnableBasePlatformGroup");
        enableBasePlatformGroup.LayoutDirection = Axis.Horizontal;
        UIHelper.CreateSmallToggle(enableBasePlatformGroup, nameof(EnableBasePlatform),
            "AVRS_EnableBasePlatform", EnableBasePlatform.Value, value =>
            {
                EnableBasePlatform.Value = value;
                UpdateBasePlatformVisibility();
            });
        #endregion
        
        UIHelper.CreateSectionHeader(modGroup, "SmoothingHeader", "AVRS_Smoothing", false);
        
        #region CameraPositionSmoothing
        CustomGroup cameraPositionSmoothingGroup = UIHelper.CreateGroup(modGroup, "CameraPositionSmoothingGroup");
        cameraPositionSmoothingGroup.LayoutDirection = Axis.Horizontal;
        UIHelper.CreateLabel(cameraPositionSmoothingGroup, "CameraPositionSmoothingLabel", "AVRS_CameraPositionSmoothing");
        CustomInputField cameraPositionSmoothingInput = UIHelper.CreateInputField(cameraPositionSmoothingGroup,
            "CameraPositionSmoothingInput", (_, newValue) =>
        {
            if (!float.TryParse(newValue, out float value))
            {
                return;
            }
            
            CameraPositionSmoothing.Value = value * 100; // a little bit more comprehensible imo
            UpdateSpectatorCameraSmoothing();
        });
        cameraPositionSmoothingInput.InputField.SetText(CameraPositionSmoothing.Value.ToString(CultureInfo.InvariantCulture));
        #endregion
        
        #region CameraAngleSmoothing
        CustomGroup cameraAngleSmoothingGroup = UIHelper.CreateGroup(modGroup, "CameraAngleSmoothingGroup");
        cameraAngleSmoothingGroup.LayoutDirection = Axis.Horizontal;
        UIHelper.CreateLabel(cameraAngleSmoothingGroup, "CameraAngleSmoothingLabel", "AVRS_CameraAngleSmoothing");
        CustomInputField cameraAngleSmoothingInput = UIHelper.CreateInputField(cameraAngleSmoothingGroup,
            "CameraAngleSmoothingInput", (_, newValue) =>
        {
            if (!float.TryParse(newValue, out float value))
            {
                return;
            }
            
            CameraAngleSmoothing.Value = value * 100; // a little bit more comprehensible imo
            UpdateSpectatorCameraSmoothing();
        });
        cameraAngleSmoothingInput.InputField.SetText(CameraAngleSmoothing.Value.ToString(CultureInfo.InvariantCulture));
        #endregion
        
        UIHelper.CreateSectionHeader(modGroup, "AngleOffsetHeader", "AVRS_ControllerOffsets", false);
        
        #region LeftHandAngleOffset
        CustomGroup leftHandAngleOffsetGroup = UIHelper.CreateGroup(modGroup, "LeftHandAngleOffsetGroup");
        leftHandAngleOffsetGroup.LayoutDirection = Axis.Horizontal;
        UIHelper.CreateLabel(leftHandAngleOffsetGroup, "LeftHandAngleOffsetLabel", "AVRS_LeftHandAngleOffset");
        CustomInputField leftHandXAngleOffsetInput = UIHelper.CreateInputField(leftHandAngleOffsetGroup,
            "LeftHandXAngleOffsetInput", (_, newValue) =>
            {
                if (!float.TryParse(newValue, out float value))
                {
                    return;
                }

                LeftHandXAngleOffset.Value = value;
            });
        CustomInputField leftHandYAngleOffsetInput = UIHelper.CreateInputField(leftHandAngleOffsetGroup,
            "LeftHandYAngleOffsetInput", (_, newValue) =>
            {
                if (!float.TryParse(newValue, out float value))
                {
                    return;
                }

                LeftHandYAngleOffset.Value = value;
            });
        CustomInputField leftHandZAngleOffsetInput = UIHelper.CreateInputField(leftHandAngleOffsetGroup,
            "LeftHandZAngleOffsetInput", (_, newValue) =>
            {
                if (!float.TryParse(newValue, out float value))
                {
                    return;
                }

                LeftHandZAngleOffset.Value = value;
            });
        leftHandXAngleOffsetInput.InputField.SetText(LeftHandXAngleOffset.Value.ToString(CultureInfo.InvariantCulture));
        leftHandYAngleOffsetInput.InputField.SetText(LeftHandYAngleOffset.Value.ToString(CultureInfo.InvariantCulture));
        leftHandZAngleOffsetInput.InputField.SetText(LeftHandZAngleOffset.Value.ToString(CultureInfo.InvariantCulture));
        #endregion
        
        #region RightHandAngleOffset
        CustomGroup rightHandAngleOffsetGroup = UIHelper.CreateGroup(modGroup, "RightHandAngleOffsetGroup");
        rightHandAngleOffsetGroup.LayoutDirection = Axis.Horizontal;
        UIHelper.CreateLabel(rightHandAngleOffsetGroup, "RightHandAngleOffsetLabel", "AVRS_RightHandAngleOffset");
        CustomInputField rightHandXAngleOffsetInput = UIHelper.CreateInputField(rightHandAngleOffsetGroup,
            "RightHandXAngleOffsetInput", (_, newValue) =>
            {
                if (!float.TryParse(newValue, out float value))
                {
                    return;
                }

                RightHandXAngleOffset.Value = value;
            });
        CustomInputField rightHandYAngleOffsetInput = UIHelper.CreateInputField(rightHandAngleOffsetGroup,
            "RightHandYAngleOffsetInput", (_, newValue) =>
            {
                if (!float.TryParse(newValue, out float value))
                {
                    return;
                }

                RightHandYAngleOffset.Value = value;
            });
        CustomInputField rightHandZAngleOffsetInput = UIHelper.CreateInputField(rightHandAngleOffsetGroup,
            "RightHandZAngleOffsetInput", (_, newValue) =>
            {
                if (!float.TryParse(newValue, out float value))
                {
                    return;
                }

                RightHandZAngleOffset.Value = value;
            });
        rightHandXAngleOffsetInput.InputField.SetText(RightHandXAngleOffset.Value.ToString(CultureInfo.InvariantCulture));
        rightHandYAngleOffsetInput.InputField.SetText(RightHandYAngleOffset.Value.ToString(CultureInfo.InvariantCulture));
        rightHandZAngleOffsetInput.InputField.SetText(RightHandZAngleOffset.Value.ToString(CultureInfo.InvariantCulture));
        #endregion
    }
}