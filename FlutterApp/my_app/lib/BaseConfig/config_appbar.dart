import 'package:flutter/material.dart';

class AppBarConfig1 {
  static backgroundColor(BuildContext context) {
    return Theme.of(context).colorScheme.inversePrimary;
  }

  /// 回上一頁
  static Widget backButton(BuildContext context) {
    return IconButton(
      icon: const Icon(Icons.arrow_back, color: Colors.black),
      onPressed: () => Navigator.of(context).pop(),
    );
  }
}
