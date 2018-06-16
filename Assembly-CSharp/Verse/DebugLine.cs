using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C09 RID: 3081
	internal struct DebugLine
	{
		// Token: 0x0600434C RID: 17228 RVA: 0x00238320 File Offset: 0x00236720
		public DebugLine(Vector3 a, Vector3 b, int ticksLeft = 100, SimpleColor color = SimpleColor.White)
		{
			this.a = a;
			this.b = b;
			this.deathTick = Find.TickManager.TicksGame + ticksLeft;
			this.color = color;
		}

		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x0600434D RID: 17229 RVA: 0x0023834C File Offset: 0x0023674C
		public bool Done
		{
			get
			{
				return this.deathTick <= Find.TickManager.TicksGame;
			}
		}

		// Token: 0x0600434E RID: 17230 RVA: 0x00238376 File Offset: 0x00236776
		public void Draw()
		{
			GenDraw.DrawLineBetween(this.a, this.b, this.color);
		}

		// Token: 0x04002E01 RID: 11777
		public Vector3 a;

		// Token: 0x04002E02 RID: 11778
		public Vector3 b;

		// Token: 0x04002E03 RID: 11779
		private int deathTick;

		// Token: 0x04002E04 RID: 11780
		private SimpleColor color;
	}
}
