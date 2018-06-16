using System;

namespace Verse
{
	// Token: 0x02000E0A RID: 3594
	public class CompWindSource : ThingComp
	{
		// Token: 0x06005152 RID: 20818 RVA: 0x0029ACA6 File Offset: 0x002990A6
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.wind, "wind", 0f, false);
		}

		// Token: 0x0400354F RID: 13647
		public float wind;
	}
}
