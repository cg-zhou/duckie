using Duckie.Services.PacManager;
using Duckie.Services.UserConfigs;
using Duckie.Utils.Localization;
using Duckie.Utils.Ui;
using Duckie.Views.Common;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Duckie.Views;

public partial class PacManageView : UserControl
{
    public PacManageView()
    {
        InitializeComponent();
        InitializePacManagement();
    }

    /// <summary>
    /// Initialize PAC management
    /// </summary>
    private void InitializePacManagement()
    {
        // Subscribe to PAC status change events
        PacManagerService.PacStatusChanged += OnPacStatusChanged;

        // Load PAC configurations
        LoadPacConfigs();

        // Update status display
        UpdateCurrentStatus();
    }

    /// <summary>
    /// Load PAC configurations
    /// </summary>
    private void LoadPacConfigs()
    {
        try
        {
            PacListPanel.Children.Clear();

            var currentPac = PacManagerService.GetCurrentPacConfig();
            var allPacs = PacManagerService.GetAllPacConfigs();

            // Add "No PAC" option
            var noPacItem = CreatePacItem(null, currentPac == null);
            PacListPanel.Children.Add(noPacItem);

            // Add all PAC configurations
            foreach (var pac in allPacs)
            {
                var isActive = currentPac?.Uri == pac.Uri;
                var pacItem = CreatePacItem(pac, isActive);
                PacListPanel.Children.Add(pacItem);
            }
        }
        catch (Exception ex)
        {
            UiUtils.Error(ex, LocUtils.GetString("Error_FailedToLoadPAC"));
        }
    }

    /// <summary>
    /// Update current status display
    /// </summary>
    private void UpdateCurrentStatus()
    {
        // Status display removed from UI
    }

    /// <summary>
    /// Create PAC configuration item UI
    /// </summary>
    private Border CreatePacItem(PacConfig pac, bool isActive)
    {
        var border = new Border
        {
            Background = Brushes.White,
            BorderBrush = new SolidColorBrush(Color.FromRgb(0xDE, 0xE2, 0xE6)),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(6),
            Margin = new Thickness(0, 0, 0, 10),
            Padding = new Thickness(15, 12, 15, 12)
        };

        var grid = new Grid();
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

        // Status indicator
        var statusIndicator = CreateStatusIndicator(isActive);
        statusIndicator.VerticalAlignment = VerticalAlignment.Center;
        statusIndicator.HorizontalAlignment = HorizontalAlignment.Center;
        Grid.SetColumn(statusIndicator, 0);
        grid.Children.Add(statusIndicator);

        // Content area
        var contentPanel = new StackPanel { Orientation = Orientation.Vertical };

        if (pac == null)
        {
            // "No PAC" item
            var titleText = new TextBlock
            {
                Text = LocUtils.GetString("Label_NoPAC"),
                FontSize = 14,
                FontWeight = FontWeights.SemiBold,
                Foreground = new SolidColorBrush(Color.FromRgb(0x33, 0x33, 0x33))
            };
            contentPanel.Children.Add(titleText);

            var descText = new TextBlock
            {
                Text = LocUtils.GetString("Label_DirectConnection"),
                FontSize = 12,
                Foreground = new SolidColorBrush(Color.FromRgb(0x66, 0x66, 0x66)),
                Margin = new Thickness(0, 2, 0, 0)
            };
            contentPanel.Children.Add(descText);
        }
        else
        {
            // PAC configuration item
            var titleText = new TextBlock
            {
                Text = pac.Name,
                FontSize = 14,
                FontWeight = FontWeights.SemiBold,
                Foreground = new SolidColorBrush(Color.FromRgb(0x33, 0x33, 0x33))
            };
            contentPanel.Children.Add(titleText);

            var uriText = new TextBlock
            {
                Text = pac.Uri,
                FontSize = 12,
                Foreground = new SolidColorBrush(Color.FromRgb(0x66, 0x66, 0x66)),
                Margin = new Thickness(0, 2, 0, 0),
                TextTrimming = TextTrimming.CharacterEllipsis
            };
            contentPanel.Children.Add(uriText);
        }

        Grid.SetColumn(contentPanel, 1);
        grid.Children.Add(contentPanel);

        // Action buttons
        if (pac != null)
        {
            var buttonPanel = new StackPanel { Orientation = Orientation.Horizontal };

            var editButton = new Button
            {
                Content = LocUtils.GetString("Btn_Edit"),
                Width = 60,
                Height = 32,
                Margin = new Thickness(0, 0, 8, 0),
                Tag = pac
            };
            editButton.SetResourceReference(Button.StyleProperty, "ModernButtonStyle");
            editButton.Click += EditPacButton_Click;
            buttonPanel.Children.Add(editButton);

            var deleteButton = new Button
            {
                Content = LocUtils.GetString("Btn_Delete"),
                Width = 70,
                Height = 32,
                Tag = pac
            };
            deleteButton.SetResourceReference(Button.StyleProperty, "ModernButtonStyle");
            deleteButton.Click += DeletePacButton_Click;
            buttonPanel.Children.Add(deleteButton);

            Grid.SetColumn(buttonPanel, 2);
            grid.Children.Add(buttonPanel);
        }

        border.Child = grid;

        // Add click event to apply PAC
        border.MouseLeftButtonUp += (s, e) => ApplyPac(pac);
        border.Cursor = System.Windows.Input.Cursors.Hand;

        return border;
    }

