using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C08 RID: 3080
	internal struct DebugLine
	{
		// Token: 0x04002E10 RID: 11792
		public Vector3 a;

		// Token: 0x04002E11 RID: 11793
		public Vector3 b;

		// Token: 0x04002E12 RID: 11794
		private int deathTick;

		// Token: 0x04002E13 RID: 11795
		private SimpleColor color;

		// Token: 0x06004356 RID: 17238 RVA: 0x00239A7C File Offset: 0x00237E7C
		public DebugLine(Vector3 a, Vector3 b, int ticksLeft = 100, SimpleColor color = SimpleColor.White)
		{
			this.a = a;
			this.b = b;
			this.deathTick = Find.TickManager.TicksGame + ticksLeft;
			this.color = color;
		}

		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x06004357 RID: 17239 RVA: 0x00239AA8 File Offset: 0x00237EA8
		public bool Done
		{
			get
			{
				return this.deathTick <= Find.TickManager.TicksGame;
			}
		}

		// Token: 0x06004358 RID: 17240 RVA: 0x00239AD2 File Offset: 0x00237ED2
		public void Draw()
		{
			GenDraw.DrawLineBetween(this.a, this.b, this.color);
		}
	}
}
