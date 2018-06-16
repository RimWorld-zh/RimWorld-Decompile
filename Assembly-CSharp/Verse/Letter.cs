using System;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000E76 RID: 3702
	public abstract class Letter : IArchivable, ILoadReferenceable, IExposable
	{
		// Token: 0x17000DBB RID: 3515
		// (get) Token: 0x06005710 RID: 22288 RVA: 0x0019FB40 File Offset: 0x0019DF40
		public virtual bool CanShowInLetterStack
		{
			get
			{
				bool result;
				if (this.lookTargets == null || !this.lookTargets.Any)
				{
					result = true;
				}
				else
				{
					int i = 0;
					while (i < this.lookTargets.targets.Count)
					{
						GlobalTargetInfo globalTargetInfo = this.lookTargets.targets[i];
						if (this.def == LetterDefOf.Death || globalTargetInfo.Thing == null || !globalTargetInfo.Thing.Destroyed)
						{
							goto IL_B5;
						}
						Pawn pawn = globalTargetInfo.Thing as Pawn;
						if (pawn != null && !pawn.Corpse.DestroyedOrNull() && (pawn.Corpse.Spawned || pawn.Corpse.ParentHolder != null))
						{
							goto IL_B5;
						}
						IL_DE:
						i++;
						continue;
						IL_B5:
						if (globalTargetInfo.WorldObject != null && !globalTargetInfo.WorldObject.Spawned)
						{
							goto IL_DE;
						}
						return true;
					}
					result = false;
				}
				return result;
			}
		}

		// Token: 0x17000DBC RID: 3516
		// (get) Token: 0x06005711 RID: 22289 RVA: 0x0019FC50 File Offset: 0x0019E050
		public bool ArchivedOnly
		{
			get
			{
				return !Find.LetterStack.LettersListForReading.Contains(this);
			}
		}

		// Token: 0x17000DBD RID: 3517
		// (get) Token: 0x06005712 RID: 22290 RVA: 0x0019FC78 File Offset: 0x0019E078
		public IThingHolder ParentHolder
		{
			get
			{
				return Find.World;
			}
		}

		// Token: 0x17000DB4 RID: 3508
		// (get) Token: 0x06005713 RID: 22291 RVA: 0x0019FC94 File Offset: 0x0019E094
		Texture IArchivable.ArchivedIcon
		{
			get
			{
				return this.def.Icon;
			}
		}

		// Token: 0x17000DB5 RID: 3509
		// (get) Token: 0x06005714 RID: 22292 RVA: 0x0019FCB4 File Offset: 0x0019E0B4
		Color IArchivable.ArchivedIconColor
		{
			get
			{
				return this.def.color;
			}
		}

		// Token: 0x17000DB6 RID: 3510
		// (get) Token: 0x06005715 RID: 22293 RVA: 0x0019FCD4 File Offset: 0x0019E0D4
		string IArchivable.ArchivedLabel
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x17000DB7 RID: 3511
		// (get) Token: 0x06005716 RID: 22294 RVA: 0x0019FCF0 File Offset: 0x0019E0F0
		string IArchivable.ArchivedTooltip
		{
			get
			{
				return this.GetMouseoverText();
			}
		}

		// Token: 0x17000DB8 RID: 3512
		// (get) Token: 0x06005717 RID: 22295 RVA: 0x0019FD0C File Offset: 0x0019E10C
		int IArchivable.CreatedTicksGame
		{
			get
			{
				return this.arrivalTick;
			}
		}

		// Token: 0x17000DB9 RID: 3513
		// (get) Token: 0x06005718 RID: 22296 RVA: 0x0019FD28 File Offset: 0x0019E128
		bool IArchivable.CanCullArchivedNow
		{
			get
			{
				return !Find.LetterStack.LettersListForReading.Contains(this);
			}
		}

		// Token: 0x17000DBA RID: 3514
		// (get) Token: 0x06005719 RID: 22297 RVA: 0x0019FD50 File Offset: 0x0019E150
		LookTargets IArchivable.LookTargets
		{
			get
			{
				return this.lookTargets;
			}
		}

		// Token: 0x0600571A RID: 22298 RVA: 0x0019FD6C File Offset: 0x0019E16C
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ID, "ID", 0, false);
			Scribe_Defs.Look<LetterDef>(ref this.def, "def");
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", new object[0]);
			Scribe_References.Look<Faction>(ref this.relatedFaction, "relatedFaction", false);
			Scribe_Values.Look<int>(ref this.arrivalTick, "arrivalTick", 0, false);
		}

		// Token: 0x0600571B RID: 22299 RVA: 0x0019FDE8 File Offset: 0x0019E1E8
		public virtual void DrawButtonAt(float topY)
		{
			float num = (float)UI.screenWidth - 38f - 12f;
			Rect rect = new Rect(num, topY, 38f, 30f);
			Rect rect2 = new Rect(rect);
			float num2 = Time.time - this.arrivalTime;
			Color color = this.def.color;
			if (num2 < 1f)
			{
				rect2.y -= (1f - num2) * 200f;
				color.a = num2 / 1f;
			}
			if (!Mouse.IsOver(rect) && this.def.bounce && num2 > 15f)
			{
				if (num2 % 5f < 1f)
				{
					float num3 = (float)UI.screenWidth * 0.06f;
					float num4 = 2f * (num2 % 1f) - 1f;
					float num5 = num3 * (1f - num4 * num4);
					rect2.x -= num5;
				}
			}
			if (Event.current.type == EventType.Repaint)
			{
				if (this.def.flashInterval > 0f)
				{
					float num6 = Time.time - (this.arrivalTime + 1f);
					if (num6 > 0f && num6 % this.def.flashInterval < 1f)
					{
						GenUI.DrawFlash(num, topY, (float)UI.screenWidth * 0.6f, Pulser.PulseBrightness(1f, 1f, num6) * 0.55f, this.def.flashColor);
					}
				}
				GUI.color = color;
				Widgets.DrawShadowAround(rect2);
				GUI.DrawTexture(rect2, this.def.Icon);
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperRight;
				string text = this.PostProcessedLabel();
				Vector2 vector = Text.CalcSize(text);
				float x = vector.x;
				float y = vector.y;
				Vector2 vector2 = new Vector2(rect2.x + rect2.width / 2f, rect2.center.y - y / 2f + 4f);
				float num7 = vector2.x + x / 2f - (float)(UI.screenWidth - 2);
				if (num7 > 0f)
				{
					vector2.x -= num7;
				}
				Rect position = new Rect(vector2.x - x / 2f - 6f - 1f, vector2.y, x + 12f, 16f);
				GUI.DrawTexture(position, TexUI.GrayTextBG);
				GUI.color = new Color(1f, 1f, 1f, 0.75f);
				Rect rect3 = new Rect(vector2.x - x / 2f, vector2.y - 3f, x, 999f);
				Widgets.Label(rect3, text);
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
			}
			if (Event.current.type == EventType.MouseDown && Event.current.button == 1 && Mouse.IsOver(rect))
			{
				SoundDefOf.Click.PlayOneShotOnCamera(null);
				Find.LetterStack.RemoveLetter(this);
				Event.current.Use();
			}
			if (Widgets.ButtonInvisible(rect2, false))
			{
				this.OpenLetter();
				Event.current.Use();
			}
		}

		// Token: 0x0600571C RID: 22300 RVA: 0x001A015C File Offset: 0x0019E55C
		public virtual void CheckForMouseOverTextAt(float topY)
		{
			float num = (float)UI.screenWidth - 38f - 12f;
			Rect rect = new Rect(num, topY, 38f, 30f);
			if (Mouse.IsOver(rect))
			{
				Find.LetterStack.Notify_LetterMouseover(this);
				string mouseoverText = this.GetMouseoverText();
				if (!mouseoverText.NullOrEmpty())
				{
					Text.Font = GameFont.Small;
					Text.Anchor = TextAnchor.UpperLeft;
					float num2 = Text.CalcHeight(mouseoverText, 310f);
					num2 += 20f;
					float x = num - 330f - 10f;
					Rect infoRect = new Rect(x, topY - num2 / 2f, 330f, num2);
					Find.WindowStack.ImmediateWindow(2768333, infoRect, WindowLayer.Super, delegate
					{
						Text.Font = GameFont.Small;
						Rect position = infoRect.AtZero().ContractedBy(10f);
						GUI.BeginGroup(position);
						Widgets.Label(new Rect(0f, 0f, position.width, position.height), mouseoverText);
						GUI.EndGroup();
					}, true, false, 1f);
				}
			}
		}

		// Token: 0x0600571D RID: 22301
		protected abstract string GetMouseoverText();

		// Token: 0x0600571E RID: 22302
		public abstract void OpenLetter();

		// Token: 0x0600571F RID: 22303 RVA: 0x001A0257 File Offset: 0x0019E657
		public virtual void Received()
		{
		}

		// Token: 0x06005720 RID: 22304 RVA: 0x001A025A File Offset: 0x0019E65A
		public virtual void Removed()
		{
		}

		// Token: 0x06005721 RID: 22305 RVA: 0x001A025D File Offset: 0x0019E65D
		public void Notify_MapRemoved(Map map)
		{
			if (this.lookTargets != null)
			{
				this.lookTargets.Notify_MapRemoved(map);
			}
		}

		// Token: 0x06005722 RID: 22306 RVA: 0x001A0278 File Offset: 0x0019E678
		protected virtual string PostProcessedLabel()
		{
			return this.label;
		}

		// Token: 0x06005723 RID: 22307 RVA: 0x001A0293 File Offset: 0x0019E693
		void IArchivable.OpenArchived()
		{
			this.OpenLetter();
		}

		// Token: 0x06005724 RID: 22308 RVA: 0x001A029C File Offset: 0x0019E69C
		public string GetUniqueLoadID()
		{
			return "Letter_" + this.ID;
		}

		// Token: 0x040039B8 RID: 14776
		public int ID;

		// Token: 0x040039B9 RID: 14777
		public LetterDef def;

		// Token: 0x040039BA RID: 14778
		public string label;

		// Token: 0x040039BB RID: 14779
		public LookTargets lookTargets;

		// Token: 0x040039BC RID: 14780
		public Faction relatedFaction;

		// Token: 0x040039BD RID: 14781
		public int arrivalTick;

		// Token: 0x040039BE RID: 14782
		public float arrivalTime;

		// Token: 0x040039BF RID: 14783
		public string debugInfo;

		// Token: 0x040039C0 RID: 14784
		public const float DrawWidth = 38f;

		// Token: 0x040039C1 RID: 14785
		public const float DrawHeight = 30f;

		// Token: 0x040039C2 RID: 14786
		private const float FallTime = 1f;

		// Token: 0x040039C3 RID: 14787
		private const float FallDistance = 200f;
	}
}
