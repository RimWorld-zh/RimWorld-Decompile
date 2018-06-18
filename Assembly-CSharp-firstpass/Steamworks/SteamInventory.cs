using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200013F RID: 319
	public static class SteamInventory
	{
		// Token: 0x060005A7 RID: 1447 RVA: 0x00007DA8 File Offset: 0x00005FA8
		public static EResult GetResultStatus(SteamInventoryResult_t resultHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_GetResultStatus(resultHandle);
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x00007DC8 File Offset: 0x00005FC8
		public static bool GetResultItems(SteamInventoryResult_t resultHandle, SteamItemDetails_t[] pOutItemsArray, ref uint punOutItemsArraySize)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_GetResultItems(resultHandle, pOutItemsArray, ref punOutItemsArraySize);
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x00007DEC File Offset: 0x00005FEC
		public static uint GetResultTimestamp(SteamInventoryResult_t resultHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_GetResultTimestamp(resultHandle);
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x00007E0C File Offset: 0x0000600C
		public static bool CheckResultSteamID(SteamInventoryResult_t resultHandle, CSteamID steamIDExpected)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_CheckResultSteamID(resultHandle, steamIDExpected);
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x00007E2D File Offset: 0x0000602D
		public static void DestroyResult(SteamInventoryResult_t resultHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamInventory_DestroyResult(resultHandle);
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x00007E3C File Offset: 0x0000603C
		public static bool GetAllItems(out SteamInventoryResult_t pResultHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_GetAllItems(out pResultHandle);
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x00007E5C File Offset: 0x0000605C
		public static bool GetItemsByID(out SteamInventoryResult_t pResultHandle, SteamItemInstanceID_t[] pInstanceIDs, uint unCountInstanceIDs)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_GetItemsByID(out pResultHandle, pInstanceIDs, unCountInstanceIDs);
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x00007E80 File Offset: 0x00006080
		public static bool SerializeResult(SteamInventoryResult_t resultHandle, byte[] pOutBuffer, out uint punOutBufferSize)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_SerializeResult(resultHandle, pOutBuffer, out punOutBufferSize);
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x00007EA4 File Offset: 0x000060A4
		public static bool DeserializeResult(out SteamInventoryResult_t pOutResultHandle, byte[] pBuffer, uint unBufferSize, bool bRESERVED_MUST_BE_FALSE = false)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_DeserializeResult(out pOutResultHandle, pBuffer, unBufferSize, bRESERVED_MUST_BE_FALSE);
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x00007EC8 File Offset: 0x000060C8
		public static bool GenerateItems(out SteamInventoryResult_t pResultHandle, SteamItemDef_t[] pArrayItemDefs, uint[] punArrayQuantity, uint unArrayLength)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_GenerateItems(out pResultHandle, pArrayItemDefs, punArrayQuantity, unArrayLength);
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x00007EEC File Offset: 0x000060EC
		public static bool GrantPromoItems(out SteamInventoryResult_t pResultHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_GrantPromoItems(out pResultHandle);
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x00007F0C File Offset: 0x0000610C
		public static bool AddPromoItem(out SteamInventoryResult_t pResultHandle, SteamItemDef_t itemDef)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_AddPromoItem(out pResultHandle, itemDef);
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x00007F30 File Offset: 0x00006130
		public static bool AddPromoItems(out SteamInventoryResult_t pResultHandle, SteamItemDef_t[] pArrayItemDefs, uint unArrayLength)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_AddPromoItems(out pResultHandle, pArrayItemDefs, unArrayLength);
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x00007F54 File Offset: 0x00006154
		public static bool ConsumeItem(out SteamInventoryResult_t pResultHandle, SteamItemInstanceID_t itemConsume, uint unQuantity)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_ConsumeItem(out pResultHandle, itemConsume, unQuantity);
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x00007F78 File Offset: 0x00006178
		public static bool ExchangeItems(out SteamInventoryResult_t pResultHandle, SteamItemDef_t[] pArrayGenerate, uint[] punArrayGenerateQuantity, uint unArrayGenerateLength, SteamItemInstanceID_t[] pArrayDestroy, uint[] punArrayDestroyQuantity, uint unArrayDestroyLength)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_ExchangeItems(out pResultHandle, pArrayGenerate, punArrayGenerateQuantity, unArrayGenerateLength, pArrayDestroy, punArrayDestroyQuantity, unArrayDestroyLength);
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x00007FA4 File Offset: 0x000061A4
		public static bool TransferItemQuantity(out SteamInventoryResult_t pResultHandle, SteamItemInstanceID_t itemIdSource, uint unQuantity, SteamItemInstanceID_t itemIdDest)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_TransferItemQuantity(out pResultHandle, itemIdSource, unQuantity, itemIdDest);
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x00007FC7 File Offset: 0x000061C7
		public static void SendItemDropHeartbeat()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamInventory_SendItemDropHeartbeat();
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x00007FD4 File Offset: 0x000061D4
		public static bool TriggerItemDrop(out SteamInventoryResult_t pResultHandle, SteamItemDef_t dropListDefinition)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_TriggerItemDrop(out pResultHandle, dropListDefinition);
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x00007FF8 File Offset: 0x000061F8
		public static bool TradeItems(out SteamInventoryResult_t pResultHandle, CSteamID steamIDTradePartner, SteamItemInstanceID_t[] pArrayGive, uint[] pArrayGiveQuantity, uint nArrayGiveLength, SteamItemInstanceID_t[] pArrayGet, uint[] pArrayGetQuantity, uint nArrayGetLength)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_TradeItems(out pResultHandle, steamIDTradePartner, pArrayGive, pArrayGiveQuantity, nArrayGiveLength, pArrayGet, pArrayGetQuantity, nArrayGetLength);
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00008024 File Offset: 0x00006224
		public static bool LoadItemDefinitions()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_LoadItemDefinitions();
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x00008044 File Offset: 0x00006244
		public static bool GetItemDefinitionIDs(SteamItemDef_t[] pItemDefIDs, out uint punItemDefIDsArraySize)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_GetItemDefinitionIDs(pItemDefIDs, out punItemDefIDsArraySize);
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x00008068 File Offset: 0x00006268
		public static bool GetItemDefinitionProperty(SteamItemDef_t iDefinition, string pchPropertyName, out string pchValueBuffer, ref uint punValueBufferSize)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)punValueBufferSize);
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchPropertyName))
			{
				bool flag = NativeMethods.ISteamInventory_GetItemDefinitionProperty(iDefinition, utf8StringHandle, intPtr, ref punValueBufferSize);
				pchValueBuffer = ((!flag) ? null : InteropHelp.PtrToStringUTF8(intPtr));
				Marshal.FreeHGlobal(intPtr);
				result = flag;
			}
			return result;
		}
	}
}
