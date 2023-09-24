import 'package:flutter/material.dart';
import 'package:sample_app/BaseConfig/config_appbar.dart';
import 'package:sample_app/BaseConfig/config_materialapp.dart';

import 'package:sample_app/Plugins/plugin_app_setting.dart';

class PageAppSetting extends StatefulWidget {
  const PageAppSetting({Key? key}) : super(key: key);

  @override
  State<PageAppSetting> createState() => _PageAppSettingState();
}

class _PageAppSettingState extends State<PageAppSetting> {
  @override
  void initState() {
    super.initState();
  }

  @override
  void dispose() {
    // Dispose of the controller when the widget is disposed.
    super.dispose();
  }

  Widget wbody() {
    final appSettingsActions = getOpenAppSettingsActions();
    final appSettingsPanelActions = getOpenAppSettingsPanelActions();

    return CustomScrollView(
      slivers: [
        const SliverToBoxAdapter(
          child: Padding(
            padding: EdgeInsets.all(8.0),
            child: Text(
              'openAppSettings() options',
              style: TextStyle(fontSize: 20, fontWeight: FontWeight.bold),
            ),
          ),
        ),
        SliverList(
          delegate: SliverChildListDelegate.fixed(appSettingsActions),
        ),
        const SliverToBoxAdapter(
          child: Padding(
            padding: EdgeInsets.all(8.0),
            child: Text(
              'openAppSettingsPanel() options',
              style: TextStyle(fontSize: 20, fontWeight: FontWeight.bold),
            ),
          ),
        ),
        SliverList(
          delegate: SliverChildListDelegate.fixed(appSettingsPanelActions),
        ),
      ],
    );
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
              title: const Text("Camera sample"),
            ),
            body: wbody()));
  }
}
