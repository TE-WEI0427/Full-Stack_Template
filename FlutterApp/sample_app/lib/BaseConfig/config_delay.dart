import 'package:flutter/material.dart';

// 這裡設定一些 delay 的 function，通常會有一個回調函式參數
// 搭配 FutureBuilder 使用，方法掛在 future 參數上。

/// Use this method on the future parameter of FutureBuilder
Future<Widget> delayGetWidge(Function callback) {
  return Future.delayed(const Duration(seconds: 1), () => callback());
}

/// Use this method on the future parameter of FutureBuilder
Future<void> delayGetVoid(Future<void> callback) {
  return Future.delayed(const Duration(seconds: 1), () => callback);
}
