using System;

namespace RimWorld
{
	// Token: 0x0200063D RID: 1597
	public abstract class ScenPart_Rule : ScenPart
	{
		// Token: 0x060020FA RID: 8442 RVA: 0x0011901A File Offset: 0x0011741A
		public override void PostGameStart()
		{
			this.ApplyRule();
		}

		// Token: 0x060020FB RID: 8443
		protected abstract void ApplyRule();
	}
}