    /// <summary>
    /// Create status indicator
    /// </summary>
    private Canvas CreateStatusIndicator(bool isActive)
    {
        var canvas = new Canvas { Width = 16, Height = 16 };

        // Outer circle
        var outerCircle = new Ellipse
        {
            Width = 12,
            Height = 12,
            Stroke = new SolidColorBrush(Color.FromRgb(0x00, 0x7B, 0xFF)),
            StrokeThickness = 2,
            Fill = Brushes.Transparent
        };
        Canvas.SetLeft(outerCircle, 2);
        Canvas.SetTop(outerCircle, 2);
        canvas.Children.Add(outerCircle);

        if (isActive)
        {
            // Inner circle (green dot)
            var innerCircle = new Ellipse
            {
                Width = 6,
                Height = 6,
                Fill = new SolidColorBrush(Color.FromRgb(0x28, 0xA7, 0x45))
            };
            Canvas.SetLeft(innerCircle, 5);
            Canvas.SetTop(innerCircle, 5);
            canvas.Children.Add(innerCircle);
        }

        return canvas;
    }

    /// <summary>
    /// Apply PAC configuration
    /// </summary>
    private void ApplyPac(PacConfig pac)
    {
        try
        {
            var success = PacManagerService.ApplyPacConfig(pac);
            if (success)
            {
                var message = pac == null
                    ? LocUtils.GetString("Status_PACCleared")
                    : LocUtils.GetString("Status_PACSwitch", pac.Name);
                StatusText.Text = message;
                LoadPacConfigs(); // Reload to update status
            }
        }
        catch (Exception ex)
        {
            UiUtils.Error(ex, LocUtils.GetString("Error_FailedToUpdatePAC"));
        }
    }

    /// <summary>
    /// PAC status change event handler
    /// </summary>
    private void OnPacStatusChanged(object sender, PacStatusChangedEventArgs e)
    {
        Dispatcher.Invoke(() =>
        {
            UpdateCurrentStatus();
            LoadPacConfigs(); // Reload to update status

            if (e.IsSuccess)
            {
                var message = e.CurrentConfig == null
                    ? LocUtils.GetString("Status_PACCleared")
                    : LocUtils.GetString("Status_PACSwitch", e.CurrentConfig.Name);
                StatusText.Text = message;
            }
            else
            {
                StatusText.Text = LocUtils.GetString("Status_PACFailed", e.ErrorMessage);
            }
        });
    }

    #region Event Handlers

    private void AddPacButton_Click(object sender, RoutedEventArgs e)
    {
        var content = new PacEditContent();
        var dialog = DialogEx.Create(
            LocUtils.GetString("Dialog_AddPAC"),
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
                            StatusText.Text = LocUtils.GetString("Status_PACAdded", content.PacConfig.Name);
                            LoadPacConfigs();
                            d.DialogResult = true;
                            d.Close();
                        }
                        else
                        {
                            UiUtils.Warning(LocUtils.GetString("Error_FailedToAddPAC"), LocUtils.GetString("Dialog_AddPAC"));
                        }
                    }
                    catch (Exception ex)
                    {
                        UiUtils.Error(ex, LocUtils.GetString("Error_FailedToAddPAC"));
                    }
                }
            }
        );

        dialog.ShowDialog();
    }

    private void EditPacButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is PacConfig pac)
        {
            var content = new PacEditContent(pac);
            var dialog = DialogEx.Create(
                LocUtils.GetString("Dialog_EditPAC"),
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
                                StatusText.Text = LocUtils.GetString("Status_PACUpdated", content.PacConfig.Name);
                                LoadPacConfigs();
                                d.DialogResult = true;
                                d.Close();
                            }
                            else
                            {
                                UiUtils.Warning(LocUtils.GetString("Error_FailedToUpdatePAC"), LocUtils.GetString("Dialog_EditPAC"));
                            }
                        }
                        catch (Exception ex)
                        {
                            UiUtils.Error(ex, LocUtils.GetString("Error_FailedToUpdatePAC"));
                        }
                    }
                }
            );

            dialog.ShowDialog();
        }
    }

    private void DeletePacButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is PacConfig pac)
        {
            var result = MessageBox.Show(
                LocUtils.GetString("Confirm_DeletePAC", pac.Name),
                LocUtils.GetString("Dialog_ConfirmDelete"),
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var success = PacManagerService.RemovePacConfig(pac);
                    if (success)
                    {
                        StatusText.Text = LocUtils.GetString("Status_PACDeleted", pac.Name);
                        LoadPacConfigs();
                    }
                    else
                    {
                        UiUtils.Warning(LocUtils.GetString("Error_FailedToDeletePAC"), LocUtils.GetString("Dialog_ConfirmDelete"));
                    }
                }
                catch (Exception ex)
                {
                    UiUtils.Error(ex, LocUtils.GetString("Error_FailedToDeletePAC"));
                }
            }
        }
    }

    #endregion
}
