using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000E79 RID: 3705
	public class StandardLetter : ChoiceLetter
	{
		// Token: 0x17000DC2 RID: 3522
		// (get) Token: 0x0600574F RID: 22351 RVA: 0x002CDEC4 File Offset: 0x002CC2C4
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
