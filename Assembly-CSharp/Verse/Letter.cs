using System;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000E74 RID: 3700
	public abstract class Letter : IArchivable, ILoadReferenceable, IExposable
	{
		// Token: 0x040039C6 RID: 14790
		public int ID;

		// Token: 0x040039C7 RID: 14791
		public LetterDef def;

		// Token: 0x040039C8 RID: 14792
		public string label;

		// Token: 0x040039C9 RID: 14793
		public LookTargets lookTargets;

		// Token: 0x040039CA RID: 14794
		public Faction relatedFaction;

		// Token: 0x040039CB RID: 14795
		public int arrivalTick;

		// Token: 0x040039CC RID: 14796
		public float arrivalTime;

		// Token: 0x040039CD RID: 14797
		public string debugInfo;

		// Token: 0x040039CE RID: 14798
		public const float DrawWidth = 38f;

		// Token: 0x040039CF RID: 14799
		public const float DrawHeight = 30f;

		// Token: 0x040039D0 RID: 14800
		private const float FallTime = 1f;

		// Token: 0x040039D1 RID: 14801
		private const float FallDistance = 200f;

		// Token: 0x17000DBD RID: 3517
		// (get) Token: 0x0600572E RID: 22318 RVA: 0x0019FDE8 File Offset: 0x0019E1E8
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

		// Token: 0x17000DBE RID: 3518
		// (get) Token: 0x0600572F RID: 22319 RVA: 0x0019FEF8 File Offset: 0x0019E2F8
		public bool ArchivedOnly
		{
			get
			{
				return !Find.LetterStack.LettersListForReading.Contains(this);
			}
		}

		// Token: 0x17000DBF RID: 3519
		// (get) Token: 0x06005730 RID: 22320 RVA: 0x0019FF20 File Offset: 0x0019E320
		public IThingHolder ParentHolder
		{
			get
			{
				return Find.World;
			}
		}

		// Token: 0x17000DB6 RID: 3510
		// (get) Token: 0x06005731 RID: 22321 RVA: 0x0019FF3C File Offset: 0x0019E33C
		Texture IArchivable.ArchivedIcon
		{
			get
			{
				return this.def.Icon;
			}
		}

		// Token: 0x17000DB7 RID: 3511
		// (get) Token: 0x06005732 RID: 22322 RVA: 0x0019FF5C File Offset: 0x0019E35C
		Color IArchivable.ArchivedIconColor
		{
			get
			{
				return this.def.color;
			}
		}

		// Token: 0x17000DB8 RID: 3512
		// (get) Token: 0x06005733 RID: 22323 RVA: 0x0019FF7C File Offset: 0x0019E37C
		string IArchivable.ArchivedLabel
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x17000DB9 RID: 3513
		// (get) Token: 0x06005734 RID: 22324 RVA: 0x0019FF98 File Offset: 0x0019E398
		string IArchivable.ArchivedTooltip
		{
			get
			{
				return this.GetMouseoverText();
			}
		}

		// Token: 0x17000DBA RID: 3514
		// (get) Token: 0x06005735 RID: 22325 RVA: 0x0019FFB4 File Offset: 0x0019E3B4
		int IArchivable.CreatedTicksGame
		{
			get
			{
				return this.arrivalTick;
			}
		}

		// Token: 0x17000DBB RID: 3515
		// (get) Token: 0x06005736 RID: 22326 RVA: 0x0019FFD0 File Offset: 0x0019E3D0
		bool IArchivable.CanCullArchivedNow
		{
			get
			{
				return !Find.LetterStack.LettersListForReading.Contains(this);
			}
		}

		// Token: 0x17000DBC RID: 3516
		// (get) Token: 0x06005737 RID: 22327 RVA: 0x0019FFF8 File Offset: 0x0019E3F8
		LookTargets IArchivable.LookTargets
		{
			get
			{
				return this.lookTargets;
			}
		}

		// Token: 0x06005738 RID: 22328 RVA: 0x001A0014 File Offset: 0x0019E414
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ID, "ID", 0, false);
			Scribe_Defs.Look<LetterDef>(ref this.def, "def");
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", new object[0]);
			Scribe_References.Look<Faction>(ref this.relatedFaction, "relatedFaction", false);
			Scribe_Values.Look<int>(ref this.arrivalTick, "arrivalTick", 0, false);
		}

		// Token: 0x06005739 RID: 22329 RVA: 0x001A0090 File Offset: 0x0019E490
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

		// Token: 0x0600573A RID: 22330 RVA: 0x001A0404 File Offset: 0x0019E804
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

		// Token: 0x0600573B RID: 22331
		protected abstract string GetMouseoverText();

		// Token: 0x0600573C RID: 22332
		public abstract void OpenLetter();

		// Token: 0x0600573D RID: 22333 RVA: 0x001A04FF File Offset: 0x0019E8FF
		public virtual void Received()
		{
		}

		// Token: 0x0600573E RID: 22334 RVA: 0x001A0502 File Offset: 0x0019E902
		public virtual void Removed()
		{
		}

		// Token: 0x0600573F RID: 22335 RVA: 0x001A0505 File Offset: 0x0019E905
		public void Notify_MapRemoved(Map map)
		{
			if (this.lookTargets != null)
			{
				this.lookTargets.Notify_MapRemoved(map);
			}
		}

		// Token: 0x06005740 RID: 22336 RVA: 0x001A0520 File Offset: 0x0019E920
		protected virtual string PostProcessedLabel()
		{
			return this.label;
		}

		// Token: 0x06005741 RID: 22337 RVA: 0x001A053B File Offset: 0x0019E93B
		void IArchivable.OpenArchived()
		{
			this.OpenLetter();
		}

		// Token: 0x06005742 RID: 22338 RVA: 0x001A0544 File Offset: 0x0019E944
		public string GetUniqueLoadID()
		{
			return "Letter_" + this.ID;
		}
	}
}
