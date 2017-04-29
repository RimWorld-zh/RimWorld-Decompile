using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace RimWorld.Planet
{
	public class WorldLayer_Sun : WorldLayer
	{
		private const float SunDrawSize = 15f;

		protected override int Layer
		{
			get
			{
				return WorldCameraManager.WorldSkyboxLayer;
			}
		}

		protected override Quaternion Rotation
		{
			get
			{
				return Quaternion.LookRotation(GenCelestial.CurSunPositionInWorldSpace());
			}
		}

		[DebuggerHidden]
		public override IEnumerable Regenerate()
		{
			WorldLayer_Sun.<Regenerate>c__IteratorF5 <Regenerate>c__IteratorF = new WorldLayer_Sun.<Regenerate>c__IteratorF5();
			<Regenerate>c__IteratorF.<>f__this = this;
			WorldLayer_Sun.<Regenerate>c__IteratorF5 expr_0E = <Regenerate>c__IteratorF;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
