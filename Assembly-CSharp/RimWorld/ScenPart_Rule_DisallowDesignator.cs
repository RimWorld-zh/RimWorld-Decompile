using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200063C RID: 1596
	public class ScenPart_Rule_DisallowDesignator : ScenPart_Rule
	{
		// Token: 0x060020FA RID: 8442 RVA: 0x00119A95 File Offset: 0x00117E95
		protected override void ApplyRule()
		{
			Current.Game.Rules.SetAllowDesignator(this.def.designatorType, false);
		}
	}
}
