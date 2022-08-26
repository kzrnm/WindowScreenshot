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
    public async Task ActivatePreviousAccountAsync()
    {
        if (!CanChangeUser)
            return;

        await Task.Yield();
        System.Diagnostics.Debug.WriteLine("ActivatePreviousAccountAsync");
    }

    public async Task ActivateNextAccountAsync()
    {
        if (!CanChangeUser)
            return;

        await Task.Yield();
        System.Diagnostics.Debug.WriteLine("ActivateNextAccountAsync");
    }

    public async Task ActivatePreviousImageAccountAsync()
    {
        if (!CanChangeUser)
            return;

        await Task.Yield();
        System.Diagnostics.Debug.WriteLine("ActivatePreviousImageAccountAsync");
    }

    public async Task ActivateNextImageAccountAsync()
    {
        if (!CanChangeUser)
            return;

        await Task.Yield();
        System.Diagnostics.Debug.WriteLine("ActivateNextImageAccountAsync");
    }
}
