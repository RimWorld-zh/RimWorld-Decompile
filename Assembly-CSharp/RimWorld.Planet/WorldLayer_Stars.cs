using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public class WorldLayer_Stars : WorldLayer
	{
		public const float DistanceToStars = 10f;

		private const int StarsCount = 1500;

		private const float DistToSunToReduceStarSize = 0.8f;

		private bool calculatedForStaticRotation;

		private int calculatedForStartingTile = -1;

		private static readonly FloatRange StarsDrawSize = new FloatRange(1f, 3.8f);

		protected override int Layer
		{
			get
			{
				return WorldCameraManager.WorldSkyboxLayer;
			}
		}

		public override bool ShouldRegenerate
		{
			get
			{
				return base.ShouldRegenerate || (Find.GameInitData != null && Find.GameInitData.startingTile != this.calculatedForStartingTile) || this.UseStaticRotation != this.calculatedForStaticRotation;
			}
		}

		private bool UseStaticRotation
		{
			get
			{
				return Current.ProgramState == ProgramState.Entry;
			}
		}

		protected override Quaternion Rotation
		{
			get
			{
				if (this.UseStaticRotation)
				{
					return Quaternion.identity;
				}
				return Quaternion.LookRotation(GenCelestial.CurSunPositionInWorldSpace());
			}
		}

		[DebuggerHidden]
		public override IEnumerable Regenerate()
		{
			WorldLayer_Stars.<Regenerate>c__IteratorF4 <Regenerate>c__IteratorF = new WorldLayer_Stars.<Regenerate>c__IteratorF4();
			<Regenerate>c__IteratorF.<>f__this = this;
			WorldLayer_Stars.<Regenerate>c__IteratorF4 expr_0E = <Regenerate>c__IteratorF;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
