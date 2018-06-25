using System;

namespace RimWorld
{
	// Token: 0x0200063B RID: 1595
	public abstract class ScenPart_Rule : ScenPart
	{
		// Token: 0x060020F8 RID: 8440 RVA: 0x0011928E File Offset: 0x0011768E
		public override void PostGameStart()
		{
			this.ApplyRule();
		}

		// Token: 0x060020F9 RID: 8441
		protected abstract void ApplyRule();
	}
}
