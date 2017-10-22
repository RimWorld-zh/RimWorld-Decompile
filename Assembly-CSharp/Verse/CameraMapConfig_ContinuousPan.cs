namespace Verse
{
	public class CameraMapConfig_ContinuousPan : CameraMapConfig
	{
		public CameraMapConfig_ContinuousPan()
		{
			base.dollyRateKeys = 10f;
			base.dollyRateMouseDrag = 4f;
			base.dollyRateScreenEdge = 5f;
			base.camSpeedDecayFactor = 1f;
			base.moveSpeedScale = 1f;
		}
	}
}
