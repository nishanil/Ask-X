using System;
using System.Threading.Tasks;

namespace AskXhacker
{
	public interface IBluemixDialogService
	{
		void ConfigureDefaultService();
		Task<Message> SendMessage(Message message);

		Task ReConfigureService();
	}
}

