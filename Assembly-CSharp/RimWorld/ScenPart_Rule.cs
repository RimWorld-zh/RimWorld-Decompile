using System;

namespace RimWorld
{
	// Token: 0x0200063D RID: 1597
	public abstract class ScenPart_Rule : ScenPart
	{
		// Token: 0x060020FC RID: 8444 RVA: 0x00119092 File Offset: 0x00117492
		public override void PostGameStart()
		{
			this.ApplyRule();
		}

		// Token: 0x060020FD RID: 8445
		protected abstract void ApplyRule();
	}
}
