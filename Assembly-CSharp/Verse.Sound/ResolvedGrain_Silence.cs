namespace Verse.Sound
{
	public class ResolvedGrain_Silence : ResolvedGrain
	{
		public AudioGrain_Silence sourceGrain;

		public ResolvedGrain_Silence(AudioGrain_Silence sourceGrain)
		{
			this.sourceGrain = sourceGrain;
			base.duration = sourceGrain.durationRange.RandomInRange;
		}

		public override string ToString()
		{
			return "Silence";
		}

		public override bool Equals(object obj)
		{
			bool result;
			if (obj == null)
			{
				result = false;
			}
			else
			{
				ResolvedGrain_Silence resolvedGrain_Silence = obj as ResolvedGrain_Silence;
				result = (resolvedGrain_Silence != null && resolvedGrain_Silence.sourceGrain == this.sourceGrain);
			}
			return result;
		}

		public override int GetHashCode()
		{
			return this.sourceGrain.GetHashCode();
		}
	}
}
