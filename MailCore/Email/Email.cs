using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MailCore
{
	[Serializable]
	public class Email
	{
		[JsonProperty("id")] public Guid Id { get;}
		[JsonProperty("date")] public DateTime DateReg { get; }
		[JsonProperty("sender")] public String Sender { get;}
		[JsonProperty("recipient")] public String Recipient { get;}
		[JsonProperty("subject")] public String Name { get; }
		[JsonProperty("text")] public String Content { get; }
		[JsonProperty("tags")] public List<String> Tags { get; }

		[JsonConstructor]
		public Email(Guid id, DateTime dateReg, String sender, String recipient, String name, String content,
			List<String> tags)
		{
			Id = id;
			Name = name;
			DateReg = dateReg;
			Recipient = recipient;
			Sender = sender;
			Content = content;
			Tags = tags;
		}

		public override String ToString()
		{
			return "email:\n" +
			       $"	id : {Id}\n" +
			       $"	dateReg : {DateReg}\n" +
			       $"	sender : {Sender}\n" +
			       $"	recipient : {Recipient}\n" +
			       $"	context : {Content}";

			
		}

	}
}