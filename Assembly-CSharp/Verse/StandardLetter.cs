using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000E78 RID: 3704
	public class StandardLetter : ChoiceLetter
	{
		// Token: 0x17000DC2 RID: 3522
		// (get) Token: 0x0600574F RID: 22351 RVA: 0x002CDCD8 File Offset: 0x002CC0D8
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
