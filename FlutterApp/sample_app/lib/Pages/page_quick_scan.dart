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
  BarcodeCapture? barcode;

  final MobileScannerController controller = MobileScannerController(
    torchEnabled: true,
    // formats: [BarcodeFormat.qrCode]
    // facing: CameraFacing.front,
    // detectionSpeed: DetectionSpeed.normal
    // detectionTimeoutMs: 1000,
    // returnImage: true,
  );

  bool isStarted = true;

  void _startOrStop() {
    try {
      if (isStarted) {
        controller.stop();
      } else {
        controller.start();
      }
      setState(() {
        isStarted = !isStarted;
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
    controller.dispose();
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
    return const SafeArea(
        child: Column(
      children: [
        Expanded(
          flex: 2,
          child: Center(
            child: Text(
              'Your scanned barcode will appear here!',
            ),
          ),
        ),
        Expanded(
          child: Center(
            child: Text(
              'Your scanned barcode will appear here!',
            ),
          ),
        )
      ],
    ));
  }

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
                    IconButton(
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
                    ),
                    IconButton(
                      color: Colors.white,
                      icon: isStarted
                          ? const Icon(Icons.stop)
                          : const Icon(Icons.play_arrow),
                      iconSize: 32.0,
                      onPressed: _startOrStop,
                    ),
                    Center(
                      child: SizedBox(
                        width: MediaQuery.of(context).size.width - 200,
                        height: 50,
                        child: FittedBox(
                          child: Text(
                            barcode?.barcodes.first.rawValue ??
                                'Scan something!',
                            overflow: TextOverflow.fade,
                            style: Theme.of(context)
                                .textTheme
                                .headlineMedium!
                                .copyWith(color: Colors.white),
                          ),
                        ),
                      ),
                    ),
                    IconButton(
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
                    ),
                  ],
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget wScanControl() {
    return MobileScanner(
      controller: controller,
      errorBuilder: (context, error, child) {
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
