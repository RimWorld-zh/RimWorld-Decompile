using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E64 RID: 3684
	public class Command_Action : Command
	{
		// Token: 0x04003989 RID: 14729
		public Action action;

		// Token: 0x060056DA RID: 22234 RVA: 0x002CBC2B File Offset: 0x002CA02B
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			this.action();
		}
	}
}
