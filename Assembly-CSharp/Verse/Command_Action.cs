using System;
using UnityEngine;

namespace Verse
{
	public class Command_Action : Command
	{
		public Action action;

		public Command_Action()
		{
		}

		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			this.action();
		}
	}
}
