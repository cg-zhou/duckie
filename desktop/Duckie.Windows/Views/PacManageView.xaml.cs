using Duckie.Services.PacManager;
using Duckie.Shared.Services.UserConfigs;
using Duckie.Shared.Utils.Localization;
using Duckie.Shared.Utils.Ui;
using Duckie.Shared.Views.Common;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Duckie.Views;

public sealed class PacListItem
{
    public PacConfig Config { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public bool CanEdit => Config != null;
}

public partial class PacManageView : UserControl
{
    public PacManageView()
    {
        InitializeComponent();
        CycleHotKeyEditor.Operation = PacManagerHotKeyService.CycleAction;
        Loaded += (_, _) =>
        {
            PacManagerService.PacStatusChanged += OnPacStatusChanged;
            EmbeddedLocalizationManager.Instance.LanguageChanged += OnLanguageChanged;
            LoadPacConfigs();
        };
        Unloaded += (_, _) =>
        {
            PacManagerService.PacStatusChanged -= OnPacStatusChanged;
            EmbeddedLocalizationManager.Instance.LanguageChanged -= OnLanguageChanged;
        };
        IsVisibleChanged += (_, _) => { if (IsVisible) LoadPacConfigs(); };
    }

    private void OnLanguageChanged(object sender, EventArgs e) => LoadPacConfigs();

    private void LoadPacConfigs()
    {
        try
        {
            var currentUri = PacManagerService.GetCurrentPacUri();
            var configs = PacManagerService.GetAllPacConfigs();
            var direct = string.IsNullOrWhiteSpace(currentUri);
            PacItems.ItemsSource = configs.Select(config => new PacListItem
            {
                Config = config, Name = config.Name, Description = config.Uri,
                IsActive = string.Equals(config.Uri, currentUri, StringComparison.OrdinalIgnoreCase)
            }).Concat(new[] { new PacListItem
            {
                Name = LocKey.Label_DirectConnection.Text(), Description = LocKey.PAC_DirectDescription.Text(), IsActive = direct
            } }).ToArray();
        }
        catch (Exception ex)
        {
            UiUtils.ShowToast(LocKey.Error_FailedToLoadPAC.Text() + "：" + ex.Message, true);
        }
    }

    private void PacChoice_Click(object sender, RoutedEventArgs e)
    {
        if (!(sender is RadioButton choice)) return;
        var pac = choice.Tag as PacConfig;
        var currentUri = PacManagerService.GetCurrentPacUri();
        if (string.Equals(currentUri ?? "", pac?.Uri ?? "", StringComparison.OrdinalIgnoreCase)) return;
        PacManagerService.ApplyPacConfig(pac);
    }

    private void OnPacStatusChanged(object sender, PacStatusChangedEventArgs e)
    {
        Dispatcher.Invoke(() =>
        {
            LoadPacConfigs();
            if (!IsVisible) return;
            UiUtils.ShowToast(e.IsSuccess
                ? e.CurrentConfig == null ? LocKey.Status_PACCleared.Text() : LocKey.Status_PACSwitch.Text(e.CurrentConfig.Name)
                : LocKey.Status_PACFailed.Text(e.ErrorMessage), !e.IsSuccess);
        });
    }

    #region Event Handlers

    private void AddPacButton_Click(object sender, RoutedEventArgs e)
    {
        var content = new PacEditContent();
        var dialog = DialogEx.Create(
            LocKey.Dialog_AddPAC.Text(),
            content,
            DialogButtons.OKCancel,
            (d) =>
            {
                if (content.ValidateAndCreateConfig())
                {
                    try
                    {
                        var success = PacManagerService.AddPacConfig(content.PacConfig.Name, content.PacConfig.Uri);
                        if (success)
                        {
                            UiUtils.ShowToast(LocKey.Status_PACAdded.Text(content.PacConfig.Name));
                            LoadPacConfigs();
                            d.DialogResult = true;
                            d.Close();
                        }
                        else
                        {
                            UiUtils.Warning(LocKey.Error_FailedToAddPAC.Text(), LocKey.Dialog_AddPAC.Text());
                        }
                    }
                    catch (Exception ex)
                    {
                        UiUtils.Error(ex, LocKey.Error_FailedToAddPAC.Text());
                    }
                }
            }
        );

        ShowPacDialog(dialog);
    }

    private void EditPacButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement element && element.Tag is PacConfig pac)
        {
            var content = new PacEditContent(pac);
            var dialog = DialogEx.Create(
                LocKey.Dialog_EditPAC.Text(),
                content,
                DialogButtons.OKCancel,
                (d) =>
                {
                    if (content.ValidateAndCreateConfig())
                    {
                        try
                        {
                            var success = PacManagerService.UpdatePacConfig(pac, content.PacConfig.Name, content.PacConfig.Uri);
                            if (success)
                            {
                                UiUtils.ShowToast(LocKey.Status_PACUpdated.Text(content.PacConfig.Name));
                                LoadPacConfigs();
                                d.DialogResult = true;
                                d.Close();
                            }
                            else
                            {
                                UiUtils.Warning(LocKey.Error_FailedToUpdatePAC.Text(), LocKey.Dialog_EditPAC.Text());
                            }
                        }
                        catch (Exception ex)
                        {
                            UiUtils.Error(ex, LocKey.Error_FailedToUpdatePAC.Text());
                        }
                    }
                }
            );

            ShowPacDialog(dialog);
        }
    }

    private void ShowPacDialog(DialogEx dialog)
    {
        dialog.Width = 440;
        dialog.SizeToContent = SizeToContent.Height;
        dialog.SetResourceReference(FontFamilyProperty, "DesignFontFamily");
        dialog.SetResourceReference(BackgroundProperty, "DesignSurfaceBrush");
        dialog.Resources["PrimaryButtonStyle"] = FindResource("DesignPrimaryButton");
        dialog.Resources["ModernButtonStyle"] = FindResource("DesignButton");
        dialog.ShowDialog();
    }

    private void DeletePacButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement element && element.Tag is PacConfig pac)
        {
            var result = MessageBox.Show(
                LocKey.Confirm_DeletePAC.Text(pac.Name),
                LocKey.Dialog_ConfirmDelete.Text(),
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var success = PacManagerService.RemovePacConfig(pac);
                    if (success)
                    {
                        UiUtils.ShowToast(LocKey.Status_PACDeleted.Text(pac.Name));
                        LoadPacConfigs();
                    }
                    else
                    {
                        UiUtils.Warning(LocKey.Error_FailedToDeletePAC.Text(), LocKey.Dialog_ConfirmDelete.Text());
                    }
                }
                catch (Exception ex)
                {
                    UiUtils.Error(ex, LocKey.Error_FailedToDeletePAC.Text());
                }
            }
        }
    }

    #endregion
}
