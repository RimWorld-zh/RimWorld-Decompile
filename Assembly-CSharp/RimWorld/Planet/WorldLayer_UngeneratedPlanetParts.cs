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
	public class WorldLayer_UngeneratedPlanetParts : WorldLayer
	{
		private const int SubdivisionsCount = 4;

		private const float ViewAngleOffset = 10f;

		public WorldLayer_UngeneratedPlanetParts()
		{
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
			Vector3 planetViewCenter = Find.WorldGrid.viewCenter;
			float planetViewAngle = Find.WorldGrid.viewAngle;
			if (planetViewAngle < 180f)
			{
				List<Vector3> collection;
				List<int> collection2;
				SphereGenerator.Generate(4, 99.85f, -planetViewCenter, 180f - Mathf.Min(planetViewAngle, 180f) + 10f, out collection, out collection2);
				LayerSubMesh subMesh = base.GetSubMesh(WorldMaterials.UngeneratedPlanetParts);
				subMesh.verts.AddRange(collection);
				subMesh.tris.AddRange(collection2);
			}
			base.FinalizeMesh(MeshParts.All);
			yield break;
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

			internal Vector3 <planetViewCenter>__0;

			internal float <planetViewAngle>__0;

			internal WorldLayer_UngeneratedPlanetParts $this;

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
				planetViewCenter = Find.WorldGrid.viewCenter;
				planetViewAngle = Find.WorldGrid.viewAngle;
				if (planetViewAngle < 180f)
				{
					List<Vector3> collection;
					List<int> collection2;
					SphereGenerator.Generate(4, 99.85f, -planetViewCenter, 180f - Mathf.Min(planetViewAngle, 180f) + 10f, out collection, out collection2);
					LayerSubMesh subMesh = base.GetSubMesh(WorldMaterials.UngeneratedPlanetParts);
					subMesh.verts.AddRange(collection);
					subMesh.tris.AddRange(collection2);
				}
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
				WorldLayer_UngeneratedPlanetParts.<Regenerate>c__Iterator0 <Regenerate>c__Iterator = new WorldLayer_UngeneratedPlanetParts.<Regenerate>c__Iterator0();
				<Regenerate>c__Iterator.$this = this;
				return <Regenerate>c__Iterator;
			}
		}
	}
}
