using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000E76 RID: 3702
	public class StandardLetter : ChoiceLetter
	{
		// Token: 0x17000DC3 RID: 3523
		// (get) Token: 0x0600574B RID: 22347 RVA: 0x002CDBAC File Offset: 0x002CBFAC
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
