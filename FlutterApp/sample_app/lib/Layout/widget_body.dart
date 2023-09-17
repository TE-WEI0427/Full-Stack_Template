import 'package:flutter/material.dart';

import 'package:sample_app/Widgets/cus_button.dart';
import 'package:sample_app/HttpService/callapi_1.dart';
import 'package:sample_app/Pages/page_device_info.dart';
import 'package:sample_app/Plugins/plugin_camera.dart';
import 'package:sample_app/Pages/page_image_picker.dart';
import 'package:sample_app/Pages/page_dialog.dart';

/// 頁面內容元件
///
/// 搭配 NavigationBar 使用
///
/// ---------------------------------
/// 參數說明
///
/// currentPageIndex : NavigationBar 的  selectedIndex
Widget pagebody(BuildContext context, int currentPageIndex) {
  return <Widget>[
    Container(
      color: Colors.yellow,
      alignment: Alignment.center,
      child: Column(children: [
        cusButtons('Get Token', [Colors.blue, Colors.green], 2, () {
          getToken();
        }),
        cusButtons('plugin : Device info plus', [Colors.blue, Colors.green], 2,
            () {
          Navigator.push(context,
              MaterialPageRoute(builder: (context) => const PageDEviceInfo()));
        }),
        cusButtons('plugin : Camera', [Colors.blue, Colors.green], 2, () {
          pushToCamera(context);
        }),
        cusButtons('plugin : Image picker', [Colors.blue, Colors.green], 2, () {
          Navigator.push(context,
              MaterialPageRoute(builder: (context) => const PageImagePicker()));
        }),
        cusButtons('ShowDialog', [Colors.blue, Colors.green], 2, () {
          Navigator.push(context,
              MaterialPageRoute(builder: (context) => const PageDialog()));
        }),
      ]),
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
