using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C05 RID: 3077
	internal struct DebugLine
	{
		// Token: 0x06004353 RID: 17235 RVA: 0x002396C0 File Offset: 0x00237AC0
		public DebugLine(Vector3 a, Vector3 b, int ticksLeft = 100, SimpleColor color = SimpleColor.White)
		{
			this.a = a;
			this.b = b;
			this.deathTick = Find.TickManager.TicksGame + ticksLeft;
			this.color = color;
		}

		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x06004354 RID: 17236 RVA: 0x002396EC File Offset: 0x00237AEC
		public bool Done
		{
			get
			{
				return this.deathTick <= Find.TickManager.TicksGame;
			}
		}

		// Token: 0x06004355 RID: 17237 RVA: 0x00239716 File Offset: 0x00237B16
		public void Draw()
		{
			GenDraw.DrawLineBetween(this.a, this.b, this.color);
		}

		// Token: 0x04002E09 RID: 11785
		public Vector3 a;

		// Token: 0x04002E0A RID: 11786
		public Vector3 b;

		// Token: 0x04002E0B RID: 11787
		private int deathTick;

		// Token: 0x04002E0C RID: 11788
		private SimpleColor color;
	}
}
