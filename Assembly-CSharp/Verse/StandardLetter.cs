using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000E77 RID: 3703
	public class StandardLetter : ChoiceLetter
	{
		// Token: 0x17000DC0 RID: 3520
		// (get) Token: 0x0600572B RID: 22315 RVA: 0x002CBF9C File Offset: 0x002CA39C
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				yield return base.OK;
				if (this.lookTargets.IsValid())
				{
					yield return base.JumpToLocation;
				}
				yield break;
			}
		}
	}
}
