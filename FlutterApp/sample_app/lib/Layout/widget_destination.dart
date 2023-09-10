import 'package:flutter/material.dart';

/// 按鈕列
///
/// NavigationDestination List
List<Widget> pageDestination() {
  return const <Widget>[
    NavigationDestination(
      selectedIcon: Icon(Icons.home),
      icon: Icon(Icons.home_outlined),
      label: '首頁',
    ),
    NavigationDestination(
      selectedIcon: Icon(Icons.business),
      icon: Icon(Icons.business),
      label: '統計',
    ),
    NavigationDestination(
      selectedIcon: Icon(Icons.favorite),
      icon: Icon(Icons.favorite),
      label: '收藏',
    ),
    NavigationDestination(
      selectedIcon: Icon(Icons.collections_bookmark),
      icon: Icon(Icons.collections_bookmark),
      label: '紀錄',
    ),
    NavigationDestination(
      selectedIcon: Icon(Icons.person),
      icon: Icon(Icons.person),
      label: '我的',
    )
  ];
}
