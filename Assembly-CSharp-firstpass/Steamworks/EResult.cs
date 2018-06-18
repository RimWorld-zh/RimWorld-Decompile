using System;

namespace Steamworks
{
	// Token: 0x02000113 RID: 275
	public enum EResult
	{
		// Token: 0x040004B8 RID: 1208
		k_EResultOK = 1,
		// Token: 0x040004B9 RID: 1209
		k_EResultFail,
		// Token: 0x040004BA RID: 1210
		k_EResultNoConnection,
		// Token: 0x040004BB RID: 1211
		k_EResultInvalidPassword = 5,
		// Token: 0x040004BC RID: 1212
		k_EResultLoggedInElsewhere,
		// Token: 0x040004BD RID: 1213
		k_EResultInvalidProtocolVer,
		// Token: 0x040004BE RID: 1214
		k_EResultInvalidParam,
		// Token: 0x040004BF RID: 1215
		k_EResultFileNotFound,
		// Token: 0x040004C0 RID: 1216
		k_EResultBusy,
		// Token: 0x040004C1 RID: 1217
		k_EResultInvalidState,
		// Token: 0x040004C2 RID: 1218
		k_EResultInvalidName,
		// Token: 0x040004C3 RID: 1219
		k_EResultInvalidEmail,
		// Token: 0x040004C4 RID: 1220
		k_EResultDuplicateName,
		// Token: 0x040004C5 RID: 1221
		k_EResultAccessDenied,
		// Token: 0x040004C6 RID: 1222
		k_EResultTimeout,
		// Token: 0x040004C7 RID: 1223
		k_EResultBanned,
		// Token: 0x040004C8 RID: 1224
		k_EResultAccountNotFound,
		// Token: 0x040004C9 RID: 1225
		k_EResultInvalidSteamID,
		// Token: 0x040004CA RID: 1226
		k_EResultServiceUnavailable,
		// Token: 0x040004CB RID: 1227
		k_EResultNotLoggedOn,
		// Token: 0x040004CC RID: 1228
		k_EResultPending,
		// Token: 0x040004CD RID: 1229
		k_EResultEncryptionFailure,
		// Token: 0x040004CE RID: 1230
		k_EResultInsufficientPrivilege,
		// Token: 0x040004CF RID: 1231
		k_EResultLimitExceeded,
		// Token: 0x040004D0 RID: 1232
		k_EResultRevoked,
		// Token: 0x040004D1 RID: 1233
		k_EResultExpired,
		// Token: 0x040004D2 RID: 1234
		k_EResultAlreadyRedeemed,
		// Token: 0x040004D3 RID: 1235
		k_EResultDuplicateRequest,
		// Token: 0x040004D4 RID: 1236
		k_EResultAlreadyOwned,
		// Token: 0x040004D5 RID: 1237
		k_EResultIPNotFound,
		// Token: 0x040004D6 RID: 1238
		k_EResultPersistFailed,
		// Token: 0x040004D7 RID: 1239
		k_EResultLockingFailed,
		// Token: 0x040004D8 RID: 1240
		k_EResultLogonSessionReplaced,
		// Token: 0x040004D9 RID: 1241
		k_EResultConnectFailed,
		// Token: 0x040004DA RID: 1242
		k_EResultHandshakeFailed,
		// Token: 0x040004DB RID: 1243
		k_EResultIOFailure,
		// Token: 0x040004DC RID: 1244
		k_EResultRemoteDisconnect,
		// Token: 0x040004DD RID: 1245
		k_EResultShoppingCartNotFound,
		// Token: 0x040004DE RID: 1246
		k_EResultBlocked,
		// Token: 0x040004DF RID: 1247
		k_EResultIgnored,
		// Token: 0x040004E0 RID: 1248
		k_EResultNoMatch,
		// Token: 0x040004E1 RID: 1249
		k_EResultAccountDisabled,
		// Token: 0x040004E2 RID: 1250
		k_EResultServiceReadOnly,
		// Token: 0x040004E3 RID: 1251
		k_EResultAccountNotFeatured,
		// Token: 0x040004E4 RID: 1252
		k_EResultAdministratorOK,
		// Token: 0x040004E5 RID: 1253
		k_EResultContentVersion,
		// Token: 0x040004E6 RID: 1254
		k_EResultTryAnotherCM,
		// Token: 0x040004E7 RID: 1255
		k_EResultPasswordRequiredToKickSession,
		// Token: 0x040004E8 RID: 1256
		k_EResultAlreadyLoggedInElsewhere,
		// Token: 0x040004E9 RID: 1257
		k_EResultSuspended,
		// Token: 0x040004EA RID: 1258
		k_EResultCancelled,
		// Token: 0x040004EB RID: 1259
		k_EResultDataCorruption,
		// Token: 0x040004EC RID: 1260
		k_EResultDiskFull,
		// Token: 0x040004ED RID: 1261
		k_EResultRemoteCallFailed,
		// Token: 0x040004EE RID: 1262
		k_EResultPasswordUnset,
		// Token: 0x040004EF RID: 1263
		k_EResultExternalAccountUnlinked,
		// Token: 0x040004F0 RID: 1264
		k_EResultPSNTicketInvalid,
		// Token: 0x040004F1 RID: 1265
		k_EResultExternalAccountAlreadyLinked,
		// Token: 0x040004F2 RID: 1266
		k_EResultRemoteFileConflict,
		// Token: 0x040004F3 RID: 1267
		k_EResultIllegalPassword,
		// Token: 0x040004F4 RID: 1268
		k_EResultSameAsPreviousValue,
		// Token: 0x040004F5 RID: 1269
		k_EResultAccountLogonDenied,
		// Token: 0x040004F6 RID: 1270
		k_EResultCannotUseOldPassword,
		// Token: 0x040004F7 RID: 1271
		k_EResultInvalidLoginAuthCode,
		// Token: 0x040004F8 RID: 1272
		k_EResultAccountLogonDeniedNoMail,
		// Token: 0x040004F9 RID: 1273
		k_EResultHardwareNotCapableOfIPT,
		// Token: 0x040004FA RID: 1274
		k_EResultIPTInitError,
		// Token: 0x040004FB RID: 1275
		k_EResultParentalControlRestricted,
		// Token: 0x040004FC RID: 1276
		k_EResultFacebookQueryError,
		// Token: 0x040004FD RID: 1277
		k_EResultExpiredLoginAuthCode,
		// Token: 0x040004FE RID: 1278
		k_EResultIPLoginRestrictionFailed,
		// Token: 0x040004FF RID: 1279
		k_EResultAccountLockedDown,
		// Token: 0x04000500 RID: 1280
		k_EResultAccountLogonDeniedVerifiedEmailRequired,
		// Token: 0x04000501 RID: 1281
		k_EResultNoMatchingURL,
		// Token: 0x04000502 RID: 1282
		k_EResultBadResponse,
		// Token: 0x04000503 RID: 1283
		k_EResultRequirePasswordReEntry,
		// Token: 0x04000504 RID: 1284
		k_EResultValueOutOfRange,
		// Token: 0x04000505 RID: 1285
		k_EResultUnexpectedError,
		// Token: 0x04000506 RID: 1286
		k_EResultDisabled,
		// Token: 0x04000507 RID: 1287
		k_EResultInvalidCEGSubmission,
		// Token: 0x04000508 RID: 1288
		k_EResultRestrictedDevice,
		// Token: 0x04000509 RID: 1289
		k_EResultRegionLocked,
		// Token: 0x0400050A RID: 1290
		k_EResultRateLimitExceeded,
		// Token: 0x0400050B RID: 1291
		k_EResultAccountLoginDeniedNeedTwoFactor,
		// Token: 0x0400050C RID: 1292
		k_EResultItemDeleted,
		// Token: 0x0400050D RID: 1293
		k_EResultAccountLoginDeniedThrottle,
		// Token: 0x0400050E RID: 1294
		k_EResultTwoFactorCodeMismatch,
		// Token: 0x0400050F RID: 1295
		k_EResultTwoFactorActivationCodeMismatch,
		// Token: 0x04000510 RID: 1296
		k_EResultAccountAssociatedToMultiplePartners,
		// Token: 0x04000511 RID: 1297
		k_EResultNotModified,
		// Token: 0x04000512 RID: 1298
		k_EResultNoMobileDevice,
		// Token: 0x04000513 RID: 1299
		k_EResultTimeNotSynced,
		// Token: 0x04000514 RID: 1300
		k_EResultSmsCodeFailed,
		// Token: 0x04000515 RID: 1301
		k_EResultAccountLimitExceeded,
		// Token: 0x04000516 RID: 1302
		k_EResultAccountActivityLimitExceeded,
		// Token: 0x04000517 RID: 1303
		k_EResultPhoneActivityLimitExceeded,
		// Token: 0x04000518 RID: 1304
		k_EResultRefundToWallet,
		// Token: 0x04000519 RID: 1305
		k_EResultEmailSendFailure,
		// Token: 0x0400051A RID: 1306
		k_EResultNotSettled
	}
}
