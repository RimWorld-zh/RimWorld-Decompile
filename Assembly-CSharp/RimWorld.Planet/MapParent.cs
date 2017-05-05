using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

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
				MapParent.<>c__Iterator101 <>c__Iterator = new MapParent.<>c__Iterator101();
				MapParent.<>c__Iterator101 expr_07 = <>c__Iterator;
				expr_07.$PC = -2;
				return expr_07;
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
				if (this.def.isTempIncidentMapOwner && this.HasMap)
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

		[DebuggerHidden]
		public override IEnumerable<Gizmo> GetGizmos()
		{
			MapParent.<GetGizmos>c__Iterator102 <GetGizmos>c__Iterator = new MapParent.<GetGizmos>c__Iterator102();
			<GetGizmos>c__Iterator.<>f__this = this;
			MapParent.<GetGizmos>c__Iterator102 expr_0E = <GetGizmos>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		[DebuggerHidden]
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			MapParent.<GetFloatMenuOptions>c__Iterator103 <GetFloatMenuOptions>c__Iterator = new MapParent.<GetFloatMenuOptions>c__Iterator103();
			<GetFloatMenuOptions>c__Iterator.caravan = caravan;
			<GetFloatMenuOptions>c__Iterator.<$>caravan = caravan;
			<GetFloatMenuOptions>c__Iterator.<>f__this = this;
			MapParent.<GetFloatMenuOptions>c__Iterator103 expr_1C = <GetFloatMenuOptions>c__Iterator;
			expr_1C.$PC = -2;
			return expr_1C;
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
				text = text + "ForceExitAndRemoveMapCountdown".Translate(new object[]
				{
					this.ForceExitAndRemoveMapCountdownTimeLeftString
				}) + ".";
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
						if (Dialog_FormCaravan.AllSendablePawns(this.Map, true).Any((Pawn x) => x.IsColonist))
						{
							Messages.Message("MessageYouHaveToReformCaravanNow".Translate(), new GlobalTargetInfo(base.Tile), MessageSound.Standard);
							Current.Game.VisibleMap = this.Map;
							Dialog_FormCaravan window = new Dialog_FormCaravan(this.Map, true, delegate
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
							if (MapParent.tmpPawns.Any<Pawn>())
							{
								if (MapParent.tmpPawns.Any((Pawn x) => CaravanUtility.IsOwner(x, Faction.OfPlayer)))
								{
									Caravan o = CaravanExitMapUtility.ExitMapAndCreateCaravan(MapParent.tmpPawns, Faction.OfPlayer, base.Tile);
									Messages.Message("MessageAutomaticallyReformedCaravan".Translate(), o, MessageSound.Benefit);
								}
								else
								{
									StringBuilder stringBuilder = new StringBuilder();
									for (int i = 0; i < MapParent.tmpPawns.Count; i++)
									{
										stringBuilder.AppendLine("    " + MapParent.tmpPawns[i].LabelCap);
									}
									Find.LetterStack.ReceiveLetter("LetterLabelPawnsLostDueToMapCountdown".Translate(), "LetterPawnsLostDueToMapCountdown".Translate(new object[]
									{
										stringBuilder.ToString().TrimEndNewlines()
									}), LetterDefOf.BadNonUrgent, new GlobalTargetInfo(base.Tile), null);
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
			bool flag;
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
	}
}
