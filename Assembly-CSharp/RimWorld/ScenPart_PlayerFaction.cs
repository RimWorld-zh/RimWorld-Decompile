using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class ScenPart_PlayerFaction : ScenPart
	{
		internal FactionDef factionDef;

		[CompilerGenerated]
		private static Func<FactionDef, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<FactionDef, bool> <>f__am$cache1;

		public ScenPart_PlayerFaction()
		{
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<FactionDef>(ref this.factionDef, "factionDef");
		}

		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight);
			if (Widgets.ButtonText(scenPartRect, this.factionDef.LabelCap, true, false, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (FactionDef localFd2 in from d in DefDatabase<FactionDef>.AllDefs
				where d.isPlayer
				select d)
				{
					FactionDef localFd = localFd2;
					list.Add(new FloatMenuOption(localFd.LabelCap, delegate()
					{
						this.factionDef = localFd;
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		public override string Summary(Scenario scen)
		{
			return "ScenPart_PlayerFaction".Translate(new object[]
			{
				this.factionDef.label
			});
		}

		public override void Randomize()
		{
			this.factionDef = (from fd in DefDatabase<FactionDef>.AllDefs
			where fd.isPlayer
			select fd).RandomElement<FactionDef>();
		}

		public override void PostWorldGenerate()
		{
			Find.GameInitData.playerFaction = FactionGenerator.NewGeneratedFaction(this.factionDef);
			Find.FactionManager.Add(Find.GameInitData.playerFaction);
			FactionGenerator.EnsureRequiredEnemies(Find.GameInitData.playerFaction);
		}

		public override void PreMapGenerate()
		{
			FactionBase factionBase = (FactionBase)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.FactionBase);
			factionBase.SetFaction(Find.GameInitData.playerFaction);
			factionBase.Tile = Find.GameInitData.startingTile;
			factionBase.Name = FactionBaseNameGenerator.GenerateFactionBaseName(factionBase, Find.GameInitData.playerFaction.def.playerInitialSettlementNameMaker);
			Find.WorldObjects.Add(factionBase);
		}

		public override void PostGameStart()
		{
			Find.GameInitData.playerFaction = null;
		}

		public override IEnumerable<string> ConfigErrors()
		{
			if (this.factionDef == null)
			{
				yield return "factionDef is null";
			}
			yield break;
		}

		[CompilerGenerated]
		private static bool <DoEditInterface>m__0(FactionDef d)
		{
			return d.isPlayer;
		}

		[CompilerGenerated]
		private static bool <Randomize>m__1(FactionDef fd)
		{
			return fd.isPlayer;
		}

		[CompilerGenerated]
		private sealed class <DoEditInterface>c__AnonStorey1
		{
			internal FactionDef localFd;

			internal ScenPart_PlayerFaction $this;

			public <DoEditInterface>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				this.$this.factionDef = this.localFd;
			}
		}

		[CompilerGenerated]
		private sealed class <ConfigErrors>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal ScenPart_PlayerFaction $this;

			internal string $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <ConfigErrors>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (this.factionDef == null)
					{
						this.$current = "factionDef is null";
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					break;
				case 1u:
					break;
				default:
					return false;
				}
				this.$PC = -1;
				return false;
			}

			string IEnumerator<string>.Current
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
				return this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<string> IEnumerable<string>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				ScenPart_PlayerFaction.<ConfigErrors>c__Iterator0 <ConfigErrors>c__Iterator = new ScenPart_PlayerFaction.<ConfigErrors>c__Iterator0();
				<ConfigErrors>c__Iterator.$this = this;
				return <ConfigErrors>c__Iterator;
			}
		}
	}
}
