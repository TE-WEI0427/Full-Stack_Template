import 'package:flutter/foundation.dart';

import 'package:device_info_plus/device_info_plus.dart';
import 'package:android_id/android_id.dart';

import 'package:sample_app/Plugins/plugin_device_info.dart';

Future<Map<String, String>> getMobileDeviceInfo() async {
  // 非 Android 或 iOS，傳回空資料
  if (defaultTargetPlatform != TargetPlatform.android ||
      defaultTargetPlatform != TargetPlatform.iOS) {
    return <String, String>{};
  }

  DeviceInfoPlugin deviceInfoPlugin = DeviceInfoPlugin();

  // 取得裝置資訊 from device_info_plus
  Map<String, dynamic> deviceData = await initPlatformState(deviceInfoPlugin);

  // 建立回傳資料
  Map<String, String> info = {
    "isPhysicalDevice": deviceData["isPhysicalDevice"], // 是否為實體裝置
    "model": "", // 型號
    "brand": "", // 品牌
    "OSVersion": "", // 作業系統版本
    "platform": "", // 平台
    "UUID": "",
  };

  if (defaultTargetPlatform == TargetPlatform.android) {
    info.update("model", (value) => deviceData["model"]);
    info.update("brand", (value) => deviceData["brand"]);
    info.update("OSVersion", (value) => deviceData["version.release"]);
    info.update("platform", (value) => "Android");
    const AndroidId()
        .getId()
        .then((value) => info.update("UUID", (value) => value));

    // info["model"] = deviceData["model"];
    // info["brand"] = deviceData["brand"];
    // info["OSVersion"] = deviceData["version.release"];
    // info["platform"] = "Android";

    // info["UUID"] = (await const AndroidId().getId())!;
  }

  if (defaultTargetPlatform == TargetPlatform.iOS) {
    info.update("model", (value) => deviceData["utsname.machine"]);
    info.update("brand", (value) => deviceData["model"]);
    info.update("OSVersion", (value) => deviceData["systemVersion"]);
    info.update("platform", (value) => deviceData["systemName"]);
    info.update("UUID", (value) => deviceData["identifierForVendor"]);

    // info["model"] = deviceData["utsname.machine"];
    // info["brand"] = deviceData["model"];
    // info["OSVersion"] = deviceData["systemVersion"];
    // info["platform"] = deviceData["systemName"];
    // info["UUID"] = deviceData["identifierForVendor"];
  }

  return info;
}
