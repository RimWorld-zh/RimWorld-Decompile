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
	[StaticConstructorOnStartup]
	public class FormCaravanComp : WorldObjectComp
	{
		public static readonly Texture2D FormCaravanCommand = ContentFinder<Texture2D>.Get("UI/Commands/FormCaravan", true);

		public FormCaravanComp()
		{
		}

		public WorldObjectCompProperties_FormCaravan Props
		{
			get
			{
				return (WorldObjectCompProperties_FormCaravan)this.props;
			}
		}

		private MapParent MapParent
		{
			get
			{
				return (MapParent)this.parent;
			}
		}

		public bool Reform
		{
			get
			{
				return !this.MapParent.HasMap || !this.MapParent.Map.IsPlayerHome;
			}
		}

		public bool CanFormOrReformCaravanNow
		{
			get
			{
				MapParent mapParent = this.MapParent;
				return mapParent.HasMap && (!this.Reform || (!GenHostility.AnyHostileActiveThreatToPlayer(mapParent.Map) && mapParent.Map.mapPawns.FreeColonistsSpawnedCount != 0));
			}
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			MapParent mapParent = (MapParent)this.parent;
			if (mapParent.HasMap)
			{
				if (!this.Reform)
				{
					yield return new Command_Action
					{
						defaultLabel = "CommandFormCaravan".Translate(),
						defaultDesc = "CommandFormCaravanDesc".Translate(),
						icon = FormCaravanComp.FormCaravanCommand,
						hotKey = KeyBindingDefOf.Misc2,
						tutorTag = "FormCaravan",
						action = delegate()
						{
							Find.WindowStack.Add(new Dialog_FormCaravan(mapParent.Map, false, null, false));
						}
					};
				}
				else if (mapParent.Map.mapPawns.FreeColonistsSpawnedCount != 0)
				{
					Command_Action reformCaravan = new Command_Action();
					reformCaravan.defaultLabel = "CommandReformCaravan".Translate();
					reformCaravan.defaultDesc = "CommandReformCaravanDesc".Translate();
					reformCaravan.icon = FormCaravanComp.FormCaravanCommand;
					reformCaravan.hotKey = KeyBindingDefOf.Misc2;
					reformCaravan.tutorTag = "ReformCaravan";
					reformCaravan.action = delegate()
					{
						Find.WindowStack.Add(new Dialog_FormCaravan(mapParent.Map, true, null, false));
					};
					if (GenHostility.AnyHostileActiveThreatToPlayer(mapParent.Map))
					{
						reformCaravan.Disable("CommandReformCaravanFailHostilePawns".Translate());
					}
					yield return reformCaravan;
				}
			}
			yield break;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static FormCaravanComp()
		{
		}

		[CompilerGenerated]
		private sealed class <GetGizmos>c__Iterator0 : IEnumerable, IEnumerable<Gizmo>, IEnumerator, IDisposable, IEnumerator<Gizmo>
		{
			internal Command_Action <formCaravan>__1;

			internal Command_Action <reformCaravan>__2;

			internal FormCaravanComp $this;

			internal Gizmo $current;

			internal bool $disposing;

			internal int $PC;

			private FormCaravanComp.<GetGizmos>c__Iterator0.<GetGizmos>c__AnonStorey1 $locvar0;

			[DebuggerHidden]
			public <GetGizmos>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
				{
					MapParent mapParent = (MapParent)this.parent;
					if (mapParent.HasMap)
					{
						if (!base.Reform)
						{
							Command_Action formCaravan = new Command_Action();
							formCaravan.defaultLabel = "CommandFormCaravan".Translate();
							formCaravan.defaultDesc = "CommandFormCaravanDesc".Translate();
							formCaravan.icon = FormCaravanComp.FormCaravanCommand;
							formCaravan.hotKey = KeyBindingDefOf.Misc2;
							formCaravan.tutorTag = "FormCaravan";
							formCaravan.action = delegate()
							{
								Find.WindowStack.Add(new Dialog_FormCaravan(mapParent.Map, false, null, false));
							};
							this.$current = formCaravan;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
						}
						else
						{
							if (mapParent.Map.mapPawns.FreeColonistsSpawnedCount == 0)
							{
								break;
							}
							reformCaravan = new Command_Action();
							reformCaravan.defaultLabel = "CommandReformCaravan".Translate();
							reformCaravan.defaultDesc = "CommandReformCaravanDesc".Translate();
							reformCaravan.icon = FormCaravanComp.FormCaravanCommand;
							reformCaravan.hotKey = KeyBindingDefOf.Misc2;
							reformCaravan.tutorTag = "ReformCaravan";
							reformCaravan.action = delegate()
							{
								Find.WindowStack.Add(new Dialog_FormCaravan(mapParent.Map, true, null, false));
							};
							if (GenHostility.AnyHostileActiveThreatToPlayer(mapParent.Map))
							{
								reformCaravan.Disable("CommandReformCaravanFailHostilePawns".Translate());
							}
							this.$current = reformCaravan;
							if (!this.$disposing)
							{
								this.$PC = 2;
							}
						}
						return true;
					}
					break;
				}
				case 1u:
					break;
				case 2u:
					break;
				default:
					return false;
				}
				this.$PC = -1;
				return false;
			}

			Gizmo IEnumerator<Gizmo>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.Gizmo>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				FormCaravanComp.<GetGizmos>c__Iterator0 <GetGizmos>c__Iterator = new FormCaravanComp.<GetGizmos>c__Iterator0();
				<GetGizmos>c__Iterator.$this = this;
				return <GetGizmos>c__Iterator;
			}

			private sealed class <GetGizmos>c__AnonStorey1
			{
				internal MapParent mapParent;

				internal FormCaravanComp.<GetGizmos>c__Iterator0 <>f__ref$0;

				public <GetGizmos>c__AnonStorey1()
				{
				}

				internal void <>m__0()
				{
					Find.WindowStack.Add(new Dialog_FormCaravan(this.mapParent.Map, false, null, false));
				}

				internal void <>m__1()
				{
					Find.WindowStack.Add(new Dialog_FormCaravan(this.mapParent.Map, true, null, false));
				}
			}
		}
	}
}
