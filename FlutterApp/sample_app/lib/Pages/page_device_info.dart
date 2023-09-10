import 'package:flutter/material.dart';

import 'package:sample_app/BaseConfig/config_appbar.dart';
import 'package:sample_app/BaseConfig/config_materialapp.dart';
import 'package:sample_app/BaseConfig/config_printlog.dart';
import 'package:sample_app/BaseConfig/config_delay.dart';

import 'package:device_info_plus/device_info_plus.dart';
import 'package:sample_app/Plugins/plugin_device_info.dart';

class PageDEviceInfo extends StatefulWidget {
  const PageDEviceInfo({Key? key}) : super(key: key);

  @override
  State<PageDEviceInfo> createState() => _PageDEviceInfoState();
}

class _PageDEviceInfoState extends State<PageDEviceInfo> {
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
          title: Text(getAppBarTitle()),
          elevation: 4,
        ),
        body: FutureBuilder<Widget>(
          future: delayGetWidge(getDEviceInfo),
          builder: (context, snapshot) {
            devLog(runtimeType.toString(), snapshot.connectionState.toString());
            devLog(runtimeType.toString(), snapshot.hasData.toString());
            devError(runtimeType.toString(), snapshot.data.runtimeType);

            if (snapshot.connectionState == ConnectionState.done &&
                snapshot.hasData) {
              return snapshot.data!;
            } else {
              /// you handle others state like error while it will a widget no matter what, you can skip it
              return const Center(child: CircularProgressIndicator());
            }
          },
        ),
      ),
    );
  }
}

Future<Widget> getDEviceInfo() async {
  final DeviceInfoPlugin deviceInfoPlugin = DeviceInfoPlugin();

  Map<String, dynamic> deviceData = await initPlatformState(deviceInfoPlugin);

  return genDeviceInfoListView(deviceData);
}

Widget genDeviceInfoListView(Map<String, dynamic> deviceData) {
  return ListView(
      children: deviceData.keys.map(
    (String property) {
      return Row(
        children: <Widget>[
          Container(
            padding: const EdgeInsets.all(10),
            child: Text(
              property,
              style: const TextStyle(
                fontWeight: FontWeight.bold,
              ),
            ),
          ),
          Expanded(
            child: Container(
              padding: const EdgeInsets.symmetric(vertical: 10),
              child: Text(
                '${deviceData[property]}',
                maxLines: 10,
                overflow: TextOverflow.ellipsis,
              ),
            ),
          ),
        ],
      );
    },
  ).toList());
}
