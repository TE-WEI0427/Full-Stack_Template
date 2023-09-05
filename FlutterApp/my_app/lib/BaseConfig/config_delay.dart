import 'package:flutter/material.dart';

Future<Widget> delayGetWidge(Function callback) {
  return Future.delayed(const Duration(seconds: 1), () => callback());
}
