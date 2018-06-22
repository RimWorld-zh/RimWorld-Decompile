using System;

namespace RimWorld
{
	// Token: 0x02000639 RID: 1593
	public abstract class ScenPart_Rule : ScenPart
	{
		// Token: 0x060020F4 RID: 8436 RVA: 0x0011913E File Offset: 0x0011753E
		public override void PostGameStart()
		{
			this.ApplyRule();
		}

		// Token: 0x060020F5 RID: 8437
		protected abstract void ApplyRule();
	}
}
