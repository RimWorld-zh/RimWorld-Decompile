using System;

namespace Verse
{
	// Token: 0x02000D0B RID: 3339
	public class HediffComp_Effecter : HediffComp
	{
		// Token: 0x17000BA8 RID: 2984
		// (get) Token: 0x060049B6 RID: 18870 RVA: 0x00269A98 File Offset: 0x00267E98
		public HediffCompProperties_Effecter Props
		{
			get
			{
				return (HediffCompProperties_Effecter)this.props;
			}
		}

		// Token: 0x060049B7 RID: 18871 RVA: 0x00269AB8 File Offset: 0x00267EB8
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
