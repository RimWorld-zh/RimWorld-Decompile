using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C07 RID: 3079
	internal struct DebugLine
	{
		// Token: 0x04002E09 RID: 11785
		public Vector3 a;

		// Token: 0x04002E0A RID: 11786
		public Vector3 b;

		// Token: 0x04002E0B RID: 11787
		private int deathTick;

		// Token: 0x04002E0C RID: 11788
		private SimpleColor color;

		// Token: 0x06004356 RID: 17238 RVA: 0x0023979C File Offset: 0x00237B9C
		public DebugLine(Vector3 a, Vector3 b, int ticksLeft = 100, SimpleColor color = SimpleColor.White)
		{
			this.a = a;
			this.b = b;
			this.deathTick = Find.TickManager.TicksGame + ticksLeft;
			this.color = color;
		}

		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x06004357 RID: 17239 RVA: 0x002397C8 File Offset: 0x00237BC8
		public bool Done
		{
			get
			{
				return this.deathTick <= Find.TickManager.TicksGame;
			}
		}

		// Token: 0x06004358 RID: 17240 RVA: 0x002397F2 File Offset: 0x00237BF2
		public void Draw()
		{
			GenDraw.DrawLineBetween(this.a, this.b, this.color);
		}
	}
}
