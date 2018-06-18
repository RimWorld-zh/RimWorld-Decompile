using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020004E1 RID: 1249
	public class Pawn_StoryTracker : IExposable
	{
		// Token: 0x0600163B RID: 5691 RVA: 0x000C53D4 File Offset: 0x000C37D4
		public Pawn_StoryTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.traits = new TraitSet(pawn);
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x0600163C RID: 5692 RVA: 0x000C5430 File Offset: 0x000C3830
		// (set) Token: 0x0600163D RID: 5693 RVA: 0x000C5462 File Offset: 0x000C3862
		public string Title
		{
			get
			{
				string titleDefault;
				if (this.title != null)
				{
					titleDefault = this.title;
				}
				else
				{
					titleDefault = this.TitleDefault;
				}
				return titleDefault;
			}
			set
			{
				this.title = null;
				if (value != this.Title && !value.NullOrEmpty())
				{
					this.title = value;
				}
			}
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x0600163E RID: 5694 RVA: 0x000C5490 File Offset: 0x000C3890
		public string TitleCap
		{
			get
			{
				return this.Title.CapitalizeFirst();
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x0600163F RID: 5695 RVA: 0x000C54B0 File Offset: 0x000C38B0
		public string TitleDefault
		{
			get
			{
				string result;
				if (this.adulthood != null)
				{
					result = this.adulthood.TitleFor(this.pawn.gender);
				}
				else if (this.childhood != null)
				{
					result = this.childhood.TitleFor(this.pawn.gender);
				}
				else
				{
					result = "";
				}
				return result;
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06001640 RID: 5696 RVA: 0x000C5518 File Offset: 0x000C3918
		public string TitleDefaultCap
		{
			get
			{
				return this.TitleDefault.CapitalizeFirst();
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06001641 RID: 5697 RVA: 0x000C5538 File Offset: 0x000C3938
		public string TitleShort
		{
			get
			{
				string result;
				if (this.title != null)
				{
					result = this.title;
				}
				else if (this.adulthood != null)
				{
					result = this.adulthood.TitleShortFor(this.pawn.gender);
				}
				else if (this.childhood != null)
				{
					result = this.childhood.TitleShortFor(this.pawn.gender);
				}
				else
				{
					result = "";
				}
				return result;
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06001642 RID: 5698 RVA: 0x000C55B8 File Offset: 0x000C39B8
		public string TitleShortCap
		{
			get
			{
				return this.TitleShort.CapitalizeFirst();
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06001643 RID: 5699 RVA: 0x000C55D8 File Offset: 0x000C39D8
		public Color SkinColor
		{
			get
			{
				return PawnSkinColors.GetSkinColor(this.melanin);
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06001644 RID: 5700 RVA: 0x000C55F8 File Offset: 0x000C39F8
		public IEnumerable<Backstory> AllBackstories
		{
			get
			{
				if (this.childhood != null)
				{
					yield return this.childhood;
				}
				if (this.adulthood != null)
				{
					yield return this.adulthood;
				}
				yield break;
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06001645 RID: 5701 RVA: 0x000C5624 File Offset: 0x000C3A24
		public string HeadGraphicPath
		{
			get
			{
				if (this.headGraphicPath == null)
				{
					this.headGraphicPath = GraphicDatabaseHeadRecords.GetHeadRandom(this.pawn.gender, this.pawn.story.SkinColor, this.pawn.story.crownType).GraphicPath;
				}
				return this.headGraphicPath;
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06001646 RID: 5702 RVA: 0x000C5688 File Offset: 0x000C3A88
		public List<WorkTypeDef> DisabledWorkTypes
		{
			get
			{
				if (this.cachedDisabledWorkTypes == null)
				{
					this.cachedDisabledWorkTypes = new List<WorkTypeDef>();
					foreach (Backstory backstory in this.AllBackstories)
					{
						foreach (WorkTypeDef item in backstory.DisabledWorkTypes)
						{
							if (!this.cachedDisabledWorkTypes.Contains(item))
							{
								this.cachedDisabledWorkTypes.Add(item);
							}
						}
					}
					for (int i = 0; i < this.traits.allTraits.Count; i++)
					{
						foreach (WorkTypeDef item2 in this.traits.allTraits[i].GetDisabledWorkTypes())
						{
							if (!this.cachedDisabledWorkTypes.Contains(item2))
							{
								this.cachedDisabledWorkTypes.Add(item2);
							}
						}
					}
				}
				return this.cachedDisabledWorkTypes;
			}
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06001647 RID: 5703 RVA: 0x000C5808 File Offset: 0x000C3C08
		public WorkTags CombinedDisabledWorkTags
		{
			get
			{
				WorkTags workTags = WorkTags.None;
				if (this.childhood != null)
				{
					workTags |= this.childhood.workDisables;
				}
				if (this.adulthood != null)
				{
					workTags |= this.adulthood.workDisables;
				}
				for (int i = 0; i < this.traits.allTraits.Count; i++)
				{
					workTags |= this.traits.allTraits[i].def.disabledWorkTags;
				}
				return workTags;
			}
		}

		// Token: 0x06001648 RID: 5704 RVA: 0x000C5894 File Offset: 0x000C3C94
		public void ExposeData()
		{
			string text = (this.childhood == null) ? null : this.childhood.identifier;
			Scribe_Values.Look<string>(ref text, "childhood", null, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && !text.NullOrEmpty())
			{
				if (!BackstoryDatabase.TryGetWithIdentifier(text, out this.childhood, true))
				{
					Log.Error("Couldn't load child backstory with identifier " + text + ". Giving random.", false);
					this.childhood = BackstoryDatabase.RandomBackstory(BackstorySlot.Childhood);
				}
			}
			string text2 = (this.adulthood == null) ? null : this.adulthood.identifier;
			Scribe_Values.Look<string>(ref text2, "adulthood", null, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && !text2.NullOrEmpty())
			{
				if (!BackstoryDatabase.TryGetWithIdentifier(text2, out this.adulthood, true))
				{
					Log.Error("Couldn't load adult backstory with identifier " + text2 + ". Giving random.", false);
					this.adulthood = BackstoryDatabase.RandomBackstory(BackstorySlot.Adulthood);
				}
			}
			Scribe_Defs.Look<BodyTypeDef>(ref this.bodyType, "bodyType");
			Scribe_Values.Look<CrownType>(ref this.crownType, "crownType", CrownType.Undefined, false);
			Scribe_Values.Look<string>(ref this.headGraphicPath, "headGraphicPath", null, false);
			Scribe_Defs.Look<HairDef>(ref this.hairDef, "hairDef");
			Scribe_Values.Look<Color>(ref this.hairColor, "hairColor", default(Color), false);
			Scribe_Values.Look<float>(ref this.melanin, "melanin", 0f, false);
			Scribe_Deep.Look<TraitSet>(ref this.traits, "traits", new object[]
			{
				this.pawn
			});
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.hairDef == null)
				{
					this.hairDef = DefDatabase<HairDef>.AllDefs.RandomElement<HairDef>();
				}
			}
		}

		// Token: 0x06001649 RID: 5705 RVA: 0x000C5A64 File Offset: 0x000C3E64
		public Backstory GetBackstory(BackstorySlot slot)
		{
			Backstory result;
			if (slot == BackstorySlot.Childhood)
			{
				result = this.childhood;
			}
			else
			{
				result = this.adulthood;
			}
			return result;
		}

		// Token: 0x0600164A RID: 5706 RVA: 0x000C5A94 File Offset: 0x000C3E94
		public bool WorkTypeIsDisabled(WorkTypeDef w)
		{
			return this.DisabledWorkTypes.Contains(w);
		}

		// Token: 0x0600164B RID: 5707 RVA: 0x000C5AB8 File Offset: 0x000C3EB8
		public bool OneOfWorkTypesIsDisabled(List<WorkTypeDef> wts)
		{
			for (int i = 0; i < wts.Count; i++)
			{
				if (this.WorkTypeIsDisabled(wts[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600164C RID: 5708 RVA: 0x000C5B00 File Offset: 0x000C3F00
		public bool WorkTagIsDisabled(WorkTags w)
		{
			return (this.CombinedDisabledWorkTags & w) != WorkTags.None;
		}

		// Token: 0x0600164D RID: 5709 RVA: 0x000C5B23 File Offset: 0x000C3F23
		internal void Notify_TraitChanged()
		{
			this.cachedDisabledWorkTypes = null;
		}

		// Token: 0x04000CED RID: 3309
		private Pawn pawn;

		// Token: 0x04000CEE RID: 3310
		public Backstory childhood;

		// Token: 0x04000CEF RID: 3311
		public Backstory adulthood;

		// Token: 0x04000CF0 RID: 3312
		public float melanin;

		// Token: 0x04000CF1 RID: 3313
		public Color hairColor = Color.white;

		// Token: 0x04000CF2 RID: 3314
		public CrownType crownType = CrownType.Undefined;

		// Token: 0x04000CF3 RID: 3315
		public BodyTypeDef bodyType = null;

		// Token: 0x04000CF4 RID: 3316
		private string headGraphicPath = null;

		// Token: 0x04000CF5 RID: 3317
		public HairDef hairDef = null;

		// Token: 0x04000CF6 RID: 3318
		public TraitSet traits;

		// Token: 0x04000CF7 RID: 3319
		public string title = null;

		// Token: 0x04000CF8 RID: 3320
		private List<WorkTypeDef> cachedDisabledWorkTypes = null;
	}
}
