using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E66 RID: 3686
	public class Command_Action : Command
	{
		// Token: 0x04003989 RID: 14729
		public Action action;

		// Token: 0x060056DE RID: 22238 RVA: 0x002CBD57 File Offset: 0x002CA157
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			this.action();
		}
	}
}
