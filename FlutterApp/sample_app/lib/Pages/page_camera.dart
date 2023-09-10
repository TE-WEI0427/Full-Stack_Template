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
  late CameraController _cameracontroller;
  late Future<void> _initializeControllerFuture;

  late StreamController<List<String>> imagesStreamController; // 圖片資料存取控制器
  var images = <String>[]; // 儲存顯示圖片的路徑字串

  @override
  void initState() {
    super.initState();

    // (3) camera
    _cameracontroller = getCameraControl(widget.camera);
    _initializeControllerFuture = cameraInitState(_cameracontroller);

    imagesStreamController = StreamController.broadcast(); // 初始化控制器
  }

  @override
  void dispose() {
    // Dispose of the controller when the widget is disposed.
    // (4) camera
    cameraDispose(_cameracontroller);
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
            title: const Text("Camera"),
          ),
          body: bodyContainer(),
          floatingActionButton:
              action(_initializeControllerFuture, _cameracontroller)),
    );
  }

  /// 拍照並將圖片顯示出來
  Widget action(Future<void> initializeControllerFuture,
      CameraController cameracontroller) {
    return FloatingActionButton(
      child: const Icon(Icons.camera_alt),
      // Provide an onPressed callback.
      onPressed: () async {
        // Take the Picture in a try / catch block. If anything goes wrong,
        // catch the error.
        try {
          // (5) camera
          String imagePath =
              await openCamera(initializeControllerFuture, _cameracontroller);
          devLog("$runtimeType action", imagePath);

          // 儲存圖片路徑
          images.add(imagePath);

          // 設定控制器資料 (將資料陣列設定給他)
          imagesStreamController.sink.add(images);
        } catch (e) {
          // If an error occurs, log the error to the console.
          devError("$runtimeType action", e.runtimeType);
        }
      },
    );
  }

  /// page body
  bodyContainer() {
    return FutureBuilder<void>(
      future: delayGetVoid(_initializeControllerFuture),
      builder: (context, snapshot) {
        if (snapshot.connectionState == ConnectionState.done) {
          // If the Future is complete, display the preview.
          return Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              cameraContainer(),
              SizedBox(height: MediaQuery.of(context).size.height * 0.01),
              imgContainer(),
              SizedBox(height: MediaQuery.of(context).size.height * 0.01),
            ],
          );
        } else {
          // Otherwise, display a loading indicator.
          return const Center(child: CircularProgressIndicator());
        }
      },
    );
  }

  /// 拍照區塊
  SizedBox cameraContainer() {
    return SizedBox(
        width: MediaQuery.of(context).size.width,
        height: MediaQuery.of(context).size.height * 0.6,
        child: CameraPreview(_cameracontroller));
  }

  /// 圖片區塊 (SingleChildScrollView)
  Expanded imgContainer() {
    return Expanded(
        child: SingleChildScrollView(
            child: SizedBox(
                // 局部更新此部件
                child: StreamBuilder(
      initialData: const <String>[], // 初始化需要刷新的数据
      stream: imagesStreamController.stream, // 控制器對應的stream
      builder: (context, snapshot) {
        devLog("$runtimeType", snapshot.data.runtimeType.toString());
        // snapshot.data 對應initialData的資料類型
        if (!snapshot.hasData) {
          // Check the status here
          return const Center(child: CircularProgressIndicator());
        } else {
          return GridView(
              gridDelegate: const SliverGridDelegateWithFixedCrossAxisCount(
                crossAxisCount: 4, // 單行數量
                crossAxisSpacing: 10, // 行 間距
                mainAxisSpacing: 10, // 列 間距
              ),
              primary: false,
              shrinkWrap: true,
              children: imgListGenerate(snapshot));
        }
      },
    ))));
  }

  /// list 圖片顯示 (在這裡設定圖片大小)
  List<Widget> imgListGenerate(AsyncSnapshot<List<String>> snapshot) {
    return List<Widget>.generate(
        snapshot.data!.length,
        ((index) => SizedBox(
            width: 100,
            height: 100,
            child: Image.file(File(snapshot.data![index])))));
  }
}
