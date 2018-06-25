using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200063C RID: 1596
	public class ScenPart_Rule_DisallowDesignator : ScenPart_Rule
	{
		// Token: 0x060020FB RID: 8443 RVA: 0x0011982D File Offset: 0x00117C2D
		protected override void ApplyRule()
		{
			Current.Game.Rules.SetAllowDesignator(this.def.designatorType, false);
		}
	}
}
