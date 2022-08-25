using System.Threading.Tasks;

namespace Kzrnm.TwitterJikkyo.Models;

public class AccountService
{
    public AccountService(ConfigMaster configMaster)
    {
        ConfigMaster = configMaster;
    }
    public ConfigMaster ConfigMaster { get; }
    private bool CanChangeUser => ConfigMaster.Config.Value.Accounts.Length > 1;
    public async Task ActivatePreviousUserAsync()
    {
        if (!CanChangeUser)
            return;

        await Task.Yield();
        System.Diagnostics.Debug.WriteLine("ActivatePreviousUserAsync");
    }

    public async Task ActivateNextUserAsync()
    {
        if (!CanChangeUser)
            return;

        await Task.Yield();
        System.Diagnostics.Debug.WriteLine("ActivateNextUserAsync");
    }

    public async Task ActivatePreviousImageUserAsync()
    {
        if (!CanChangeUser)
            return;

        await Task.Yield();
        System.Diagnostics.Debug.WriteLine("ActivatePreviousImageUserAsync");
    }

    public async Task ActivateNextImageUserAsync()
    {
        if (!CanChangeUser)
            return;

        await Task.Yield();
        System.Diagnostics.Debug.WriteLine("ActivateNextImageUserAsync");
    }
}
