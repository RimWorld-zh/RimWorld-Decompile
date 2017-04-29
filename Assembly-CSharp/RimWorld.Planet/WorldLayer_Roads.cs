using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Verse.Noise;

namespace RimWorld.Planet
{
	public class WorldLayer_Roads : WorldLayer_Paths
	{
		private ModuleBase roadDisplacementX = new Perlin(1.0, 2.0, 0.5, 3, 74173887, QualityMode.Medium);

		private ModuleBase roadDisplacementY = new Perlin(1.0, 2.0, 0.5, 3, 67515931, QualityMode.Medium);

		private ModuleBase roadDisplacementZ = new Perlin(1.0, 2.0, 0.5, 3, 87116801, QualityMode.Medium);

		[DebuggerHidden]
		public override IEnumerable Regenerate()
		{
			WorldLayer_Roads.<Regenerate>c__IteratorF3 <Regenerate>c__IteratorF = new WorldLayer_Roads.<Regenerate>c__IteratorF3();
			<Regenerate>c__IteratorF.<>f__this = this;
			WorldLayer_Roads.<Regenerate>c__IteratorF3 expr_0E = <Regenerate>c__IteratorF;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		public override Vector3 FinalizePoint(Vector3 inp, float distortionFrequency, float distortionIntensity)
		{
			Vector3 coordinate = inp * distortionFrequency;
			float magnitude = inp.magnitude;
			inp = (inp + new Vector3(this.roadDisplacementX.GetValue(coordinate), this.roadDisplacementY.GetValue(coordinate), this.roadDisplacementZ.GetValue(coordinate)) * distortionIntensity).normalized * magnitude;
			return inp + inp.normalized * 0.012f;
		}
	}
}
