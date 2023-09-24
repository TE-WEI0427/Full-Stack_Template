import 'package:flutter/material.dart';
import 'package:sample_app/BaseConfig/config_appbar.dart';
import 'package:sample_app/BaseConfig/config_materialapp.dart';

import 'package:sample_app/PluginSampleCode/mobile_scanner_example/barcode_list_scanner_controller.dart';
import 'package:sample_app/PluginSampleCode/mobile_scanner_example/barcode_scanner_controller.dart';
import 'package:sample_app/PluginSampleCode/mobile_scanner_example/barcode_scanner_pageview.dart';
import 'package:sample_app/PluginSampleCode/mobile_scanner_example/barcode_scanner_returning_image.dart';
import 'package:sample_app/PluginSampleCode/mobile_scanner_example/barcode_scanner_window.dart';
import 'package:sample_app/PluginSampleCode/mobile_scanner_example/barcode_scanner_without_controller.dart';
import 'package:sample_app/PluginSampleCode/mobile_scanner_example/barcode_scanner_zoom.dart';

class PageMobileScanner extends StatefulWidget {
  const PageMobileScanner({Key? key}) : super(key: key);

  @override
  State<PageMobileScanner> createState() => _PageMobileScannerState();
}

class _PageMobileScannerState extends State<PageMobileScanner> {
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
    Widget wbody = SizedBox(
        width: MediaQuery.of(context).size.width,
        height: MediaQuery.of(context).size.height,
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            ElevatedButton(
              onPressed: () {
                Navigator.of(context).push(
                  MaterialPageRoute(
                    builder: (context) =>
                        const BarcodeListScannerWithController(),
                  ),
                );
              },
              child: const Text('MobileScanner with List Controller'),
            ),
            ElevatedButton(
              onPressed: () {
                Navigator.of(context).push(
                  MaterialPageRoute(
                    builder: (context) => const BarcodeScannerWithController(),
                  ),
                );
              },
              child: const Text('MobileScanner with Controller'),
            ),
            ElevatedButton(
              onPressed: () {
                Navigator.of(context).push(
                  MaterialPageRoute(
                    builder: (context) => const BarcodeScannerWithScanWindow(),
                  ),
                );
              },
              child: const Text('MobileScanner with ScanWindow'),
            ),
            ElevatedButton(
              onPressed: () {
                Navigator.of(context).push(
                  MaterialPageRoute(
                    builder: (context) => const BarcodeScannerReturningImage(),
                  ),
                );
              },
              child:
                  const Text('MobileScanner with Controller (returning image)'),
            ),
            ElevatedButton(
              onPressed: () {
                Navigator.of(context).push(
                  MaterialPageRoute(
                    builder: (context) =>
                        const BarcodeScannerWithoutController(),
                  ),
                );
              },
              child: const Text('MobileScanner without Controller'),
            ),
            ElevatedButton(
              onPressed: () {
                Navigator.of(context).push(
                  MaterialPageRoute(
                    builder: (context) => const BarcodeScannerWithZoom(),
                  ),
                );
              },
              child: const Text('MobileScanner with zoom slider'),
            ),
            ElevatedButton(
              onPressed: () {
                Navigator.of(context).push(
                  MaterialPageRoute(
                    builder: (context) => const BarcodeScannerPageView(),
                  ),
                );
              },
              child: const Text('MobileScanner pageView'),
            ),
          ],
        ));

    return MaterialApp(
        theme: ThemeData(
            colorScheme: ThemeDataConfig1.colorScheme,
            useMaterial3: ThemeDataConfig1.useMaterial3),
        home: Scaffold(
            appBar: AppBar(
              backgroundColor: AppBarConfig1.backgroundColor(context),
              leading: AppBarConfig1.wBackButton(context),
              title: const Text("mobile scanner sample"),
            ),
            body: wbody));
  }
}
