using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public class WorldLayer_Stars : WorldLayer
	{
		private bool calculatedForStaticRotation = false;

		private int calculatedForStartingTile = -1;

		public const float DistanceToStars = 10f;

		private static readonly FloatRange StarsDrawSize = new FloatRange(1f, 3.8f);

		private const int StarsCount = 1500;

		private const float DistToSunToReduceStarSize = 0.8f;

		public WorldLayer_Stars()
		{
		}

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
				Quaternion result;
				if (this.UseStaticRotation)
				{
					result = Quaternion.identity;
				}
				else
				{
					result = Quaternion.LookRotation(GenCelestial.CurSunPositionInWorldSpace());
				}
				return result;
			}
		}

		public override IEnumerable Regenerate()
		{
			IEnumerator enumerator = this.<Regenerate>__BaseCallProxy0().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object result = enumerator.Current;
					yield return result;
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			Rand.PushState();
			Rand.Seed = Find.World.info.Seed;
			for (int i = 0; i < 1500; i++)
			{
				Vector3 unitVector = Rand.UnitVector3;
				Vector3 pos = unitVector * 10f;
				LayerSubMesh subMesh = base.GetSubMesh(WorldMaterials.Stars);
				float num = WorldLayer_Stars.StarsDrawSize.RandomInRange;
				Vector3 rhs = (!this.UseStaticRotation) ? Vector3.forward : GenCelestial.CurSunPositionInWorldSpace().normalized;
				float num2 = Vector3.Dot(unitVector, rhs);
				if (num2 > 0.8f)
				{
					num *= GenMath.LerpDouble(0.8f, 1f, 1f, 0.35f, num2);
				}
				WorldRendererUtility.PrintQuadTangentialToPlanet(pos, num, 0f, subMesh, true, true, true);
			}
			this.calculatedForStartingTile = ((Find.GameInitData == null) ? -1 : Find.GameInitData.startingTile);
			this.calculatedForStaticRotation = this.UseStaticRotation;
			Rand.PopState();
			base.FinalizeMesh(MeshParts.All);
			yield break;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static WorldLayer_Stars()
		{
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable <Regenerate>__BaseCallProxy0()
		{
			return base.Regenerate();
		}

		[CompilerGenerated]
		private sealed class <Regenerate>c__Iterator0 : IEnumerable, IEnumerable<object>, IEnumerator, IDisposable, IEnumerator<object>
		{
			internal IEnumerator $locvar0;

			internal object <result>__1;

			internal IDisposable $locvar1;

			internal WorldLayer_Stars $this;

			internal object $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <Regenerate>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = base.<Regenerate>__BaseCallProxy0().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						result = enumerator.Current;
						this.$current = result;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if ((disposable = (enumerator as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
				}
				Rand.PushState();
				Rand.Seed = Find.World.info.Seed;
				for (int i = 0; i < 1500; i++)
				{
					Vector3 unitVector = Rand.UnitVector3;
					Vector3 pos = unitVector * 10f;
					LayerSubMesh subMesh = base.GetSubMesh(WorldMaterials.Stars);
					float num2 = WorldLayer_Stars.StarsDrawSize.RandomInRange;
					Vector3 rhs = (!base.UseStaticRotation) ? Vector3.forward : GenCelestial.CurSunPositionInWorldSpace().normalized;
					float num3 = Vector3.Dot(unitVector, rhs);
					if (num3 > 0.8f)
					{
						num2 *= GenMath.LerpDouble(0.8f, 1f, 1f, 0.35f, num3);
					}
					WorldRendererUtility.PrintQuadTangentialToPlanet(pos, num2, 0f, subMesh, true, true, true);
				}
				this.calculatedForStartingTile = ((Find.GameInitData == null) ? -1 : Find.GameInitData.startingTile);
				this.calculatedForStaticRotation = base.UseStaticRotation;
				Rand.PopState();
				base.FinalizeMesh(MeshParts.All);
				this.$PC = -1;
				return false;
			}

			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if ((disposable = (enumerator as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<object>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<object> IEnumerable<object>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				WorldLayer_Stars.<Regenerate>c__Iterator0 <Regenerate>c__Iterator = new WorldLayer_Stars.<Regenerate>c__Iterator0();
				<Regenerate>c__Iterator.$this = this;
				return <Regenerate>c__Iterator;
			}
		}
	}
}
