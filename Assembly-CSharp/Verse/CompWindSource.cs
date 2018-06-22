using System;

namespace Verse
{
	// Token: 0x02000E06 RID: 3590
	public class CompWindSource : ThingComp
	{
		// Token: 0x06005164 RID: 20836 RVA: 0x0029C262 File Offset: 0x0029A662
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.wind, "wind", 0f, false);
		}

		// Token: 0x04003554 RID: 13652
		public float wind;
	}
}
