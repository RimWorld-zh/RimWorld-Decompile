using System;

namespace Verse
{
	// Token: 0x02000D0C RID: 3340
	public class HediffComp_Effecter : HediffComp
	{
		// Token: 0x17000BA7 RID: 2983
		// (get) Token: 0x060049A2 RID: 18850 RVA: 0x00268588 File Offset: 0x00266988
		public HediffCompProperties_Effecter Props
		{
			get
			{
				return (HediffCompProperties_Effecter)this.props;
			}
		}

		// Token: 0x060049A3 RID: 18851 RVA: 0x002685A8 File Offset: 0x002669A8
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
