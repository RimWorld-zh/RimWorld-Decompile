using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200063A RID: 1594
	public class ScenPart_Rule_DisallowDesignator : ScenPart_Rule
	{
		// Token: 0x060020F7 RID: 8439 RVA: 0x001196DD File Offset: 0x00117ADD
		protected override void ApplyRule()
		{
			Current.Game.Rules.SetAllowDesignator(this.def.designatorType, false);
		}
	}
}
