using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E67 RID: 3687
	public class Command_Action : Command
	{
		// Token: 0x04003991 RID: 14737
		public Action action;

		// Token: 0x060056DE RID: 22238 RVA: 0x002CBF43 File Offset: 0x002CA343
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			this.action();
		}
	}
}
