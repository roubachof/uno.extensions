using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TemplateStudio.Wizards.Model;
using TemplateStudio.Wizards.Views;

namespace TemplateStudio.Wizards.ViewModel
{
	public class SequentialFlowvViewModel
	{
		public Frame ContentFrame { get; set; }
		public ObservableCollection< SequentialFlow> SequentialFlowList { get; set; }
		private SequentialFlow _SelectSequentialFlow { get; set; }
		public SequentialFlow SelectSequentialFlow {
			get { return _SelectSequentialFlow; }
			set {
				if (_SelectSequentialFlow != value) { _SelectSequentialFlow = value; HandleSelectedItem(); }
			}
		}
		public void HandleSelectedItem()
		{
			if(ContentFrame != null)
			{ 
				ContentFrame.Content = SelectSequentialFlow.getPage;
			}
		}
		public SequentialFlowvViewModel() {
			SequentialFlowList = new ObservableCollection<SequentialFlow>()
			{
				new SequentialFlow() { getPage = new SelectPage(), Name = "Platform" },
				new SequentialFlow() { getPage = new SelectPage(), Name = "Features" },
				new SequentialFlow() { getPage = new SelectPage(), Name = "Extensions" },
				new SequentialFlow() { getPage = new SelectPage(), Name = "Coding Style" },
				new SequentialFlow() { getPage = new SelectPage(), Name = "Framework" },
				new SequentialFlow() { getPage = new SelectPage(), Name = "Architecture" }

			};
		}
	}
}
