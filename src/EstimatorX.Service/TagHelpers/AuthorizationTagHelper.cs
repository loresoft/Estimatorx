using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace EstimatorX.Service.TagHelpers
{
    [HtmlTargetElement("authorize-content")]
    [HtmlTargetElement(Attributes = "authorize")]
    [HtmlTargetElement(Attributes = "authorize-policy")]
    [HtmlTargetElement(Attributes = "authorize-roles")]
    [HtmlTargetElement(Attributes = "authorize-schemes")]
    public class AuthorizationTagHelper : TagHelper, IAuthorizeData
    {
        private readonly IAuthorizationPolicyProvider _policyProvider;
        private readonly IPolicyEvaluator _policyEvaluator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthorizationTagHelper(IHttpContextAccessor httpContextAccessor, IAuthorizationPolicyProvider policyProvider, IPolicyEvaluator policyEvaluator)
        {
            _httpContextAccessor = httpContextAccessor;
            _policyProvider = policyProvider;
            _policyEvaluator = policyEvaluator;
        }

        /// <summary>
        /// Gets or sets the policy name that determines access to the HTML block.
        /// </summary>
        [HtmlAttributeName("authorize-policy")]
        public string Policy { get; set; }

        /// <summary>
        /// Gets or sets a comma delimited list of roles that are allowed to access the HTML  block.
        /// </summary>
        [HtmlAttributeName("authorize-roles")]
        public string Roles { get; set; }

        /// <summary>
        /// Gets or sets a comma delimited list of schemes from which user information is constructed.
        /// </summary>
        [HtmlAttributeName("authorize-schemes")]
        public string AuthenticationSchemes { get; set; }

        /// <summary>
        /// Gets or sets the owner to authorize.
        /// </summary>
        /// <value>
        /// The owner to authorize.
        /// </value>
        [HtmlAttributeName("authorize-owner")]
        public string Owner { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var ownerResult = EvaluateOwner();
            var authorizeResult = await EvaluateAuthorization();

            if (!ownerResult && !authorizeResult.Succeeded)
                output.SuppressOutput();
        }

        private bool EvaluateOwner()
        {
            var currentUser = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            if (string.IsNullOrEmpty(currentUser) || string.IsNullOrEmpty(Owner))
                return false;

            return string.Equals(currentUser, Owner, StringComparison.OrdinalIgnoreCase);
        }

        private async Task<PolicyAuthorizationResult> EvaluateAuthorization()
        {
            var policy = await AuthorizationPolicy.CombineAsync(_policyProvider, new[] { this });

            var authenticateResult = await _policyEvaluator.AuthenticateAsync(policy, _httpContextAccessor.HttpContext);

            var authorizeResult = await _policyEvaluator.AuthorizeAsync(policy, authenticateResult, _httpContextAccessor.HttpContext, null);

            return authorizeResult;
        }
    }
}
