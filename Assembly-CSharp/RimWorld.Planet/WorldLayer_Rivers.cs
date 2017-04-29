using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Verse.Noise;

namespace RimWorld.Planet
{
	public class WorldLayer_Rivers : WorldLayer_Paths
	{
		private const float PerlinFrequency = 0.5f;

		private const float PerlinMagnitude = 0.15f;

		private Color32 riverColor = new Color32(73, 82, 100, 255);

		private ModuleBase riverDisplacementX = new Perlin(0.5, 2.0, 0.5, 3, 84905524, QualityMode.Medium);

		private ModuleBase riverDisplacementY = new Perlin(0.5, 2.0, 0.5, 3, 37971116, QualityMode.Medium);

		private ModuleBase riverDisplacementZ = new Perlin(0.5, 2.0, 0.5, 3, 91572032, QualityMode.Medium);

		public WorldLayer_Rivers()
		{
			this.pointyEnds = true;
		}

		[DebuggerHidden]
		public override IEnumerable Regenerate()
		{
			WorldLayer_Rivers.<Regenerate>c__IteratorF2 <Regenerate>c__IteratorF = new WorldLayer_Rivers.<Regenerate>c__IteratorF2();
			<Regenerate>c__IteratorF.<>f__this = this;
			WorldLayer_Rivers.<Regenerate>c__IteratorF2 expr_0E = <Regenerate>c__IteratorF;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		public override Vector3 FinalizePoint(Vector3 inp, float distortionFrequency, float distortionIntensity)
		{
			float magnitude = inp.magnitude;
			inp = (inp + new Vector3(this.riverDisplacementX.GetValue(inp), this.riverDisplacementY.GetValue(inp), this.riverDisplacementZ.GetValue(inp)) * 0.15f).normalized * magnitude;
			return inp + inp.normalized * 0.008f;
		}
	}
}
