import 'package:flutter/material.dart';
import 'package:sample_app/BaseConfig/config_appbar.dart';
import 'package:sample_app/BaseConfig/config_materialapp.dart';

import 'package:mobile_scanner/mobile_scanner.dart';

class PageQuickScann extends StatefulWidget {
  const PageQuickScann({Key? key}) : super(key: key);

  @override
  State<PageQuickScann> createState() => _PageQuickScannState();
}

class _PageQuickScannState extends State<PageQuickScann> {
  /// 掃描一幀後傳回的物件。
  BarcodeCapture? barcode;

  /// 條碼掃描器的控制器
  final MobileScannerController controller = MobileScannerController(
    torchEnabled: false, // 啟動時啟用或停用手電筒（閃光燈）
    // formats: [BarcodeFormat.qrCode], // 如果提供，掃描器將僅檢測那些特定格式
    // facing: CameraFacing.front, // 選擇應使用哪一台相機
    // detectionSpeed: DetectionSpeed.normal, // 設定檢測速度。可能會導致某些裝置出現記憶體問題
    // detectionTimeoutMs: 1000, // 設定掃描器的超時時間。 超時設定以毫秒為單位。
    // returnImage: true, // 如果您想透過 Barcode 事件返回影像 則設為 true
  );

  /// 是否開始掃描
  bool isStarted = true;

  /// 控制 開始掃描 或 停止掃描
  void _startOrStop() {
    try {
      if (isStarted) {
        controller.stop(); // 開始掃描
      } else {
        controller.start(); // 停止掃描
      }
      setState(() {
        isStarted = !isStarted; // 更新狀態
      });
    } on Exception catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Something went wrong! $e'),
          backgroundColor: Colors.red,
        ),
      );
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
    controller.dispose(); // 銷毀掃描相機資源
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
              title: const Text("Quick Scann sample"),
            ),
            body: bodyContainer()));
  }

  Widget bodyContainer() {
    return SafeArea(
        child: Column(
      children: [
        barCodeScannerContainer(),
        const Expanded(
          child: Center(
            child: Text(
              'Your scanned barcode will appear here!',
            ),
          ),
        )
      ],
    ));
  }

  /// 條碼掃描區塊
  Widget barCodeScannerContainer() {
    return Expanded(
      flex: 2,
      child: ColoredBox(
        color: Colors.grey,
        child: Stack(
          children: [
            wScanControl(),
            Align(
              alignment: Alignment.bottomCenter,
              child: Container(
                alignment: Alignment.bottomCenter,
                height: 100,
                color: Colors.black.withOpacity(0.4),
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                  children: [
                    btnTorchSwitch(),
                    btnCameraSwitch(),
                    wShowText(),
                    btnCameraFacingSwitch(),
                  ],
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }

  /// 設相機的燈光控制
  Widget btnTorchSwitch() {
    return IconButton(
      color: Colors.white,
      icon: ValueListenableBuilder(
        valueListenable: controller.torchState,
        builder: (context, state, child) {
          switch (state) {
            case TorchState.off:
              return const Icon(
                Icons.flash_off,
                color: Colors.grey,
              );
            case TorchState.on:
              return const Icon(
                Icons.flash_on,
                color: Colors.yellow,
              );
          }
        },
      ),
      iconSize: 32.0,
      onPressed: () => controller.toggleTorch(),
    );
  }

  /// 條碼掃描器 開啟或關閉按鈕
  Widget btnCameraSwitch() {
    return IconButton(
      color: Colors.white,
      icon: isStarted ? const Icon(Icons.stop) : const Icon(Icons.play_arrow),
      iconSize: 32.0,
      onPressed: _startOrStop,
    );
  }

  /// 顯示掃描後的字串資料
  Widget wShowText() {
    return Center(
      child: SizedBox(
        width: MediaQuery.of(context).size.width - 200,
        height: 50,
        child: FittedBox(
          child: Text(
            barcode?.barcodes.first.rawValue ?? 'Scan something!',
            overflow: TextOverflow.fade,
            style: Theme.of(context)
                .textTheme
                .headlineMedium!
                .copyWith(color: Colors.white),
          ),
        ),
      ),
    );
  }

  /// 條碼掃描器的朝向
  Widget btnCameraFacingSwitch() {
    return IconButton(
      color: Colors.white,
      icon: ValueListenableBuilder(
        valueListenable: controller.cameraFacingState,
        builder: (context, state, child) {
          switch (state) {
            case CameraFacing.front:
              return const Icon(Icons.camera_front);
            case CameraFacing.back:
              return const Icon(Icons.camera_rear);
          }
        },
      ),
      iconSize: 32.0,
      onPressed: () => controller.switchCamera(),
    );
  }

  /// 管理條碼掃描器的控制器
  Widget wScanControl() {
    return MobileScanner(
      controller: controller,
      errorBuilder: (context, error, child) {
        // 當掃描器無法啟動時，顯示錯誤的小部件的功能
        // 如果為 null，則預設為黑色 [ColoredBox]，並帶有居中的白色 [Icons.error] 圖示。
        return wfixError(error);
      },
      fit: BoxFit.contain,
      onDetect: (barcode) {
        setState(() {
          this.barcode = barcode;
        });
      },
    );
  }

  /// 條碼掃描器無法啟動，建構顯示錯誤的小部件的功能
  Widget wfixError(MobileScannerException error) {
    String errorMessage;
    switch (error.errorCode) {
      case MobileScannerErrorCode.controllerUninitialized:
        errorMessage = 'Controller not ready.';
        break;
      case MobileScannerErrorCode.permissionDenied:
        errorMessage = 'Permission denied';
        break;
      case MobileScannerErrorCode.unsupported:
        errorMessage = 'Scanning is unsupported on this device';
        break;
      default:
        errorMessage = 'Generic Error';
        break;
    }
    return Text(errorMessage);
  }
}
