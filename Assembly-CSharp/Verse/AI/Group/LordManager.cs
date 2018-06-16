using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x020009EF RID: 2543
	public sealed class LordManager : IExposable
	{
		// Token: 0x06003914 RID: 14612 RVA: 0x001E6364 File Offset: 0x001E4764
		public LordManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06003915 RID: 14613 RVA: 0x001E6380 File Offset: 0x001E4780
		public void ExposeData()
		{
			Scribe_Collections.Look<Lord>(ref this.lords, "lords", LookMode.Deep, new object[0]);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				for (int i = 0; i < this.lords.Count; i++)
				{
					this.lords[i].lordManager = this;
				}
			}
		}

		// Token: 0x06003916 RID: 14614 RVA: 0x001E63E4 File Offset: 0x001E47E4
		public void LordManagerTick()
		{
			for (int i = 0; i < this.lords.Count; i++)
			{
				this.lords[i].LordTick();
			}
			for (int j = this.lords.Count - 1; j >= 0; j--)
			{
				LordToil curLordToil = this.lords[j].CurLordToil;
				if (curLordToil.ShouldFail)
				{
					this.RemoveLord(this.lords[j]);
				}
			}
		}

		// Token: 0x06003917 RID: 14615 RVA: 0x001E6470 File Offset: 0x001E4870
		public void LordManagerUpdate()
		{
			if (DebugViewSettings.drawLords)
			{
				for (int i = 0; i < this.lords.Count; i++)
				{
					this.lords[i].DebugDraw();
				}
			}
		}

		// Token: 0x06003918 RID: 14616 RVA: 0x001E64BC File Offset: 0x001E48BC
		public void LordManagerOnGUI()
		{
			if (DebugViewSettings.drawLords)
			{
				for (int i = 0; i < this.lords.Count; i++)
				{
					this.lords[i].DebugOnGUI();
				}
			}
			if (DebugViewSettings.drawDuties)
			{
				Text.Anchor = TextAnchor.MiddleCenter;
				Text.Font = GameFont.Tiny;
				foreach (Pawn pawn in this.map.mapPawns.AllPawns)
				{
					if (pawn.Spawned)
					{
						string text = "";
						if (!pawn.Dead && pawn.mindState.duty != null)
						{
							text = pawn.mindState.duty.ToString();
						}
						if (pawn.InMentalState)
						{
							text = text + "\nMentalState=" + pawn.MentalState.ToString();
						}
						Vector2 vector = pawn.DrawPos.MapToUIPosition();
						Widgets.Label(new Rect(vector.x - 100f, vector.y - 100f, 200f, 200f), text);
					}
				}
				Text.Anchor = TextAnchor.UpperLeft;
			}
		}

		// Token: 0x06003919 RID: 14617 RVA: 0x001E6618 File Offset: 0x001E4A18
		public void AddLord(Lord newLord)
		{
			this.lords.Add(newLord);
			newLord.lordManager = this;
		}

		// Token: 0x0600391A RID: 14618 RVA: 0x001E662E File Offset: 0x001E4A2E
		public void RemoveLord(Lord oldLord)
		{
			this.lords.Remove(oldLord);
			oldLord.Cleanup();
		}

		// Token: 0x0600391B RID: 14619 RVA: 0x001E6644 File Offset: 0x001E4A44
		public Lord LordOf(Pawn p)
		{
			for (int i = 0; i < this.lords.Count; i++)
			{
				Lord lord = this.lords[i];
				for (int j = 0; j < lord.ownedPawns.Count; j++)
				{
					if (lord.ownedPawns[j] == p)
					{
						return lord;
					}
				}
			}
			return null;
		}

		// Token: 0x0600391C RID: 14620 RVA: 0x001E66BC File Offset: 0x001E4ABC
		public void LogLords()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("======= Lords =======");
			stringBuilder.AppendLine("Count: " + this.lords.Count);
			for (int i = 0; i < this.lords.Count; i++)
			{
				Lord lord = this.lords[i];
				stringBuilder.AppendLine();
				stringBuilder.Append("#" + (i + 1) + ": ");
				if (lord.LordJob == null)
				{
					stringBuilder.AppendLine("no-job");
				}
				else
				{
					stringBuilder.AppendLine(lord.LordJob.GetType().Name);
				}
				stringBuilder.Append("Current toil: ");
				if (lord.CurLordToil == null)
				{
					stringBuilder.AppendLine("null");
				}
				else
				{
					stringBuilder.AppendLine(lord.CurLordToil.GetType().Name);
				}
				stringBuilder.AppendLine("Members (count: " + lord.ownedPawns.Count + "):");
				for (int j = 0; j < lord.ownedPawns.Count; j++)
				{
					stringBuilder.AppendLine(string.Concat(new object[]
					{
						"  ",
						lord.ownedPawns[j].LabelShort,
						" (",
						lord.ownedPawns[j].Faction,
						")"
					}));
				}
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x0400246D RID: 9325
		public Map map;

		// Token: 0x0400246E RID: 9326
		public List<Lord> lords = new List<Lord>();
	}
}
