using System;

namespace Verse
{
	// Token: 0x02000D09 RID: 3337
	public class HediffComp_Effecter : HediffComp
	{
		// Token: 0x17000BA9 RID: 2985
		// (get) Token: 0x060049B3 RID: 18867 RVA: 0x002699BC File Offset: 0x00267DBC
		public HediffCompProperties_Effecter Props
		{
			get
			{
				return (HediffCompProperties_Effecter)this.props;
			}
		}

		// Token: 0x060049B4 RID: 18868 RVA: 0x002699DC File Offset: 0x00267DDC
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
