using UnityEngine;

namespace Verse.Sound
{
	public class SoundParamSource_CameraAltitude : SoundParamSource
	{
		public override string Label
		{
			get
			{
				return "Camera altitude";
			}
		}

		public override float ValueFor(Sample samp)
		{
			Vector3 position = Find.Camera.transform.position;
			return position.y;
		}
	}
}
