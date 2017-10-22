namespace Verse
{
	public class MoteCounter
	{
		private int moteCount;

		private const int SaturatedCount = 250;

		public int MoteCount
		{
			get
			{
				return this.moteCount;
			}
		}

		public float Saturation
		{
			get
			{
				return (float)((float)this.moteCount / 250.0);
			}
		}

		public bool Saturated
		{
			get
			{
				return this.Saturation > 1.0;
			}
		}

		public bool SaturatedLowPriority
		{
			get
			{
				return this.Saturation > 0.800000011920929;
			}
		}

		public void Notify_MoteSpawned()
		{
			this.moteCount++;
		}

		public void Notify_MoteDespawned()
		{
			this.moteCount--;
		}
	}
}
