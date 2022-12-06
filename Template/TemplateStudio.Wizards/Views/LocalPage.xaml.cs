using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TemplateStudio.Wizards.Model;
using TemplateStudio.Wizards.ViewModel;

namespace TemplateStudio.Wizards.Views
{
	/// <summary>
	/// Interaction logic for LocalPage.xaml
	/// </summary>
	public partial class LocalPage : Window
	{
		public SequentialFlowvViewModel SequentialFlowvViewModel { get; set; }
		public LocalPage()
		{
			SequentialFlowvViewModel = new SequentialFlowvViewModel();
			SequentialFlowvViewModel.ContentFrame = stepFrame;

			DataContext = this;
			InitializeComponent();
		}
	}
}
