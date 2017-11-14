using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	[StaticConstructorOnStartup]
	public class MapParent : WorldObject, IThingHolder
	{
		private bool anyCaravanEverFormed;

		private static readonly Texture2D ShowMapCommand = ContentFinder<Texture2D>.Get("UI/Commands/ShowMap", true);

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
				return base.def.mapGenerator;
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

		public virtual IntVec3 MapSizeGeneratedByTransportPodsArrival
		{
			get
			{
				return Find.World.info.initialMapSize;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
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

		public override void Tick()
		{
			base.Tick();
			this.CheckRemoveMapNow();
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			using (IEnumerator<Gizmo> enumerator = base.GetGizmos().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Gizmo g = enumerator.Current;
					yield return g;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (!this.HasMap)
				yield break;
			yield return (Gizmo)new Command_Action
			{
				defaultLabel = "CommandShowMap".Translate(),
				defaultDesc = "CommandShowMapDesc".Translate(),
				icon = MapParent.ShowMapCommand,
				hotKey = KeyBindingDefOf.Misc1,
				action = delegate
				{
					Current.Game.VisibleMap = ((_003CGetGizmos_003Ec__Iterator1)/*Error near IL_011f: stateMachine*/)._0024this.Map;
					if (!CameraJumper.TryHideWorld())
					{
						SoundDefOf.TabClose.PlayOneShotOnCamera(null);
					}
				}
			};
			/*Error: Unable to find new state assignment for yield return*/;
			IL_0159:
			/*Error near IL_015a: Unexpected return in MoveNext()*/;
		}

		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			_003CGetFloatMenuOptions_003Ec__Iterator2 _003CGetFloatMenuOptions_003Ec__Iterator = (_003CGetFloatMenuOptions_003Ec__Iterator2)/*Error near IL_003c: stateMachine*/;
			using (IEnumerator<FloatMenuOption> enumerator = this._003CGetFloatMenuOptions_003E__BaseCallProxy1(caravan).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					FloatMenuOption o = enumerator.Current;
					yield return o;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (!this.HasMap)
				yield break;
			if (!this.UseGenericEnterMapFloatMenuOption)
				yield break;
			yield return new FloatMenuOption("EnterMap".Translate(this.Label), delegate
			{
				caravan.pather.StartPath(_003CGetFloatMenuOptions_003Ec__Iterator._0024this.Tile, new CaravanArrivalAction_Enter(_003CGetFloatMenuOptions_003Ec__Iterator._0024this), true);
			}, MenuOptionPriority.Default, null, null, 0f, null, this);
			/*Error: Unable to find new state assignment for yield return*/;
			IL_01e2:
			/*Error near IL_01e3: Unexpected return in MoveNext()*/;
		}

		public void CheckRemoveMapNow()
		{
			bool flag = default(bool);
			if (this.HasMap && this.ShouldRemoveMapNow(out flag))
			{
				Map map = this.Map;
				Current.Game.DeinitAndRemoveMap(map);
				if (flag)
				{
					Find.WorldObjects.Remove(this);
				}
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
	}
}
