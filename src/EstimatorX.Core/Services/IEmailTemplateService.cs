using System.Threading.Tasks;
using EstimatorX.Core.Models;

namespace EstimatorX.Core.Services
{
    public interface IEmailTemplateService
    {
        Task<bool> SendPasswordlessLoginEmail(UserPasswordlessEmail loginEmail);
        Task<bool> SendResetPasswordEmail(UserResetPasswordEmail resetPassword);

        Task<bool> SendTemplate<TModel>(EmailTemplate emailTemplate, TModel emailModel) where TModel : EmailModelBase;
    }
}