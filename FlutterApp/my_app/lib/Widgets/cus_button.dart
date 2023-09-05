import 'package:flutter/material.dart';

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
