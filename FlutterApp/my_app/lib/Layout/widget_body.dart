import 'package:flutter/material.dart';

Widget pagebody(int currentPageIndex) {
  return <Widget>[
    Container(
      color: Colors.yellow,
      alignment: Alignment.center,
      child: const Text('Page 1'),
    ),
    Container(
      color: Colors.green,
      alignment: Alignment.center,
      child: const Text('Page 2'),
    ),
    Container(
      color: Colors.blue,
      alignment: Alignment.center,
      child: const Text('Page 3'),
    ),
    Container(
      color: Colors.pink,
      alignment: Alignment.center,
      child: const Text('Page 4'),
    ),
    Container(
      color: Colors.orange,
      alignment: Alignment.center,
      child: const Text('Page 5'),
    )
  ][currentPageIndex];
}
