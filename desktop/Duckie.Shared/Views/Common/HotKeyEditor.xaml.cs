using Duckie.Shared.Utils.Localization;
using Duckie.Utils.HotKeys;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Keys = System.Windows.Forms.Keys;

namespace Duckie.Shared.Views.Common;

public partial class HotKeyEditor : UserControl
{
    private string saveError;
    public static readonly DependencyProperty OperationProperty = DependencyProperty.Register(
        nameof(Operation), typeof(HotKeyAction), typeof(HotKeyEditor),
        new PropertyMetadata(null, (sender, _) => ((HotKeyEditor)sender).Refresh()));

    public HotKeyAction Operation
    {
        get => (HotKeyAction)GetValue(OperationProperty);
        set => SetValue(OperationProperty, value);
    }

    public HotKeyEditor()
    {
        InitializeComponent();
        Refresh();
        Loaded += (_, _) =>
        {
            HotKeyBindings.Changed += OnChanged;
            EmbeddedLocalizationManager.Instance.LanguageChanged += OnChanged;
            Refresh();
        };
        Unloaded += (_, _) =>
        {
            HotKeyBindings.Changed -= OnChanged;
            EmbeddedLocalizationManager.Instance.LanguageChanged -= OnChanged;
        };
        IsVisibleChanged += (_, _) => Refresh();
    }

    private void OnChanged(object sender, EventArgs e)
    {
        saveError = null;
        Refresh();
    }

    private void KeyInput_FocusChanged(object sender, KeyboardFocusChangedEventArgs e) => Refresh();

    private void Refresh()
    {
        if (KeyInput == null) return;
        var binding = Operation == null ? new HotKeyBinding() : HotKeyBindings.Get(Operation);
        var recording = KeyInput.IsKeyboardFocused && Operation != null;
        KeyCaps.ItemsSource = GetKeyLabels(binding);
        KeyCaps.Visibility = !recording && !binding.IsEmpty ? Visibility.Visible : Visibility.Collapsed;
        IdleText.Text = recording ? LocKey.Settings_HotKey_Record.Text() : LocKey.Settings_HotKey_Unassigned.Text();
        IdleText.Visibility = recording || binding.IsEmpty ? Visibility.Visible : Visibility.Collapsed;
        RecordingHint.Visibility = recording ? Visibility.Visible : Visibility.Collapsed;
        System.Windows.Automation.AutomationProperties.SetName(KeyInput,
            recording ? LocKey.Settings_HotKey_Record.Text() : binding.IsEmpty ? LocKey.Settings_HotKey_Unassigned.Text() : binding.ToString());
        KeyInput.IsEnabled = Operation != null;
        ClearButton.IsEnabled = Operation != null && !binding.IsEmpty;
        ClearButton.Visibility = binding.IsEmpty ? Visibility.Hidden : Visibility.Visible;
        var registered = Operation != null && binding.Equals(HotKeyManager.GetRegisteredBinding(Operation.Id));
        RegistrationStatus.Text = saveError ?? (!binding.IsEmpty && !registered
            ? LocKey.Settings_HotKey_Unregistered.Text() : "");
        RegistrationStatus.Visibility = string.IsNullOrEmpty(RegistrationStatus.Text) ? Visibility.Collapsed : Visibility.Visible;
    }

    private static string[] GetKeyLabels(HotKeyBinding binding)
    {
        if (binding.IsEmpty) return Array.Empty<string>();
        var labels = new List<string>();
        if (binding.Modifiers.HasFlag(KeyModifiers.Control)) labels.Add("Ctrl");
        if (binding.Modifiers.HasFlag(KeyModifiers.Alt)) labels.Add("Alt");
        if (binding.Modifiers.HasFlag(KeyModifiers.Shift)) labels.Add("Shift");
        if (binding.Modifiers.HasFlag(KeyModifiers.Win)) labels.Add("Win");
        // Format the actual key separately: '+' must remain a keycap of its own.
        labels.Add(new HotKeyBinding(KeyModifiers.None, binding.Key).ToString());
        return labels.ToArray();
    }

    private void KeyInput_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Tab) return;
        e.Handled = true;
        if (e.IsRepeat || Operation == null) return;
        var key = e.Key == Key.System ? e.SystemKey : e.Key == Key.ImeProcessed ? e.ImeProcessedKey : e.Key;
        if (key == Key.Escape) { Keyboard.ClearFocus(); return; }
        if (key == Key.Back || key == Key.Delete) { Save(new HotKeyBinding()); return; }
        if (key == Key.LeftCtrl || key == Key.RightCtrl || key == Key.LeftAlt || key == Key.RightAlt
            || key == Key.LeftShift || key == Key.RightShift || key == Key.LWin || key == Key.RWin) return;
        var modifiers = KeyModifiers.None;
        if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control)) modifiers |= KeyModifiers.Control;
        if (Keyboard.Modifiers.HasFlag(ModifierKeys.Alt)) modifiers |= KeyModifiers.Alt;
        if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift)) modifiers |= KeyModifiers.Shift;
        if (Keyboard.Modifiers.HasFlag(ModifierKeys.Windows)) modifiers |= KeyModifiers.Win;
        var nativeKey = (Keys)KeyInterop.VirtualKeyFromKey(key);
        if (nativeKey != Keys.None) Save(new HotKeyBinding(modifiers, nativeKey));
    }

    private void ClearButton_Click(object sender, RoutedEventArgs e) => Save(new HotKeyBinding());

    private void Save(HotKeyBinding binding)
    {
        if (Operation == null) return;
        saveError = null;
        try { HotKeyBindings.Save(Operation, binding); }
        catch (Exception ex) { saveError = ex.Message; }
        finally
        {
            Keyboard.ClearFocus();
            Refresh();
        }
    }
}
