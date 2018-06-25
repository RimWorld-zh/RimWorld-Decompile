using System;

namespace Verse
{
	// Token: 0x02000E09 RID: 3593
	public class CompWindSource : ThingComp
	{
		// Token: 0x0400355B RID: 13659
		public float wind;

		// Token: 0x06005168 RID: 20840 RVA: 0x0029C66E File Offset: 0x0029AA6E
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.wind, "wind", 0f, false);
		}
	}
}
