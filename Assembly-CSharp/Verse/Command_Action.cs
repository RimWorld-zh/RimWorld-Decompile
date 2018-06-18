using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E65 RID: 3685
	public class Command_Action : Command
	{
		// Token: 0x060056BA RID: 22202 RVA: 0x002CA01B File Offset: 0x002C841B
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			this.action();
		}

		// Token: 0x0400397A RID: 14714
		public Action action;
	}
}
