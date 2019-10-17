using System.Threading.Tasks;
using LogisticsSuite.Infrastructure.Dtos;
using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Infrastructure.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsSuite.Backend.Controllers
{
	[ApiController]
	[Route("delay")]
	public class DelayController : Controller
	{
		private readonly IMessageBroker messageBroker;

		public DelayController(IMessageBroker messageBroker) => this.messageBroker = messageBroker;

		[HttpPost]
		public Task PostAsync([FromBody] DelayChangeRequestDto changeRequestDto) => messageBroker.PublishAsync(new DelayChangeRequestedMessage { DelayChangeRequest = changeRequestDto });
	}
}
