namespace Verse
{
	public class HediffComp_Effecter : HediffComp
	{
		public HediffCompProperties_Effecter Props
		{
			get
			{
				return (HediffCompProperties_Effecter)base.props;
			}
		}

		public EffecterDef CurrentStateEffecter()
		{
			if (base.parent.CurStageIndex >= this.Props.severityIndices.min && (this.Props.severityIndices.max < 0 || base.parent.CurStageIndex <= this.Props.severityIndices.max))
			{
				return this.Props.stateEffecter;
			}
			return null;
		}
	}
}
