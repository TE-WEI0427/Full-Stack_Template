import 'package:flutter/material.dart';

/// MaterialApp 的一些通用設定
///
/// ThemeData 設定集_1
///
/// -------------------------------------------------------------
/// 參數說明
///
/// colorScheme : 背景色(淺藍)，對應 ThemeData 的 colorScheme
///
/// useMaterial3 : 上一頁的按鈕元件，對應 ThemeData 的 useMaterial3
class ThemeDataConfig1 {
  static ColorScheme colorScheme =
      ColorScheme.fromSeed(seedColor: Colors.blueGrey);
  static bool useMaterial3 = true;
}
