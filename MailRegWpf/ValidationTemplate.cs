﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.ComponentModel.DataAnnotations;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace MailRegWpf
{
	internal class ValidationTemplate : IDataErrorInfo, INotifyDataErrorInfo
	{
		private readonly INotifyPropertyChanged _target;
		private readonly ValidationContext _validationContext;
		private readonly List<ValidationResult> _validationResults;

		public Boolean HasErrors => _validationResults.Count > 0;
		public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

		public String Error
		{
			get
			{
				var strings = _validationResults.Select(x => x.ErrorMessage).ToArray();
				return String.Join(Environment.NewLine, strings);
			}
		}

		public ValidationTemplate(INotifyPropertyChanged target)
		{
			_target = target;
			_validationContext = new ValidationContext(target, null, null);
			_validationResults = new List<ValidationResult>();

			Validator.TryValidateObject(target, _validationContext, _validationResults, true);

			target.PropertyChanged += Validate;
		}

		public IEnumerable GetErrors(String propertyName)
		{
			return _validationResults.Where(x => x.MemberNames.Contains(propertyName))
				.Select(x => x.ErrorMessage);
		}

		public String this[String propertyName]
		{
			get
			{
				var strings = _validationResults.Where(x => x.MemberNames.Contains(propertyName))
					.Select(x => x.ErrorMessage).ToArray();

				return String.Join(Environment.NewLine, strings);
			}
		}

		private void Validate(Object sender, PropertyChangedEventArgs e)
		{
			_validationResults.Clear();
			Validator.TryValidateObject(_target, _validationContext, _validationResults, true);

			var hashSet = new HashSet<String>(_validationResults.SelectMany(x => x.MemberNames));

			foreach (String error in hashSet) RaiseErrorsChanged(error);

			if (!HasErrors) RaiseErrorsChanged(null);
		}

		private void RaiseErrorsChanged(String propertyName)
		{
			var handler = ErrorsChanged;
			handler?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
		}
	}
}