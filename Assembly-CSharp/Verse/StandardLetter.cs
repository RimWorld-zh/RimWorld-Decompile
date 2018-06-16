using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000E78 RID: 3704
	public class StandardLetter : ChoiceLetter
	{
		// Token: 0x17000DC1 RID: 3521
		// (get) Token: 0x0600572D RID: 22317 RVA: 0x002CBF9C File Offset: 0x002CA39C
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
