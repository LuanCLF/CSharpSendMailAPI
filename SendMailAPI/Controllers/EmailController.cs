using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SendMailAPI.Entities;

namespace EmailController.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmailController : ControllerBase
    {
        public class Email
        {
            [Required]
            public required string ToEmail { get; set; }

            [Required]
            public required string Subject { get; set; }

            [Required]
            public required string Body { get; set; }
        }

        private readonly IOptions<EmailSettings> _emailSettings;

        public EmailController(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Email email)
        {
            try
            {
                MailMessage mailMessage = new();
                SmtpClient smtpClient = new("smtp.gmail.com");
                mailMessage.From = new MailAddress(_emailSettings.Value.Email);
                mailMessage.To.Add("pacmancsharp@gmail.com");
                mailMessage.Subject = $"Nova mensagem de {email.ToEmail}";
                mailMessage.Body = $"De: {email.ToEmail} \n" +
                    $"Assunto: {email.Subject} \n" +
                    $"Mensagem: {email.Body}";
                mailMessage.IsBodyHtml = false;
                smtpClient.Port = 587;
                smtpClient.Credentials = new System.Net.NetworkCredential(_emailSettings.Value.Email, _emailSettings.Value.Password);
                smtpClient.EnableSsl = true;
                smtpClient.Send(mailMessage);

                return Ok("Email enviado com sucesso!");
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
