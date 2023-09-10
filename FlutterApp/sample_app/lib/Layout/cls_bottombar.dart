import 'package:flutter/material.dart';

/// 主頁 BottomBar 樣式
///
/// 樣式_1 : NavigationBar
///
/// 掛在 Scaffold 的 bottomNavigationBar 參數上
class ClsBottomBar1 extends StatelessWidget {
  final int currentPageIndex;
  final Function(int) onPageChange;
  final Color? indicatorColor;
  final List<Widget> destinations;

  /// 建構子
  ///
  /// ------------------------------------
  /// 參數說明
  ///
  /// currentPageIndex : 當前頁的索引值
  ///
  /// onPageChange : 頁面切換時調用的方法
  ///
  /// indicatorColor : 按鈕按下後的顏色
  ///
  /// destinations : 按鈕列
  const ClsBottomBar1(
      {super.key,
      required this.currentPageIndex,
      required this.onPageChange,
      required this.indicatorColor,
      required this.destinations});

  @override
  Widget build(BuildContext context) {
    return NavigationBar(
      selectedIndex: currentPageIndex,
      onDestinationSelected: onPageChange,
      indicatorColor: indicatorColor,
      destinations: destinations,
    );
  }
}
