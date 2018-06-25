using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020005AC RID: 1452
	public class UniqueIDsManager : IExposable
	{
		// Token: 0x0400107A RID: 4218
		private int nextThingID;

		// Token: 0x0400107B RID: 4219
		private int nextBillID;

		// Token: 0x0400107C RID: 4220
		private int nextFactionID;

		// Token: 0x0400107D RID: 4221
		private int nextLordID;

		// Token: 0x0400107E RID: 4222
		private int nextTaleID;

		// Token: 0x0400107F RID: 4223
		private int nextPassingShipID;

		// Token: 0x04001080 RID: 4224
		private int nextWorldObjectID;

		// Token: 0x04001081 RID: 4225
		private int nextMapID;

		// Token: 0x04001082 RID: 4226
		private int nextAreaID;

		// Token: 0x04001083 RID: 4227
		private int nextTransporterGroupID;

		// Token: 0x04001084 RID: 4228
		private int nextAncientCryptosleepCasketGroupID;

		// Token: 0x04001085 RID: 4229
		private int nextJobID;

		// Token: 0x04001086 RID: 4230
		private int nextSignalTagID;

		// Token: 0x04001087 RID: 4231
		private int nextWorldFeatureID;

		// Token: 0x04001088 RID: 4232
		private int nextHediffID;

		// Token: 0x04001089 RID: 4233
		private int nextBattleID;

		// Token: 0x0400108A RID: 4234
		private int nextLogID;

		// Token: 0x0400108B RID: 4235
		private int nextLetterID;

		// Token: 0x06001BB6 RID: 7094 RVA: 0x000EF395 File Offset: 0x000ED795
		public UniqueIDsManager()
		{
			this.nextThingID = Rand.Range(0, 1000);
		}

		// Token: 0x06001BB7 RID: 7095 RVA: 0x000EF3B0 File Offset: 0x000ED7B0
		public int GetNextThingID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextThingID);
		}

		// Token: 0x06001BB8 RID: 7096 RVA: 0x000EF3D0 File Offset: 0x000ED7D0
		public int GetNextBillID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextBillID);
		}

		// Token: 0x06001BB9 RID: 7097 RVA: 0x000EF3F0 File Offset: 0x000ED7F0
		public int GetNextFactionID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextFactionID);
		}

		// Token: 0x06001BBA RID: 7098 RVA: 0x000EF410 File Offset: 0x000ED810
		public int GetNextLordID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextLordID);
		}

		// Token: 0x06001BBB RID: 7099 RVA: 0x000EF430 File Offset: 0x000ED830
		public int GetNextTaleID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextTaleID);
		}

		// Token: 0x06001BBC RID: 7100 RVA: 0x000EF450 File Offset: 0x000ED850
		public int GetNextPassingShipID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextPassingShipID);
		}

		// Token: 0x06001BBD RID: 7101 RVA: 0x000EF470 File Offset: 0x000ED870
		public int GetNextWorldObjectID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextWorldObjectID);
		}

		// Token: 0x06001BBE RID: 7102 RVA: 0x000EF490 File Offset: 0x000ED890
		public int GetNextMapID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextMapID);
		}

		// Token: 0x06001BBF RID: 7103 RVA: 0x000EF4B0 File Offset: 0x000ED8B0
		public int GetNextAreaID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextAreaID);
		}

		// Token: 0x06001BC0 RID: 7104 RVA: 0x000EF4D0 File Offset: 0x000ED8D0
		public int GetNextTransporterGroupID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextTransporterGroupID);
		}

		// Token: 0x06001BC1 RID: 7105 RVA: 0x000EF4F0 File Offset: 0x000ED8F0
		public int GetNextAncientCryptosleepCasketGroupID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextAncientCryptosleepCasketGroupID);
		}

		// Token: 0x06001BC2 RID: 7106 RVA: 0x000EF510 File Offset: 0x000ED910
		public int GetNextJobID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextJobID);
		}

		// Token: 0x06001BC3 RID: 7107 RVA: 0x000EF530 File Offset: 0x000ED930
		public int GetNextSignalTagID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextSignalTagID);
		}

		// Token: 0x06001BC4 RID: 7108 RVA: 0x000EF550 File Offset: 0x000ED950
		public int GetNextWorldFeatureID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextWorldFeatureID);
		}

		// Token: 0x06001BC5 RID: 7109 RVA: 0x000EF570 File Offset: 0x000ED970
		public int GetNextHediffID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextHediffID);
		}

		// Token: 0x06001BC6 RID: 7110 RVA: 0x000EF590 File Offset: 0x000ED990
		public int GetNextBattleID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextBattleID);
		}

		// Token: 0x06001BC7 RID: 7111 RVA: 0x000EF5B0 File Offset: 0x000ED9B0
		public int GetNextLogID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextLogID);
		}

		// Token: 0x06001BC8 RID: 7112 RVA: 0x000EF5D0 File Offset: 0x000ED9D0
		public int GetNextLetterID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextLetterID);
		}

		// Token: 0x06001BC9 RID: 7113 RVA: 0x000EF5F0 File Offset: 0x000ED9F0
		private static int GetNextID(ref int nextID)
		{
			if (Scribe.mode == LoadSaveMode.Saving || Scribe.mode == LoadSaveMode.LoadingVars)
			{
				Log.Warning("Getting next unique ID during saving or loading. This may cause bugs.", false);
			}
			int result = nextID;
			nextID++;
			if (nextID == 2147483647)
			{
				Log.Warning("Next ID is at max value. Resetting to 0. This may cause bugs.", false);
				nextID = 0;
			}
			return result;
		}

		// Token: 0x06001BCA RID: 7114 RVA: 0x000EF64C File Offset: 0x000EDA4C
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.nextThingID, "nextThingID", 0, false);
			Scribe_Values.Look<int>(ref this.nextBillID, "nextBillID", 0, false);
			Scribe_Values.Look<int>(ref this.nextFactionID, "nextFactionID", 0, false);
			Scribe_Values.Look<int>(ref this.nextLordID, "nextLordID", 0, false);
			Scribe_Values.Look<int>(ref this.nextTaleID, "nextTaleID", 0, false);
			Scribe_Values.Look<int>(ref this.nextPassingShipID, "nextPassingShipID", 0, false);
			Scribe_Values.Look<int>(ref this.nextWorldObjectID, "nextWorldObjectID", 0, false);
			Scribe_Values.Look<int>(ref this.nextMapID, "nextMapID", 0, false);
			Scribe_Values.Look<int>(ref this.nextAreaID, "nextAreaID", 0, false);
			Scribe_Values.Look<int>(ref this.nextTransporterGroupID, "nextTransporterGroupID", 0, false);
			Scribe_Values.Look<int>(ref this.nextAncientCryptosleepCasketGroupID, "nextAncientCryptosleepCasketGroupID", 0, false);
			Scribe_Values.Look<int>(ref this.nextJobID, "nextJobID", 0, false);
			Scribe_Values.Look<int>(ref this.nextSignalTagID, "nextSignalTagID", 0, false);
			Scribe_Values.Look<int>(ref this.nextWorldFeatureID, "nextWorldFeatureID", 0, false);
			Scribe_Values.Look<int>(ref this.nextHediffID, "nextHediffID", 0, false);
			Scribe_Values.Look<int>(ref this.nextBattleID, "nextBattleID", 0, false);
			Scribe_Values.Look<int>(ref this.nextLogID, "nextLogID", 0, false);
			Scribe_Values.Look<int>(ref this.nextLetterID, "nextLetterID", 0, false);
		}
	}
}
