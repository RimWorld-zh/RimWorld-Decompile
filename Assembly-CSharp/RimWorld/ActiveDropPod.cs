using System;
using System.Collections.Generic;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020006E1 RID: 1761
	public class ActiveDropPod : Thing, IActiveDropPod, IThingHolder
	{
		// Token: 0x0400155C RID: 5468
		public int age = 0;

		// Token: 0x0400155D RID: 5469
		private ActiveDropPodInfo contents;

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x0600264C RID: 9804 RVA: 0x00148A48 File Offset: 0x00146E48
		// (set) Token: 0x0600264D RID: 9805 RVA: 0x00148A63 File Offset: 0x00146E63
		public ActiveDropPodInfo Contents
		{
			get
			{
				return this.contents;
			}
			set
			{
				if (this.contents != null)
				{
					this.contents.parent = null;
				}
				if (value != null)
				{
					value.parent = this;
				}
				this.contents = value;
			}
		}

		// Token: 0x0600264E RID: 9806 RVA: 0x00148A91 File Offset: 0x00146E91
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
			Scribe_Deep.Look<ActiveDropPodInfo>(ref this.contents, "contents", new object[]
			{
				this
			});
		}

		// Token: 0x0600264F RID: 9807 RVA: 0x00148AC8 File Offset: 0x00146EC8
		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		// Token: 0x06002650 RID: 9808 RVA: 0x00148ADE File Offset: 0x00146EDE
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
			if (this.contents != null)
			{
				outChildren.Add(this.contents);
			}
		}

		// Token: 0x06002651 RID: 9809 RVA: 0x00148B04 File Offset: 0x00146F04
		public override void Tick()
		{
			if (this.contents != null)
			{
				this.contents.innerContainer.ThingOwnerTick(true);
				if (base.Spawned)
				{
					this.age++;
					if (this.age > this.contents.openDelay)
					{
						this.PodOpen();
					}
				}
			}
		}

		// Token: 0x06002652 RID: 9810 RVA: 0x00148B6C File Offset: 0x00146F6C
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.contents != null)
			{
				this.contents.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			}
			Map map = base.Map;
			base.Destroy(mode);
			if (mode == DestroyMode.KillFinalize)
			{
				for (int i = 0; i < 1; i++)
				{
					Thing thing = ThingMaker.MakeThing(ThingDefOf.ChunkSlagSteel, null);
					GenPlace.TryPlaceThing(thing, base.Position, map, ThingPlaceMode.Near, null, null);
				}
			}
		}

		// Token: 0x06002653 RID: 9811 RVA: 0x00148BE0 File Offset: 0x00146FE0
		private void PodOpen()
		{
			for (int i = this.contents.innerContainer.Count - 1; i >= 0; i--)
			{
				Thing thing = this.contents.innerContainer[i];
				Thing thing2;
				GenPlace.TryPlaceThing(thing, base.Position, base.Map, ThingPlaceMode.Near, out thing2, delegate(Thing placedThing, int count)
				{
					if (Find.TickManager.TicksGame < 1200 && TutorSystem.TutorialMode && placedThing.def.category == ThingCategory.Item)
					{
						Find.TutorialState.AddStartingItem(placedThing);
					}
				}, null);
				Pawn pawn = thing2 as Pawn;
				if (pawn != null)
				{
					if (pawn.RaceProps.Humanlike)
					{
						TaleRecorder.RecordTale(TaleDefOf.LandedInPod, new object[]
						{
							pawn
						});
					}
					if (pawn.IsColonist && pawn.Spawned && !base.Map.IsPlayerHome)
					{
						pawn.drafter.Drafted = true;
					}
				}
			}
			this.contents.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			if (this.contents.leaveSlag)
			{
				for (int j = 0; j < 1; j++)
				{
					Thing thing3 = ThingMaker.MakeThing(ThingDefOf.ChunkSlagSteel, null);
					GenPlace.TryPlaceThing(thing3, base.Position, base.Map, ThingPlaceMode.Near, null, null);
				}
			}
			SoundDefOf.DropPod_Open.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
			this.Destroy(DestroyMode.Vanish);
		}
	}
}
