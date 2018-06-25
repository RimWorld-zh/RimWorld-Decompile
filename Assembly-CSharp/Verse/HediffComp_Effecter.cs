using System;

namespace Verse
{
	public class HediffComp_Effecter : HediffComp
	{
		public HediffComp_Effecter()
		{
		}

		public HediffCompProperties_Effecter Props
		{
			get
			{
				return (HediffCompProperties_Effecter)this.props;
			}
		}

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
