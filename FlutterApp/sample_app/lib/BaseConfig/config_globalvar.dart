import 'package:flutter/material.dart';

/// 全域參數
class GlobalVar {
  /// 獲取當前的 BuildContext
  ///
  /// 在 main 的 MaterialApp 設定就可以了，不必於每個頁面的 MaterialApp 都設定
  static final GlobalKey<NavigatorState> gNavState = GlobalKey();

  /// 獲取當前的 Scaffold
  static final GlobalKey<ScaffoldState> gScafKey = GlobalKey();
}
