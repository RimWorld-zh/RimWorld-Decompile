namespace Verse.Sound
{
	public class SustainerScopeFader
	{
		public bool inScope = true;

		public float inScopePercent = 1f;

		private const float ScopeMatchFallRate = 0.03f;

		private const float ScopeMatchRiseRate = 0.05f;

		public void SustainerScopeUpdate()
		{
			if (this.inScope)
			{
				float num = this.inScopePercent += 0.05f;
				if (this.inScopePercent > 1.0)
				{
					this.inScopePercent = 1f;
				}
			}
			else
			{
				this.inScopePercent -= 0.03f;
				if (this.inScopePercent <= 0.0010000000474974513)
				{
					this.inScopePercent = 0f;
				}
			}
		}
	}
}
