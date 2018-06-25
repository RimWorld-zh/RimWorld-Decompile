using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x020009ED RID: 2541
	public sealed class LordManager : IExposable
	{
		// Token: 0x04002469 RID: 9321
		public Map map;

		// Token: 0x0400246A RID: 9322
		public List<Lord> lords = new List<Lord>();

		// Token: 0x06003914 RID: 14612 RVA: 0x001E67A4 File Offset: 0x001E4BA4
		public LordManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06003915 RID: 14613 RVA: 0x001E67C0 File Offset: 0x001E4BC0
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

		// Token: 0x06003916 RID: 14614 RVA: 0x001E6824 File Offset: 0x001E4C24
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

		// Token: 0x06003917 RID: 14615 RVA: 0x001E68B0 File Offset: 0x001E4CB0
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

		// Token: 0x06003918 RID: 14616 RVA: 0x001E68FC File Offset: 0x001E4CFC
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

		// Token: 0x06003919 RID: 14617 RVA: 0x001E6A58 File Offset: 0x001E4E58
		public void AddLord(Lord newLord)
		{
			this.lords.Add(newLord);
			newLord.lordManager = this;
		}

		// Token: 0x0600391A RID: 14618 RVA: 0x001E6A6E File Offset: 0x001E4E6E
		public void RemoveLord(Lord oldLord)
		{
			this.lords.Remove(oldLord);
			oldLord.Cleanup();
		}

		// Token: 0x0600391B RID: 14619 RVA: 0x001E6A84 File Offset: 0x001E4E84
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

		// Token: 0x0600391C RID: 14620 RVA: 0x001E6AFC File Offset: 0x001E4EFC
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
	}
}
