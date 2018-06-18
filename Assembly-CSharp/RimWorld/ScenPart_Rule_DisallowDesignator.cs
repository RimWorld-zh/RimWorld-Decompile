using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200063E RID: 1598
	public class ScenPart_Rule_DisallowDesignator : ScenPart_Rule
	{
		// Token: 0x060020FF RID: 8447 RVA: 0x00119631 File Offset: 0x00117A31
		protected override void ApplyRule()
		{
			Current.Game.Rules.SetAllowDesignator(this.def.designatorType, false);
		}
	}
}
