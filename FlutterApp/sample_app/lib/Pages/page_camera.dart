import 'dart:async';
import 'dart:io';

import 'package:flutter/material.dart';

import 'package:sample_app/BaseConfig/config_appbar.dart';
import 'package:sample_app/BaseConfig/config_delay.dart';
import 'package:sample_app/BaseConfig/config_materialapp.dart';
import 'package:sample_app/BaseConfig/config_printlog.dart';

import 'package:camera/camera.dart';
import 'package:sample_app/Plugins/plugin_camera.dart';

class PageCamera extends StatefulWidget {
  // (1) camera
  final CameraDescription camera;

  const PageCamera({Key? key, required this.camera}) : super(key: key);

  @override
  State<PageCamera> createState() => _PageCameraState();
}

class _PageCameraState extends State<PageCamera> {
  // (2) camera
  late CameraController _controller;
  late Future<void> _initializeControllerFuture;

  late StreamController<List<Widget>> imagesStreamController; // 圖片資料存取控制器
  var images = <Widget>[]; // 儲存顯示圖片的 Widget

  @override
  void initState() {
    super.initState();

    // (3) camera
    _controller = getCameraControl(widget.camera);
    _initializeControllerFuture = cameraInitState(_controller);

    imagesStreamController = StreamController.broadcast(); // 初始化控制器
  }

  @override
  void dispose() {
    // Dispose of the controller when the widget is disposed.
    // (4) camera
    cameraDispose(_controller);
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
            leading: AppBarConfig1.backButton(context),
            title: const Text("Camera sample"),
          ),
          body: FutureBuilder<void>(
            future: delayGetVoid(_initializeControllerFuture),
            builder: (context, snapshot) {
              if (snapshot.connectionState == ConnectionState.done) {
                // If the Future is complete, display the preview.
                return Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    SizedBox(
                        width: MediaQuery.of(context).size.width,
                        height: MediaQuery.of(context).size.height * 0.6,
                        child: CameraPreview(_controller)),
                    SizedBox(height: MediaQuery.of(context).size.height * 0.01),
                    SizedBox(
                        child: StreamBuilder(
                      initialData: const <Widget>[], // 初始化需要刷新的数据
                      stream: imagesStreamController.stream, // 控制器對應的stream
                      builder: (context, snapshot) {
                        devLog("$runtimeType",
                            snapshot.data.runtimeType.toString());
                        // snapshot.data 對應initialData的資料類型
                        return Row(
                          children: snapshot.data!,
                        );
                      },
                    ))
                  ],
                );
              } else {
                // Otherwise, display a loading indicator.
                return const Center(child: CircularProgressIndicator());
              }
            },
          ),
          floatingActionButton:
              action(_initializeControllerFuture, _controller)),
    );
  }

  /// 拍照後新增一個圖片區塊
  Widget action(
      Future<void> initializeControllerFuture, CameraController controller) {
    return FloatingActionButton(
      child: const Icon(Icons.camera_alt),
      // Provide an onPressed callback.
      onPressed: () async {
        // Take the Picture in a try / catch block. If anything goes wrong,
        // catch the error.
        try {
          // (5) camera
          String imagePath =
              await openCamera(initializeControllerFuture, controller);
          devLog("$runtimeType action", imagePath);

          // 新增 一個 圖片顯示區塊
          images.add(SizedBox(
            width: 150,
            height: 150,
            child: Image.file(File(imagePath)),
          ));

          // 設定控制器資料
          imagesStreamController.sink.add(images);
        } catch (e) {
          // If an error occurs, log the error to the console.
          devError("$runtimeType action", e.runtimeType);
        }
      },
    );
  }
}
