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
	public class Settlement : SettlementBase
	{
		private string nameInt;

		public bool namedByPlayer;

		private Material cachedMat;

		public Settlement()
		{
			this.trader = new Settlement_TraderTracker(this);
		}

		public string Name
		{
			get
			{
				return this.nameInt;
			}
			set
			{
				this.nameInt = value;
			}
		}

		public override Texture2D ExpandingIcon
		{
			get
			{
				return base.Faction.def.ExpandingIconTexture;
			}
		}

		public override string Label
		{
			get
			{
				return (this.nameInt == null) ? base.Label : this.nameInt;
			}
		}

		public override Material Material
		{
			get
			{
				if (this.cachedMat == null)
				{
					this.cachedMat = MaterialPool.MatFrom(base.Faction.def.homeIconPath, ShaderDatabase.WorldOverlayTransparentLit, base.Faction.Color, WorldMaterials.WorldObjectRenderQueue);
				}
				return this.cachedMat;
			}
		}

		public override MapGeneratorDef MapGeneratorDef
		{
			get
			{
				MapGeneratorDef result;
				if (base.Faction == Faction.OfPlayer)
				{
					result = MapGeneratorDefOf.Base_Player;
				}
				else
				{
					result = MapGeneratorDefOf.Base_Faction;
				}
				return result;
			}
		}

		public override IEnumerable<IncidentTargetTypeDef> AcceptedTypes()
		{
			foreach (IncidentTargetTypeDef type in this.<AcceptedTypes>__BaseCallProxy0())
			{
				yield return type;
			}
			if (base.Faction == Faction.OfPlayer)
			{
				yield return IncidentTargetTypeDefOf.Map_PlayerHome;
			}
			else
			{
				yield return IncidentTargetTypeDefOf.Map_Misc;
			}
			yield break;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.nameInt, "nameInt", null, false);
			Scribe_Values.Look<bool>(ref this.namedByPlayer, "namedByPlayer", false, false);
		}

		public override void Tick()
		{
			base.Tick();
			SettlementDefeatUtility.CheckDefeated(this);
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<IncidentTargetTypeDef> <AcceptedTypes>__BaseCallProxy0()
		{
			return base.AcceptedTypes();
		}

		[CompilerGenerated]
		private sealed class <AcceptedTypes>c__Iterator0 : IEnumerable, IEnumerable<IncidentTargetTypeDef>, IEnumerator, IDisposable, IEnumerator<IncidentTargetTypeDef>
		{
			internal IEnumerator<IncidentTargetTypeDef> $locvar0;

			internal IncidentTargetTypeDef <type>__1;

			internal Settlement $this;

			internal IncidentTargetTypeDef $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <AcceptedTypes>c__Iterator0()
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
					enumerator = base.<AcceptedTypes>__BaseCallProxy0().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_118;
				case 3u:
					goto IL_118;
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
						type = enumerator.Current;
						this.$current = type;
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
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				if (base.Faction == Faction.OfPlayer)
				{
					this.$current = IncidentTargetTypeDefOf.Map_PlayerHome;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
				}
				else
				{
					this.$current = IncidentTargetTypeDefOf.Map_Misc;
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
				}
				return true;
				IL_118:
				this.$PC = -1;
				return false;
			}

			IncidentTargetTypeDef IEnumerator<IncidentTargetTypeDef>.Current
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
						if (enumerator != null)
						{
							enumerator.Dispose();
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
				return this.System.Collections.Generic.IEnumerable<RimWorld.IncidentTargetTypeDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<IncidentTargetTypeDef> IEnumerable<IncidentTargetTypeDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Settlement.<AcceptedTypes>c__Iterator0 <AcceptedTypes>c__Iterator = new Settlement.<AcceptedTypes>c__Iterator0();
				<AcceptedTypes>c__Iterator.$this = this;
				return <AcceptedTypes>c__Iterator;
			}
		}
	}
}
