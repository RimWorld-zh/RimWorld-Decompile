using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E66 RID: 3686
	public class Command_Action : Command
	{
		// Token: 0x060056BC RID: 22204 RVA: 0x002CA01B File Offset: 0x002C841B
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			this.action();
		}

		// Token: 0x0400397C RID: 14716
		public Action action;
	}
}
