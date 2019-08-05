using System.Threading.Tasks;

namespace Cnblogs.Academy.Application.AccountEventSubscriber
{
    public interface IAccountEventSubscriber
    {
        Task HandleAccountDisabled(AccountDisabledEvent e);

        Task HandleAccountActivated(AccountActivatedEvent e);
    }
}
