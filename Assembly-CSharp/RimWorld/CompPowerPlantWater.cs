using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class CompPowerPlantWater : CompPowerPlant
	{
		private float spinPosition = 0f;

		private float spinRate = 1f;

		[TweakValue("Graphics", 0f, 1f)]
		private static float SpinRateFactor = 0.005f;

		[TweakValue("Graphics", 1f, 3f)]
		private static float BladeOffset = 2.36f;

		[TweakValue("Graphics", 0f, 20f)]
		private static int BladeCount = 9;

		public static readonly Material BladesMat = MaterialPool.MatFrom("Things/Building/Power/WatermillGenerator/WatermillGeneratorBlades");

		public CompPowerPlantWater()
		{
		}

		protected override float DesiredPowerOutput
		{
			get
			{
				foreach (IntVec3 c in this.WaterCells())
				{
					if (!this.parent.Map.terrainGrid.TerrainAt(c).affordances.Contains(TerrainAffordanceDefOf.MovingFluid))
					{
						return 0f;
					}
				}
				return base.DesiredPowerOutput;
			}
		}

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.spinPosition = Rand.Range(0f, 15f);
			Vector3 vector = Vector3.zero;
			foreach (IntVec3 intVec in this.WaterCells())
			{
				vector += this.parent.Map.waterInfo.GetWaterMovement(intVec.ToVector3Shifted());
			}
			this.spinRate = Mathf.Sign(Vector3.Dot(vector, this.parent.Rotation.Rotated(RotationDirection.Clockwise).FacingCell.ToVector3()));
			this.spinRate *= Rand.RangeSeeded(0.9f, 1.1f, this.parent.thingIDNumber * 60509 + 33151);
		}

		public override void CompTick()
		{
			base.CompTick();
			if (base.PowerOutput > 0.01f)
			{
				this.spinPosition = (this.spinPosition + CompPowerPlantWater.SpinRateFactor * this.spinRate + 6.28318548f) % 6.28318548f;
			}
		}

		public IEnumerable<IntVec3> WaterCells()
		{
			return CompPowerPlantWater.WaterCells(this.parent.Position, this.parent.Rotation);
		}

		public static IEnumerable<IntVec3> WaterCells(IntVec3 loc, Rot4 rot)
		{
			IntVec3 perpOffset = rot.Rotated(RotationDirection.Counterclockwise).FacingCell;
			yield return loc + rot.FacingCell * 3;
			yield return loc + rot.FacingCell * 3 - perpOffset;
			yield return loc + rot.FacingCell * 3 - perpOffset * 2;
			yield return loc + rot.FacingCell * 3 + perpOffset;
			yield return loc + rot.FacingCell * 3 + perpOffset * 2;
			yield break;
		}

		public IEnumerable<IntVec3> GroundCells()
		{
			return CompPowerPlantWater.GroundCells(this.parent.Position, this.parent.Rotation);
		}

		public static IEnumerable<IntVec3> GroundCells(IntVec3 loc, Rot4 rot)
		{
			IntVec3 perpOffset = rot.Rotated(RotationDirection.Counterclockwise).FacingCell;
			yield return loc - rot.FacingCell;
			yield return loc - rot.FacingCell - perpOffset;
			yield return loc - rot.FacingCell + perpOffset;
			yield return loc;
			yield return loc - perpOffset;
			yield return loc + perpOffset;
			yield return loc + rot.FacingCell;
			yield return loc + rot.FacingCell - perpOffset;
			yield return loc + rot.FacingCell + perpOffset;
			yield break;
		}

		public override void PostDraw()
		{
			base.PostDraw();
			Vector3 a = this.parent.TrueCenter();
			a += this.parent.Rotation.FacingCell.ToVector3() * CompPowerPlantWater.BladeOffset;
			for (int i = 0; i < CompPowerPlantWater.BladeCount; i++)
			{
				float num = this.spinPosition + 6.28318548f * (float)i / (float)CompPowerPlantWater.BladeCount;
				float x = Mathf.Abs(4f * Mathf.Sin(num));
				bool flag = num % 6.28318548f < 3.14159274f;
				Vector2 vector = new Vector2(x, 1f);
				Vector3 s = new Vector3(vector.x, 1f, vector.y);
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(a + Vector3.up * 0.046875f * Mathf.Cos(num), this.parent.Rotation.AsQuat, s);
				Graphics.DrawMesh((!flag) ? MeshPool.plane10Flip : MeshPool.plane10, matrix, CompPowerPlantWater.BladesMat, 0);
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static CompPowerPlantWater()
		{
		}

		[CompilerGenerated]
		private sealed class <WaterCells>c__Iterator0 : IEnumerable, IEnumerable<IntVec3>, IEnumerator, IDisposable, IEnumerator<IntVec3>
		{
			internal Rot4 rot;

			internal IntVec3 <perpOffset>__0;

			internal IntVec3 loc;

			internal IntVec3 $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <WaterCells>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					perpOffset = rot.Rotated(RotationDirection.Counterclockwise).FacingCell;
					this.$current = loc + rot.FacingCell * 3;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$current = loc + rot.FacingCell * 3 - perpOffset;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$current = loc + rot.FacingCell * 3 - perpOffset * 2;
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					this.$current = loc + rot.FacingCell * 3 + perpOffset;
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					this.$current = loc + rot.FacingCell * 3 + perpOffset * 2;
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				case 5u:
					this.$PC = -1;
					break;
				}
				return false;
			}

			IntVec3 IEnumerator<IntVec3>.Current
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
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.IntVec3>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				CompPowerPlantWater.<WaterCells>c__Iterator0 <WaterCells>c__Iterator = new CompPowerPlantWater.<WaterCells>c__Iterator0();
				<WaterCells>c__Iterator.rot = rot;
				<WaterCells>c__Iterator.loc = loc;
				return <WaterCells>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GroundCells>c__Iterator1 : IEnumerable, IEnumerable<IntVec3>, IEnumerator, IDisposable, IEnumerator<IntVec3>
		{
			internal Rot4 rot;

			internal IntVec3 <perpOffset>__0;

			internal IntVec3 loc;

			internal IntVec3 $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GroundCells>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					perpOffset = rot.Rotated(RotationDirection.Counterclockwise).FacingCell;
					this.$current = loc - rot.FacingCell;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$current = loc - rot.FacingCell - perpOffset;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$current = loc - rot.FacingCell + perpOffset;
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					this.$current = loc;
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					this.$current = loc - perpOffset;
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				case 5u:
					this.$current = loc + perpOffset;
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				case 6u:
					this.$current = loc + rot.FacingCell;
					if (!this.$disposing)
					{
						this.$PC = 7;
					}
					return true;
				case 7u:
					this.$current = loc + rot.FacingCell - perpOffset;
					if (!this.$disposing)
					{
						this.$PC = 8;
					}
					return true;
				case 8u:
					this.$current = loc + rot.FacingCell + perpOffset;
					if (!this.$disposing)
					{
						this.$PC = 9;
					}
					return true;
				case 9u:
					this.$PC = -1;
					break;
				}
				return false;
			}

			IntVec3 IEnumerator<IntVec3>.Current
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
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.IntVec3>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				CompPowerPlantWater.<GroundCells>c__Iterator1 <GroundCells>c__Iterator = new CompPowerPlantWater.<GroundCells>c__Iterator1();
				<GroundCells>c__Iterator.rot = rot;
				<GroundCells>c__Iterator.loc = loc;
				return <GroundCells>c__Iterator;
			}
		}
	}
}
