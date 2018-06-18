using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C08 RID: 3080
	internal struct DebugLine
	{
		// Token: 0x0600434A RID: 17226 RVA: 0x002382F8 File Offset: 0x002366F8
		public DebugLine(Vector3 a, Vector3 b, int ticksLeft = 100, SimpleColor color = SimpleColor.White)
		{
			this.a = a;
			this.b = b;
			this.deathTick = Find.TickManager.TicksGame + ticksLeft;
			this.color = color;
		}

		// Token: 0x17000A90 RID: 2704
		// (get) Token: 0x0600434B RID: 17227 RVA: 0x00238324 File Offset: 0x00236724
		public bool Done
		{
			get
			{
				return this.deathTick <= Find.TickManager.TicksGame;
			}
		}

		// Token: 0x0600434C RID: 17228 RVA: 0x0023834E File Offset: 0x0023674E
		public void Draw()
		{
			GenDraw.DrawLineBetween(this.a, this.b, this.color);
		}

		// Token: 0x04002DFF RID: 11775
		public Vector3 a;

		// Token: 0x04002E00 RID: 11776
		public Vector3 b;

		// Token: 0x04002E01 RID: 11777
		private int deathTick;

		// Token: 0x04002E02 RID: 11778
		private SimpleColor color;
	}
}
