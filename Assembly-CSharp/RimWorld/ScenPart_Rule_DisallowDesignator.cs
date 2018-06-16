using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200063E RID: 1598
	public class ScenPart_Rule_DisallowDesignator : ScenPart_Rule
	{
		// Token: 0x060020FD RID: 8445 RVA: 0x001195B9 File Offset: 0x001179B9
		protected override void ApplyRule()
		{
			Current.Game.Rules.SetAllowDesignator(this.def.designatorType, false);
		}
	}
}
