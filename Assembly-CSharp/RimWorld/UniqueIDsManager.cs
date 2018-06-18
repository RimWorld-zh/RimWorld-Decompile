using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020005AE RID: 1454
	public class UniqueIDsManager : IExposable
	{
		// Token: 0x06001BBB RID: 7099 RVA: 0x000EF1F1 File Offset: 0x000ED5F1
		public UniqueIDsManager()
		{
			this.nextThingID = Rand.Range(0, 1000);
		}

		// Token: 0x06001BBC RID: 7100 RVA: 0x000EF20C File Offset: 0x000ED60C
		public int GetNextThingID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextThingID);
		}

		// Token: 0x06001BBD RID: 7101 RVA: 0x000EF22C File Offset: 0x000ED62C
		public int GetNextBillID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextBillID);
		}

		// Token: 0x06001BBE RID: 7102 RVA: 0x000EF24C File Offset: 0x000ED64C
		public int GetNextFactionID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextFactionID);
		}

		// Token: 0x06001BBF RID: 7103 RVA: 0x000EF26C File Offset: 0x000ED66C
		public int GetNextLordID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextLordID);
		}

		// Token: 0x06001BC0 RID: 7104 RVA: 0x000EF28C File Offset: 0x000ED68C
		public int GetNextTaleID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextTaleID);
		}

		// Token: 0x06001BC1 RID: 7105 RVA: 0x000EF2AC File Offset: 0x000ED6AC
		public int GetNextPassingShipID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextPassingShipID);
		}

		// Token: 0x06001BC2 RID: 7106 RVA: 0x000EF2CC File Offset: 0x000ED6CC
		public int GetNextWorldObjectID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextWorldObjectID);
		}

		// Token: 0x06001BC3 RID: 7107 RVA: 0x000EF2EC File Offset: 0x000ED6EC
		public int GetNextMapID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextMapID);
		}

		// Token: 0x06001BC4 RID: 7108 RVA: 0x000EF30C File Offset: 0x000ED70C
		public int GetNextAreaID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextAreaID);
		}

		// Token: 0x06001BC5 RID: 7109 RVA: 0x000EF32C File Offset: 0x000ED72C
		public int GetNextTransporterGroupID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextTransporterGroupID);
		}

		// Token: 0x06001BC6 RID: 7110 RVA: 0x000EF34C File Offset: 0x000ED74C
		public int GetNextAncientCryptosleepCasketGroupID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextAncientCryptosleepCasketGroupID);
		}

		// Token: 0x06001BC7 RID: 7111 RVA: 0x000EF36C File Offset: 0x000ED76C
		public int GetNextJobID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextJobID);
		}

		// Token: 0x06001BC8 RID: 7112 RVA: 0x000EF38C File Offset: 0x000ED78C
		public int GetNextSignalTagID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextSignalTagID);
		}

		// Token: 0x06001BC9 RID: 7113 RVA: 0x000EF3AC File Offset: 0x000ED7AC
		public int GetNextWorldFeatureID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextWorldFeatureID);
		}

		// Token: 0x06001BCA RID: 7114 RVA: 0x000EF3CC File Offset: 0x000ED7CC
		public int GetNextHediffID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextHediffID);
		}

		// Token: 0x06001BCB RID: 7115 RVA: 0x000EF3EC File Offset: 0x000ED7EC
		public int GetNextBattleID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextBattleID);
		}

		// Token: 0x06001BCC RID: 7116 RVA: 0x000EF40C File Offset: 0x000ED80C
		public int GetNextLogID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextLogID);
		}

		// Token: 0x06001BCD RID: 7117 RVA: 0x000EF42C File Offset: 0x000ED82C
		public int GetNextLetterID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextLetterID);
		}

		// Token: 0x06001BCE RID: 7118 RVA: 0x000EF44C File Offset: 0x000ED84C
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

		// Token: 0x06001BCF RID: 7119 RVA: 0x000EF4A8 File Offset: 0x000ED8A8
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

		// Token: 0x0400107D RID: 4221
		private int nextThingID;

		// Token: 0x0400107E RID: 4222
		private int nextBillID;

		// Token: 0x0400107F RID: 4223
		private int nextFactionID;

		// Token: 0x04001080 RID: 4224
		private int nextLordID;

		// Token: 0x04001081 RID: 4225
		private int nextTaleID;

		// Token: 0x04001082 RID: 4226
		private int nextPassingShipID;

		// Token: 0x04001083 RID: 4227
		private int nextWorldObjectID;

		// Token: 0x04001084 RID: 4228
		private int nextMapID;

		// Token: 0x04001085 RID: 4229
		private int nextAreaID;

		// Token: 0x04001086 RID: 4230
		private int nextTransporterGroupID;

		// Token: 0x04001087 RID: 4231
		private int nextAncientCryptosleepCasketGroupID;

		// Token: 0x04001088 RID: 4232
		private int nextJobID;

		// Token: 0x04001089 RID: 4233
		private int nextSignalTagID;

		// Token: 0x0400108A RID: 4234
		private int nextWorldFeatureID;

		// Token: 0x0400108B RID: 4235
		private int nextHediffID;

		// Token: 0x0400108C RID: 4236
		private int nextBattleID;

		// Token: 0x0400108D RID: 4237
		private int nextLogID;

		// Token: 0x0400108E RID: 4238
		private int nextLetterID;
	}
}
