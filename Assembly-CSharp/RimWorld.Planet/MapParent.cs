using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	[StaticConstructorOnStartup]
	public class MapParent : WorldObject, IThingHolder
	{
		public const int DefaultForceExitAndRemoveMapCountdownHours = 24;

		private int ticksLeftToForceExitAndRemoveMap = -1;

		private bool anyCaravanEverFormed;

		private static readonly Texture2D ShowMapCommand = ContentFinder<Texture2D>.Get("UI/Commands/ShowMap", true);

		public static readonly Texture2D FormCaravanCommand = ContentFinder<Texture2D>.Get("UI/Commands/FormCaravan", true);

		private static List<Pawn> tmpPawns = new List<Pawn>();

		public bool HasMap
		{
			get
			{
				return this.Map != null;
			}
		}

		protected virtual bool UseGenericEnterMapFloatMenuOption
		{
			get
			{
				return true;
			}
		}

		public Map Map
		{
			get
			{
				return Current.Game.FindMap(this);
			}
		}

		public virtual MapGeneratorDef MapGeneratorDef
		{
			get
			{
				return null;
			}
		}

		public virtual IEnumerable<GenStepDef> ExtraGenStepDefs
		{
			get
			{
				yield break;
			}
		}

		public virtual bool TransportPodsCanLandAndGenerateMap
		{
			get
			{
				return false;
			}
		}

		public bool ForceExitAndRemoveMapCountdownActive
		{
			get
			{
				return this.ticksLeftToForceExitAndRemoveMap >= 0;
			}
		}

		public string ForceExitAndRemoveMapCountdownTimeLeftString
		{
			get
			{
				if (!this.ForceExitAndRemoveMapCountdownActive)
				{
					return string.Empty;
				}
				return MapParent.GetForceExitAndRemoveMapCountdownTimeLeftString(this.ticksLeftToForceExitAndRemoveMap);
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeftToForceExitAndRemoveMap, "ticksLeftToForceExitAndRemoveMap", -1, false);
			Scribe_Values.Look<bool>(ref this.anyCaravanEverFormed, "anyCaravanEverFormed", false, false);
		}

		public virtual void PostMapGenerate()
		{
		}

		public virtual void Notify_MyMapRemoved(Map map)
		{
		}

		public virtual void Notify_CaravanFormed(Caravan caravan)
		{
			if (!this.anyCaravanEverFormed)
			{
				this.anyCaravanEverFormed = true;
				if (base.def.isTempIncidentMapOwner && this.HasMap)
				{
					this.Map.StoryState.CopyTo(caravan.StoryState);
				}
			}
		}

		public virtual bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			alsoRemoveWorldObject = false;
			return false;
		}

		public override void PostRemove()
		{
			base.PostRemove();
			if (this.HasMap)
			{
				Current.Game.DeinitAndRemoveMap(this.Map);
			}
		}

		public void StartForceExitAndRemoveMapCountdown()
		{
			this.StartForceExitAndRemoveMapCountdown(60000);
		}

		public void StartForceExitAndRemoveMapCountdown(int duration)
		{
			this.ticksLeftToForceExitAndRemoveMap = duration;
		}

		public override void Tick()
		{
			base.Tick();
			this.TickForceExitAndRemoveMapCountdown();
			this.CheckRemoveMapNow();
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			if (this.HasMap)
			{
				yield return (Gizmo)new Command_Action
				{
					defaultLabel = "CommandShowMap".Translate(),
					defaultDesc = "CommandShowMapDesc".Translate(),
					icon = MapParent.ShowMapCommand,
					hotKey = KeyBindingDefOf.Misc1,
					action = (Action)delegate
					{
						Current.Game.VisibleMap = ((_003CGetGizmos_003Ec__Iterator103)/*Error near IL_011d: stateMachine*/)._003C_003Ef__this.Map;
						if (!CameraJumper.TryHideWorld())
						{
							SoundDefOf.TabClose.PlayOneShotOnCamera(null);
						}
					}
				};
				if (this is FactionBase && base.Faction == Faction.OfPlayer)
				{
					yield return (Gizmo)new Command_Action
					{
						defaultLabel = "CommandFormCaravan".Translate(),
						defaultDesc = "CommandFormCaravanDesc".Translate(),
						icon = MapParent.FormCaravanCommand,
						hotKey = KeyBindingDefOf.Misc2,
						tutorTag = "FormCaravan",
						action = (Action)delegate
						{
							Find.WindowStack.Add(new Dialog_FormCaravan(((_003CGetGizmos_003Ec__Iterator103)/*Error near IL_01d6: stateMachine*/)._003C_003Ef__this.Map, false, null, true));
						}
					};
				}
				else if (this.Map.mapPawns.FreeColonistsSpawnedCount != 0)
				{
					Command_Action reformCaravan = new Command_Action
					{
						defaultLabel = "CommandReformCaravan".Translate(),
						defaultDesc = "CommandReformCaravanDesc".Translate(),
						icon = MapParent.FormCaravanCommand,
						hotKey = KeyBindingDefOf.Misc2,
						tutorTag = "ReformCaravan",
						action = (Action)delegate
						{
							Find.WindowStack.Add(new Dialog_FormCaravan(((_003CGetGizmos_003Ec__Iterator103)/*Error near IL_0289: stateMachine*/)._003C_003Ef__this.Map, true, null, true));
						}
					};
					if (GenHostility.AnyHostileActiveThreat(this.Map))
					{
						reformCaravan.Disable("CommandReformCaravanFailHostilePawns".Translate());
					}
					yield return (Gizmo)reformCaravan;
				}
			}
		}

		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetFloatMenuOptions(caravan))
			{
				yield return floatMenuOption;
			}
			if (this.HasMap && this.UseGenericEnterMapFloatMenuOption)
			{
				yield return new FloatMenuOption("EnterMap".Translate(this.Label), (Action)delegate
				{
					((_003CGetFloatMenuOptions_003Ec__Iterator104)/*Error near IL_00fa: stateMachine*/).caravan.pather.StartPath(((_003CGetFloatMenuOptions_003Ec__Iterator104)/*Error near IL_00fa: stateMachine*/)._003C_003Ef__this.Tile, new CaravanArrivalAction_Enter(((_003CGetFloatMenuOptions_003Ec__Iterator104)/*Error near IL_00fa: stateMachine*/)._003C_003Ef__this), true);
				}, MenuOptionPriority.Default, null, null, 0f, null, this);
				if (Prefs.DevMode)
				{
					yield return new FloatMenuOption("EnterMap".Translate(this.Label) + " (Dev: instantly)", (Action)delegate
					{
						((_003CGetFloatMenuOptions_003Ec__Iterator104)/*Error near IL_0160: stateMachine*/).caravan.Tile = ((_003CGetFloatMenuOptions_003Ec__Iterator104)/*Error near IL_0160: stateMachine*/)._003C_003Ef__this.Tile;
						new CaravanArrivalAction_Enter(((_003CGetFloatMenuOptions_003Ec__Iterator104)/*Error near IL_0160: stateMachine*/)._003C_003Ef__this).Arrived(((_003CGetFloatMenuOptions_003Ec__Iterator104)/*Error near IL_0160: stateMachine*/).caravan);
					}, MenuOptionPriority.Default, null, null, 0f, null, this);
				}
			}
		}

		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (this.ForceExitAndRemoveMapCountdownActive)
			{
				if (text.Length > 0)
				{
					text += "\n";
				}
				text = text + "ForceExitAndRemoveMapCountdown".Translate(this.ForceExitAndRemoveMapCountdownTimeLeftString) + ".";
			}
			return text;
		}

		public static string GetForceExitAndRemoveMapCountdownTimeLeftString(int ticksLeft)
		{
			if (ticksLeft < 0)
			{
				return string.Empty;
			}
			return ticksLeft.ToStringTicksToPeriod(true, true, true);
		}

		private void TickForceExitAndRemoveMapCountdown()
		{
			if (this.ForceExitAndRemoveMapCountdownActive)
			{
				if (this.HasMap)
				{
					this.ticksLeftToForceExitAndRemoveMap--;
					if (this.ticksLeftToForceExitAndRemoveMap == 0)
					{
						if (Dialog_FormCaravan.AllSendablePawns(this.Map, true).Any((Predicate<Pawn>)((Pawn x) => x.IsColonist)))
						{
							Messages.Message("MessageYouHaveToReformCaravanNow".Translate(), new GlobalTargetInfo(base.Tile), MessageSound.Standard);
							Current.Game.VisibleMap = this.Map;
							Dialog_FormCaravan window = new Dialog_FormCaravan(this.Map, true, (Action)delegate
							{
								if (this.HasMap)
								{
									this.ShowWorldViewIfVisibleMapAboutToBeRemoved(this.Map);
									Find.WorldObjects.Remove(this);
								}
							}, false);
							Find.WindowStack.Add(window);
						}
						else
						{
							MapParent.tmpPawns.Clear();
							MapParent.tmpPawns.AddRange(from x in this.Map.mapPawns.AllPawns
							where x.Faction == Faction.OfPlayer || x.HostFaction == Faction.OfPlayer
							select x);
							if (MapParent.tmpPawns.Any())
							{
								if (MapParent.tmpPawns.Any((Predicate<Pawn>)((Pawn x) => CaravanUtility.IsOwner(x, Faction.OfPlayer))))
								{
									Caravan o = CaravanExitMapUtility.ExitMapAndCreateCaravan(MapParent.tmpPawns, Faction.OfPlayer, base.Tile);
									Messages.Message("MessageAutomaticallyReformedCaravan".Translate(), (WorldObject)o, MessageSound.Benefit);
								}
								else
								{
									StringBuilder stringBuilder = new StringBuilder();
									for (int i = 0; i < MapParent.tmpPawns.Count; i++)
									{
										stringBuilder.AppendLine("    " + MapParent.tmpPawns[i].LabelCap);
									}
									Find.LetterStack.ReceiveLetter("LetterLabelPawnsLostDueToMapCountdown".Translate(), "LetterPawnsLostDueToMapCountdown".Translate(stringBuilder.ToString().TrimEndNewlines()), LetterDefOf.BadNonUrgent, new GlobalTargetInfo(base.Tile), (string)null);
								}
								MapParent.tmpPawns.Clear();
							}
							this.ShowWorldViewIfVisibleMapAboutToBeRemoved(this.Map);
							Find.WorldObjects.Remove(this);
						}
					}
				}
				else
				{
					this.ticksLeftToForceExitAndRemoveMap = -1;
				}
			}
		}

		public void CheckRemoveMapNow()
		{
			bool flag = default(bool);
			if (this.HasMap && this.ShouldRemoveMapNow(out flag))
			{
				Map map = this.Map;
				this.ShowWorldViewIfVisibleMapAboutToBeRemoved(map);
				Current.Game.DeinitAndRemoveMap(map);
				if (flag)
				{
					Find.WorldObjects.Remove(this);
				}
			}
		}

		private void ShowWorldViewIfVisibleMapAboutToBeRemoved(Map map)
		{
			if (map == Find.VisibleMap)
			{
				Find.World.renderer.wantedMode = WorldRenderMode.Planet;
			}
		}

		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		public virtual void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
			if (this.HasMap)
			{
				outChildren.Add(this.Map);
			}
		}

		virtual IThingHolder get_ParentHolder()
		{
			return base.ParentHolder;
		}

		IThingHolder IThingHolder.get_ParentHolder()
		{
			//ILSpy generated this explicit interface implementation from .override directive in get_ParentHolder
			return this.get_ParentHolder();
		}
	}
}
