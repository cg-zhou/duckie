using Duckie.Services.UserConfigs;
using Duckie.Utils.Registry;
using Duckie.Utils.Ui;
using System.Drawing;

namespace Duckie.Services.PacManager;

/// <summary>
/// PAC management service, responsible for PAC configuration switching, application and status management
/// </summary>
public class PacManagerService
{
    /// <summary>
    /// PAC status change event
    /// </summary>
    public static event EventHandler<PacStatusChangedEventArgs> PacStatusChanged;

    /// <summary>
    /// Get all PAC configurations
    /// </summary>
    /// <returns>Array of PAC configurations</returns>
    public static PacConfig[] GetAllPacConfigs()
    {
        var userConfig = UserConfigService.Get();
        return userConfig.Pacs ?? [];
    }

    /// <summary>
    /// Get currently active PAC configuration
    /// </summary>
    /// <returns>Current PAC configuration, null if none</returns>
    public static PacConfig GetCurrentPacConfig()
    {
        var currentPacUrl = RegistryUtils.Pac.Get();
        if (string.IsNullOrWhiteSpace(currentPacUrl))
        {
            return null;
        }

        var allPacs = GetAllPacConfigs();
        return allPacs.FirstOrDefault(p => p.Uri?.Equals(currentPacUrl, StringComparison.OrdinalIgnoreCase) == true);
    }

    public static void RefreshIconBadge()
    {
        var pacConfig = GetCurrentPacConfig();
        var list = GetAllPacConfigs().ToList();
        var item = list.FirstOrDefault(x => x.Name == pacConfig?.Name);
        if (item == null)
        {
            NotifyIconUtils.ResetIco();
            return;
        }

        var index = list.IndexOf(item);
        if (index >= 0)
        {
            var colors = new[] { Color.Green, Color.Orange, Color.Blue };
            var color = colors[index % colors.Length];
            NotifyIconUtils.SetIcoBage(color);
        }
    }

    /// <summary>
    /// Apply PAC configuration
    /// </summary>
    /// <param name="pacConfig">PAC configuration to apply, null to clear PAC</param>
    /// <returns>Whether successful</returns>
    public static bool ApplyPacConfig(PacConfig pacConfig)
    {
        try
        {
            var previousConfig = GetCurrentPacConfig();

            if (pacConfig == null)
            {
                // Clear PAC settings
                RegistryUtils.Pac.Set(string.Empty);
            }
            else
            {
                // Apply new PAC settings
                RegistryUtils.Pac.Set(pacConfig.Uri);
            }

            // Trigger status change event
            PacStatusChanged?.Invoke(null, new PacStatusChangedEventArgs
            {
                PreviousConfig = previousConfig,
                CurrentConfig = pacConfig,
                IsSuccess = true
            });

            RefreshIconBadge();
            return true;
        }
        catch (Exception ex)
        {
            // Trigger failure event
            PacStatusChanged?.Invoke(null, new PacStatusChangedEventArgs
            {
                PreviousConfig = GetCurrentPacConfig(),
                CurrentConfig = pacConfig,
                IsSuccess = false,
                ErrorMessage = ex.Message
            });

            RefreshIconBadge();
            return false;
        }
    }

    /// <summary>
    /// Add new PAC configuration
    /// </summary>
    /// <param name="name">PAC name</param>
    /// <param name="uri">PAC address</param>
    /// <returns>Whether successful</returns>
    public static bool AddPacConfig(string name, string uri)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(uri))
            {
                return false;
            }

            var allPacs = GetAllPacConfigs().ToList();

            // Check if configuration with same name or URI already exists
            if (allPacs.Any(p => p.Name?.Equals(name, StringComparison.OrdinalIgnoreCase) == true ||
                                 p.Uri?.Equals(uri, StringComparison.OrdinalIgnoreCase) == true))
            {
                return false;
            }

            // Add new configuration
            allPacs.Add(new PacConfig { Name = name, Uri = uri });
            UserConfigService.Set(allPacs.ToArray());

            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Update PAC configuration
    /// </summary>
    /// <param name="oldConfig">Original configuration</param>
    /// <param name="newName">New name</param>
    /// <param name="newUri">New URI</param>
    /// <returns>Whether successful</returns>
    public static bool UpdatePacConfig(PacConfig oldConfig, string newName, string newUri)
    {
        try
        {
            if (oldConfig == null || string.IsNullOrWhiteSpace(newName) || string.IsNullOrWhiteSpace(newUri))
            {
                return false;
            }

            var allPacs = GetAllPacConfigs().ToList();
            var index = allPacs.FindIndex(p => p.Name == oldConfig.Name && p.Uri == oldConfig.Uri);

            if (index >= 0)
            {
                allPacs[index] = new PacConfig { Name = newName, Uri = newUri };
                UserConfigService.Set(allPacs.ToArray());
                return true;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Remove PAC configuration
    /// </summary>
    /// <param name="pacConfig">Configuration to remove</param>
    /// <returns>Whether successful</returns>
    public static bool RemovePacConfig(PacConfig pacConfig)
    {
        try
        {
            if (pacConfig == null)
            {
                return false;
            }

            var allPacs = GetAllPacConfigs().ToList();
            var removed = allPacs.RemoveAll(p => p.Name == pacConfig.Name && p.Uri == pacConfig.Uri);

            if (removed > 0)
            {
                UserConfigService.Set(allPacs.ToArray());

                // If the deleted PAC is currently active, clear PAC settings
                var currentConfig = GetCurrentPacConfig();
                if (currentConfig?.Uri == pacConfig.Uri)
                {
                    ApplyPacConfig(null);
                }

                return true;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Get current PAC status description
    /// </summary>
    /// <returns>Status description</returns>
    public static string GetCurrentPacStatus()
    {
        var currentConfig = GetCurrentPacConfig();
        if (currentConfig == null)
        {
            return "No PAC configured";
        }

        return $"Current PAC: {currentConfig.Name}";
    }
}

/// <summary>
/// PAC status change event arguments
/// </summary>
public class PacStatusChangedEventArgs : EventArgs
{
    public PacConfig PreviousConfig { get; set; }
    public PacConfig CurrentConfig { get; set; }
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; }
}
