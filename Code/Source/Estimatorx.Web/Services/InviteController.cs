using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Http;
using Estimatorx.Core.Security;
using Microsoft.AspNet.Identity;
using MongoDB.Driver.Linq;

namespace Estimatorx.Web.Services
{
    [Authorize]
    [RoutePrefix("api/Invite")]
    public class InviteController : ApiController
    {
        private readonly IInviteRepository _inviteRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOrganizationRepository _organizationRepository;

        public InviteController(
            IInviteRepository inviteRepository, 
            IUserRepository userRepository,
            IOrganizationRepository organizationRepository)
        {
            _inviteRepository = inviteRepository;
            _userRepository = userRepository;
            _organizationRepository = organizationRepository;
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            Invite invite;
            User user;
            if (!HasAccess(id, out invite, out user))
                return Unauthorized();

            if (invite == null)
                return NotFound();

            return Ok(invite);
        }

        [HttpGet]
        [Route("Organization/{organizationId}")]
        public IHttpActionResult GetOrganization(string organizationId)
        {
            if (string.IsNullOrEmpty(organizationId))
                return BadRequest();

            string userId = User.Identity.GetUserId();
            var user = _userRepository.Find(userId);
            if (user == null || !user.Organizations.Contains(organizationId))
                return Unauthorized();
            
            var results = _inviteRepository.All()
                 .Where(t => t.OrganizationId == organizationId);

            return Ok(results);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody]Invite value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!HasAccess(value.Id))
                return Unauthorized();

            var invite = _inviteRepository.Save(value);
            if (invite == null)
                return NotFound();

            return Ok(invite);
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            if (!HasAccess(id))
                return Unauthorized();

            _inviteRepository.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpGet]
        [Route("{id}/Send")]
        public IHttpActionResult Send(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            Invite invite;
            User user;
            if (!HasAccess(id, out invite,out user))
                return Unauthorized();

            if (invite == null)
                return NotFound();

            var o = _organizationRepository.Find(invite.OrganizationId);
            if(o == null)
                return BadRequest("Invalid Organization for invite");
            
            string link = Url.Link("Invite", new { id = invite.Id, key = invite.SecurityKey });

            string subject = string.Format("Welcome to the {0} organization on EstimatorX.com", o.Name);
            string body = string.Format(
                "<p>{0} invited you to join the {1} organization on EstimatorX.com.</p>" +
                "<p>EstimatorX is a simple project estimation application.</p>" +
                "<p>{2}</p>", user.Name, o.Name, link);

            var mailMessage = new MailMessage();
            mailMessage.To.Add(invite.Email);
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;

            var smtpClient = new SmtpClient();            
            smtpClient.Send(mailMessage);

            return Ok(invite);
        }

        private bool HasAccess(string id)
        {
            Invite invite;
            User user;
            return HasAccess(id, out invite,out user);
        }

        private bool HasAccess(string id, out Invite invite, out User user)
        {
            invite = null;
            string userId = User.Identity.GetUserId();
            user = _userRepository.Find(userId);
            if (user == null)
                return false;

            invite = _inviteRepository.Find(id);
            if (invite == null)
                return true; // allow create

            // user must be member 
            return invite.OrganizationId == user.Id
                || user.Organizations.Contains(invite.OrganizationId);
        }

    }
}