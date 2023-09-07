import 'package:flutter/material.dart';

import 'package:camera/camera.dart';
import 'package:sample_app/Pages/page_camera.dart';

void pushToCamera(BuildContext context) async {
  final cameras = await getAvailableCamera();

  if (context.mounted) {
    // 前往照相機頁面
    Navigator.push(
      context,
      MaterialPageRoute(
        builder: (context) => PageCamera(camera: cameras.first),
      ),
    );
  }
}

/// 取得裝置上可用的攝像機列表
Future<List<CameraDescription>> getAvailableCamera() async {
  // Obtain a list of the available cameras on the device.
  final cameras = await availableCameras();

  return cameras;
}

/// 建立 攝相機 控制器
CameraController getCameraControl(CameraDescription camera) {
  late CameraController controller;

  // create a CameraController.
  controller = CameraController(
    // Get a specific camera from the list of available cameras.
    camera,
    // Define the resolution to use.
    ResolutionPreset.medium,
  );

  return controller;
}

/// 初始化控制器 並返回一個 Future<void>
Future<void> cameraInitState(CameraController controller) {
  late Future<void> initializeControllerFuture;

  // initialize the controller. This returns a Future.
  initializeControllerFuture = controller.initialize();

  return initializeControllerFuture;
}

/// 結束使用攝像機
void cameraDispose(CameraController controller) {
  // Dispose of the controller when the widget is disposed.
  controller.dispose();
}

// 開啟相機
Future<String> openCamera(Future<void> initializeControllerFuture,
    CameraController controller) async {
  // Ensure that the camera is initialized.
  await initializeControllerFuture;

  // Attempt to take a picture and get the file `image`
  // where it was saved.
  final image = await controller.takePicture();

  return image.path;
}
