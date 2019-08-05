using System.Threading.Tasks;
using Cnblogs.Academy.Application.ScheduleAppService;
using DotNetCore.CAP;

namespace Cnblogs.Academy.Application.AccountEventSubscriber
{
    public class AccountEventSubscriber : IAccountEventSubscriber, ICapSubscribe
    {
        private readonly IScheduleService _scheduleService;

        public AccountEventSubscriber(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [CapSubscribe("Account.Disabled")]
        public async Task HandleAccountDisabled(AccountDisabledEvent e)
        {
            await _scheduleService.DeleteSchedulesByUserId(e.UserId);
        }

        [CapSubscribe("Account.Activated")]
        public async Task HandleAccountActivated(AccountActivatedEvent e)
        {
            await _scheduleService.RestoreSchedulesByUserId(e.UserId);
        }
    }
}
