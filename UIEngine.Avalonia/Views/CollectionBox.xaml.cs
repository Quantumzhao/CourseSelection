﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CourseScheduler.Core.DataStrucures;
using System;
using UIEngine.Nodes;

namespace UIEngine.Avalonia.Views
{
	public class CollectionBox : AbstractCollectionBox
	{
		public CollectionBox()
		{
			this.InitializeComponent();
			this.FindControl<ListBox>("MainListBox").SelectionChanged += OnSelectionChanged;
			_NextControl = this.FindControl<ContentControl>("NextControl");
			this.PropertyChanged += OnAnyPropertiesChanged;
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}

		private void OnAnyPropertiesChanged(object sender, AvaloniaPropertyChangedEventArgs e)
		{
			switch (e.Property.Name)
			{
				case nameof(NextNode):
					OnNextNodeChanged();
					break;

				default:
					break;
			}
		}

		private void OnNextNodeChanged()
		{
			Box control = null;

			if (NextNode is ObjectNode objectNode)
			{
				control = ToSpecificBox(objectNode);
			}

			_NextControl.Content = control;
		}
	}
}
