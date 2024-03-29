﻿using MailRegWpf.ViewModels;
using System;
using Unity;

namespace MailRegWpf
{
	internal class MainWindowViewModel : BinBase
	{
		public BinBase CurrentViewModel { get; private set; }

		public EmailListViewModel EmailListViewModel { get; }
		public AddEditEmailViewModel AddEditEmailViewModel { get; }

		public RelayCommand NavigationCommand { get; }

		public MainWindowViewModel()
		{
			EmailListViewModel = ContainerHelper.Container.Resolve<EmailListViewModel>();
			AddEditEmailViewModel = ContainerHelper.Container.Resolve<AddEditEmailViewModel>();

			NavigationCommand = new RelayCommand(Navigate);

			EmailListViewModel.OpenEmailClicked += OnOpenEmailClicked;
			AddEditEmailViewModel.EmailSubmited += OnEmailSubmited;
		}

		private async void OnOpenEmailClicked(Guid id)
		{
			await AddEditEmailViewModel.FillEmailData(id);
			CurrentViewModel = AddEditEmailViewModel;
		}

		private void OnEmailSubmited()
		{
			Navigate("emails");
		}

		private async void Navigate(Object parameter)
		{
			if (!(parameter is String target)) throw new ArgumentException();

			switch (target)
			{
				case "emails":
					CurrentViewModel = EmailListViewModel;
					break;
				case "add-edit":
				{
					await AddEditEmailViewModel.FillEmailData(Guid.Empty);
					CurrentViewModel = AddEditEmailViewModel;
				}
					break;
				default:
					CurrentViewModel = CurrentViewModel;
					break;
			}
		}
	}
}