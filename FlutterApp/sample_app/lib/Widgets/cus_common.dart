import 'package:flutter/material.dart';

/// 橫線
Widget wHr({String text = ""}) {
  return Row(children: <Widget>[
    const Expanded(child: Divider()),
    Visibility(visible: text != "", child: Text(text)),
    const Expanded(child: Divider()),
  ]);
}
