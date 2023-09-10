import 'package:flutter/material.dart';

/// AppBar 的一些通用設定
///
/// 設定集_1
///
/// ------------------------------------------------------------
/// 參數說明
///
/// backgroundColor : 背景色(淺藍)
///
/// wBackButton : 上一頁的按鈕元件，掛在 AppBar 的 leading 參數 上
class AppBarConfig1 {
  static backgroundColor(BuildContext context) {
    return Theme.of(context).colorScheme.inversePrimary;
  }

  /// 回上一頁
  static Widget wBackButton(BuildContext context) {
    return IconButton(
      icon: const Icon(Icons.arrow_back, color: Colors.black),
      onPressed: () => Navigator.of(context).pop(),
    );
  }
}
