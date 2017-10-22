namespace Verse
{
	public class PawnCapacityModifier
	{
		public PawnCapacityDef capacity;

		public float offset = 0f;

		public float setMax = 999f;

		public float postFactor = 1f;

		public bool SetMaxDefined
		{
			get
			{
				return this.setMax != 999.0;
			}
		}
	}
}
