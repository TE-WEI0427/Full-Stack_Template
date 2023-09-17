import 'dart:io';

import 'package:flutter/material.dart';

import 'package:sample_app/BaseConfig/config_appbar.dart';
import 'package:sample_app/BaseConfig/config_materialapp.dart';
import 'package:sample_app/BaseConfig/config_printlog.dart';

import 'package:sample_app/Plugins/plugin_image_picker.dart';

class PageImagePicker extends StatefulWidget {
  const PageImagePicker({Key? key}) : super(key: key);

  @override
  State<PageImagePicker> createState() => _PageImagePickerState();
}

class _PageImagePickerState extends State<PageImagePicker> {
  String path = ""; // 圖片路徑

  // 如果圖片路徑不為空值 就顯示路徑的圖片
  setPath(String imgPath) {
    if (imgPath.isNotEmpty) {
      setState(() {
        devLog("$runtimeType", imgPath);
        path = imgPath;
      });
    }
  }

  @override
  void initState() {
    super.initState();
  }

  @override
  void dispose() {
    // Dispose of the controller when the widget is disposed.
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
        theme: ThemeData(
            colorScheme: ThemeDataConfig1.colorScheme,
            useMaterial3: ThemeDataConfig1.useMaterial3),
        home: Scaffold(
            appBar: AppBar(
              backgroundColor: AppBarConfig1.backgroundColor(context),
              leading: AppBarConfig1.wBackButton(context),
              title: const Text("Image picker"),
            ),
            body: SizedBox(
                width: MediaQuery.of(context).size.width,
                child: SingleChildScrollView(
                  child: Column(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      imgContainer(),
                      SizedBox(
                          height: MediaQuery.of(context).size.height * 0.01),
                      btnUploadImage()
                    ],
                  ),
                ))));
  }

  /// 圖片顯示區塊
  Container imgContainer() {
    return Container(
      child: path.isNotEmpty
          ? Image.file(
              File(
                path,
              ),
              height: MediaQuery.of(context).size.height * 0.5,
              width: MediaQuery.of(context).size.width * 0.5,
            )
          : const Icon(Icons.abc_outlined), // 初始圖片
    );
  }

  /// 上傳圖片按鈕
  Container btnUploadImage() {
    return Container(
      color: Colors.black38,
      child: TextButton(
          onPressed: () {
            // 拍照或相簿選取
            showUploadImageOptionsDialog(setPath);
          },
          child: const Text("Image")),
    );
  }
}
