using System;

namespace Verse
{
	// Token: 0x02000D0D RID: 3341
	public class HediffComp_Effecter : HediffComp
	{
		// Token: 0x17000BA8 RID: 2984
		// (get) Token: 0x060049A4 RID: 18852 RVA: 0x002685B0 File Offset: 0x002669B0
		public HediffCompProperties_Effecter Props
		{
			get
			{
				return (HediffCompProperties_Effecter)this.props;
			}
		}

		// Token: 0x060049A5 RID: 18853 RVA: 0x002685D0 File Offset: 0x002669D0
		public EffecterDef CurrentStateEffecter()
		{
			EffecterDef result;
			if (this.parent.CurStageIndex >= this.Props.severityIndices.min && (this.Props.severityIndices.max < 0 || this.parent.CurStageIndex <= this.Props.severityIndices.max))
			{
				result = this.Props.stateEffecter;
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}
