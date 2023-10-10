import 'package:flutter/material.dart';

import 'package:sample_app/Widgets/cus_button.dart';
import 'package:sample_app/HttpService/callapi_1.dart';
import 'package:sample_app/Pages/page_device_info.dart';
import 'package:sample_app/Plugins/plugin_camera.dart';
import 'package:sample_app/Pages/page_image_picker.dart';
import 'package:sample_app/Pages/page_dialog.dart';
import 'package:sample_app/Pages/page_app_setting.dart';
import 'package:sample_app/Pages/page_mobile_scanner.dart';
import 'package:sample_app/Pages/page_premission_handler.dart';
import 'package:sample_app/Pages/page_quick_scan.dart';
import 'package:sample_app/Pages/page_path_provider.dart';

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
        cusButtons('plugin : rflutter_alert', [Colors.blue, Colors.green], 2,
            () {
          Navigator.push(context,
              MaterialPageRoute(builder: (context) => const PageDialog()));
        }),
        cusButtons('plugin : app_setting', [Colors.blue, Colors.green], 2, () {
          Navigator.push(context,
              MaterialPageRoute(builder: (context) => const PageAppSetting()));
        }),
        cusButtons('plugin : mobile_scanner', [Colors.blue, Colors.green], 2,
            () {
          Navigator.push(
              context,
              MaterialPageRoute(
                  builder: (context) => const PageMobileScanner()));
        }),
        cusButtons('Page Quick Scann', [Colors.blue, Colors.green], 2, () {
          Navigator.push(context,
              MaterialPageRoute(builder: (context) => const PageQuickScann()));
        }),
        cusButtons(
            'plugin : premission_handler', [Colors.blue, Colors.green], 2, () {
          Navigator.push(
              context,
              MaterialPageRoute(
                  builder: (context) => const PagePremissionHandler()));
        }),
        cusButtons('plugin : path_provider', [Colors.blue, Colors.green], 2,
            () {
          Navigator.push(
              context,
              MaterialPageRoute(
                  builder: (context) => const PagePathProvider()));
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
