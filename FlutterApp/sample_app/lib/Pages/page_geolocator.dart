import 'package:flutter/material.dart';
import 'package:sample_app/BaseConfig/config_appbar.dart';
import 'package:sample_app/BaseConfig/config_materialapp.dart';

import 'package:sample_app/Plugins/plugin_geolocator.dart';
import 'package:geolocator/geolocator.dart';

class PageGeolocator extends StatefulWidget {
  const PageGeolocator({Key? key}) : super(key: key);

  @override
  State<PageGeolocator> createState() => _PageGeolocatorState();
}

class _PageGeolocatorState extends State<PageGeolocator> {
  // late Position position;
  // late String errorMsg;

  @override
  void initState() {
    super.initState();
    // determinePosition()
    //     .then((value) => position = value)
    //     .catchError((error) => errorMsg = error);
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
              title: const Text("Geolocator sample"),
            ),
            body: null));
  }
}
