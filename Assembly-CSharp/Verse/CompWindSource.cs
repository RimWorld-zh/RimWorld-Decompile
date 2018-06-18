using System;

namespace Verse
{
	// Token: 0x02000E09 RID: 3593
	public class CompWindSource : ThingComp
	{
		// Token: 0x06005150 RID: 20816 RVA: 0x0029AC86 File Offset: 0x00299086
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.wind, "wind", 0f, false);
		}

		// Token: 0x0400354D RID: 13645
		public float wind;
	}
}
