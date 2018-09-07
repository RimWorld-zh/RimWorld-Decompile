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
				if (base.Faction == Faction.OfPlayer)
				{
					return MapGeneratorDefOf.Base_Player;
				}
				return MapGeneratorDefOf.Base_Faction;
			}
		}

		public override IEnumerable<IncidentTargetTagDef> IncidentTargetTags()
		{
			foreach (IncidentTargetTagDef type in base.IncidentTargetTags())
			{
				yield return type;
			}
			if (base.Faction == Faction.OfPlayer)
			{
				yield return IncidentTargetTagDefOf.Map_PlayerHome;
			}
			else
			{
				yield return IncidentTargetTagDefOf.Map_Misc;
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
		private IEnumerable<IncidentTargetTagDef> <IncidentTargetTags>__BaseCallProxy0()
		{
			return base.IncidentTargetTags();
		}

		[CompilerGenerated]
		private sealed class <IncidentTargetTags>c__Iterator0 : IEnumerable, IEnumerable<IncidentTargetTagDef>, IEnumerator, IDisposable, IEnumerator<IncidentTargetTagDef>
		{
			internal IEnumerator<IncidentTargetTagDef> $locvar0;

			internal IncidentTargetTagDef <type>__1;

			internal Settlement $this;

			internal IncidentTargetTagDef $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <IncidentTargetTags>c__Iterator0()
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
					enumerator = base.<IncidentTargetTags>__BaseCallProxy0().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_110;
				case 3u:
					goto IL_110;
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
					this.$current = IncidentTargetTagDefOf.Map_PlayerHome;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
				}
				else
				{
					this.$current = IncidentTargetTagDefOf.Map_Misc;
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
				}
				return true;
				IL_110:
				this.$PC = -1;
				return false;
			}

			IncidentTargetTagDef IEnumerator<IncidentTargetTagDef>.Current
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
				return this.System.Collections.Generic.IEnumerable<RimWorld.IncidentTargetTagDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<IncidentTargetTagDef> IEnumerable<IncidentTargetTagDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Settlement.<IncidentTargetTags>c__Iterator0 <IncidentTargetTags>c__Iterator = new Settlement.<IncidentTargetTags>c__Iterator0();
				<IncidentTargetTags>c__Iterator.$this = this;
				return <IncidentTargetTags>c__Iterator;
			}
		}
	}
}
