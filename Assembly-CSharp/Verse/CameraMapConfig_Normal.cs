namespace Verse
{
	public class CameraMapConfig_Normal : CameraMapConfig
	{
		public CameraMapConfig_Normal()
		{
			base.dollyRateKeys = 50f;
			base.dollyRateMouseDrag = 6.5f;
			base.dollyRateScreenEdge = 35f;
			base.camSpeedDecayFactor = 0.85f;
			base.moveSpeedScale = 2f;
		}
	}
}
