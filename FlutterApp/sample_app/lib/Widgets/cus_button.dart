import 'package:flutter/material.dart';

/// 自訂按鈕
///
/// 自訂1
///
/// ------------------------------------------------------------
/// 參數說明
///
/// text : 按鈕文字
///
/// borderColors : 框線顏色 [點擊時的顏色, 未點擊時的顏色]
///
/// borderWidth : 框線粗細
///
/// onClick : 按下按鈕後調用的事件
Widget cusButtons(String text, List<Color> borderColors, double borderWidth,
    Function onClick) {
  return OutlinedButton(
    style: ButtonStyle(
      shape: MaterialStateProperty.all<OutlinedBorder>(const StadiumBorder()),
      side: MaterialStateProperty.resolveWith<BorderSide>(
          (Set<MaterialState> states) {
        final Color color = states.contains(MaterialState.pressed)
            ? borderColors[0]
            : borderColors[1];
        return BorderSide(color: color, width: borderWidth);
      }),
    ),
    onPressed: () {
      onClick();
    },
    child: Text(text),
  );
}
