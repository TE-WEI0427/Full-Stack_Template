import 'package:flutter/material.dart';

import 'package:sample_app/BaseConfig/config_materialapp.dart';
import 'package:sample_app/BaseConfig/config_globalvar.dart';

import 'package:sample_app/Layout/cls_appbar.dart';
import 'package:sample_app/Layout/cls_bottombar.dart';

import 'package:sample_app/Layout/widget_body.dart';
import 'package:sample_app/Layout/widget_destination.dart';
import 'package:sample_app/Layout/widget_drawer.dart';

void main() {
  WidgetsFlutterBinding.ensureInitialized();
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Flutter Demo',
      theme: ThemeData(
          colorScheme: ThemeDataConfig1.colorScheme,
          useMaterial3: ThemeDataConfig1.useMaterial3),
      home: const MyHomePage(),
      navigatorKey: GlobalVar.gNavState,
    );
  }
}

class MyHomePage extends StatefulWidget {
  const MyHomePage({super.key});

  @override
  State<MyHomePage> createState() => _MyHomePageState();
}

class _MyHomePageState extends State<MyHomePage> {
  /// 當前頁面索引值
  int currentPageIndex = 0;

  @override
  void initState() {
    super.initState();
  }

  /// 頁面切換事件
  void onPageChange(int index) {
    setState(() {
      debugPrint(index.toString());
      currentPageIndex = index;
    });
  }

  @override
  Widget build(BuildContext context) {
    void topBarLeadingOnPressed() {
      GlobalVar.gScafKey.currentState!.openDrawer();
      // Scaffold.of(context).openDrawer();
    }

    void topBarOnPressed_1() {
      debugPrint("topBarOnPressed_1");
    }

    void topBarOnPressed_2() {
      debugPrint("topBarOnPressed_2");
    }

    var topBar = ClsAppBar1(context, "Flutter Demo", topBarLeadingOnPressed,
        topBarOnPressed_1, topBarOnPressed_2);

    var bottomBar = ClsBottomBar1(
        currentPageIndex: currentPageIndex,
        onPageChange: onPageChange,
        indicatorColor: Colors.amber[800],
        destinations: pageDestination());

    var pBody = pagebody(context, currentPageIndex);

    var drawer = drawer1("AndyHsu", "andyhsu@zhtech.com.tw");

    return Scaffold(
        key: GlobalVar.gScafKey,
        drawer: drawer,
        appBar: topBar,
        bottomNavigationBar: bottomBar,
        body: pBody);
  }
}
