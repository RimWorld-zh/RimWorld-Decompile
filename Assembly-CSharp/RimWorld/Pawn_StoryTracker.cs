using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Pawn_StoryTracker : IExposable
	{
		private Pawn pawn;

		public Backstory childhood;

		public Backstory adulthood;

		public float melanin;

		public Color hairColor = Color.white;

		public CrownType crownType = CrownType.Undefined;

		public BodyTypeDef bodyType = null;

		private string headGraphicPath = null;

		public HairDef hairDef = null;

		public TraitSet traits;

		public string title = null;

		private List<WorkTypeDef> cachedDisabledWorkTypes = null;

		public Pawn_StoryTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.traits = new TraitSet(pawn);
		}

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

		public string TitleCap
		{
			get
			{
				return this.Title.CapitalizeFirst();
			}
		}

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

		public string TitleDefaultCap
		{
			get
			{
				return this.TitleDefault.CapitalizeFirst();
			}
		}

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

		public string TitleShortCap
		{
			get
			{
				return this.TitleShort.CapitalizeFirst();
			}
		}

		public Color SkinColor
		{
			get
			{
				return PawnSkinColors.GetSkinColor(this.melanin);
			}
		}

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

		public bool WorkTypeIsDisabled(WorkTypeDef w)
		{
			return this.DisabledWorkTypes.Contains(w);
		}

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

		public bool WorkTagIsDisabled(WorkTags w)
		{
			return (this.CombinedDisabledWorkTags & w) != WorkTags.None;
		}

		internal void Notify_TraitChanged()
		{
			this.cachedDisabledWorkTypes = null;
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<Backstory>, IEnumerator, IDisposable, IEnumerator<Backstory>
		{
			internal Pawn_StoryTracker $this;

			internal Backstory $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (this.childhood != null)
					{
						this.$current = this.childhood;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					break;
				case 1u:
					break;
				case 2u:
					goto IL_90;
				default:
					return false;
				}
				if (this.adulthood != null)
				{
					this.$current = this.adulthood;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_90:
				this.$PC = -1;
				return false;
			}

			Backstory IEnumerator<Backstory>.Current
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
				return this.System.Collections.Generic.IEnumerable<RimWorld.Backstory>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Backstory> IEnumerable<Backstory>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Pawn_StoryTracker.<>c__Iterator0 <>c__Iterator = new Pawn_StoryTracker.<>c__Iterator0();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}
	}
}
