using System;

namespace Verse
{
	// Token: 0x02000E08 RID: 3592
	public class CompWindSource : ThingComp
	{
		// Token: 0x04003554 RID: 13652
		public float wind;

		// Token: 0x06005168 RID: 20840 RVA: 0x0029C38E File Offset: 0x0029A78E
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.wind, "wind", 0f, false);
		}
	}
}
