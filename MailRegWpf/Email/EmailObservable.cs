﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MailRegWpf
{
	internal class EmailObservable : BinBase
	{
		public Guid Id { get; }
		public DateTime Date { get; set; }
		public String Sender { get; set; }
		public String Recipient { get; set; }
		public String Subject { get; set; }
		public String Text { get; set; }
		public ObservableCollection<String> Tags { get; }

		public EmailObservable(Guid id, DateTime date, String sender, String recipient, String subject, String text,
			List<String> tags)
		{
			Id = id;
			Date = date;
			Sender = sender;
			Recipient = recipient;
			Subject = subject;
			Text = text;
			Tags = new ObservableCollection<String>(tags);
		}

		public EmailObservable(Email email)
			: this(email.Id, email.Date, email.Sender, email.Recipient, email.Subject, email.Text, email.Tags)
		{
		}
	}
}