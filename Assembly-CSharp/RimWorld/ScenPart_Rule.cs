using System;

namespace RimWorld
{
	// Token: 0x0200063B RID: 1595
	public abstract class ScenPart_Rule : ScenPart
	{
		// Token: 0x060020F7 RID: 8439 RVA: 0x001194F6 File Offset: 0x001178F6
		public override void PostGameStart()
		{
			this.ApplyRule();
		}

		// Token: 0x060020F8 RID: 8440
		protected abstract void ApplyRule();
	}
}
