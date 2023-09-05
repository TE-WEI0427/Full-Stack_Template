import 'package:flutter/material.dart';
import 'package:my_app/BaseConfig/config_materialapp.dart';

import 'package:my_app/Layout/cls_appbar.dart';
import 'package:my_app/Layout/cls_bottombar.dart';
import 'package:my_app/Layout/widget_body.dart';
import 'package:my_app/Layout/widget_destination.dart';
import 'package:my_app/Layout/widget_drawer.dart';

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
    );
  }
}

class MyHomePage extends StatefulWidget {
  const MyHomePage({super.key});

  @override
  State<MyHomePage> createState() => _MyHomePageState();
}

class _MyHomePageState extends State<MyHomePage> {
  int currentPageIndex = 0;

  @override
  void initState() {
    super.initState();
  }

  void onPageChange(int index) {
    setState(() {
      debugPrint(index.toString());
      currentPageIndex = index;
    });
  }

  var destinations = pageDestination();

  @override
  Widget build(BuildContext context) {
    var topBar = ClsAppBar1(context);

    var bottomBar = ClsBottomBar1(
        currentPageIndex: currentPageIndex,
        onPageChange: onPageChange,
        indicatorColor: Colors.amber[800],
        destinations: destinations);

    var pBody = pagebody(context, currentPageIndex);

    var drawer = drawer1("AndyHsu", "andyhsu@zhtech.com.tw");

    return Scaffold(
        drawer: drawer,
        appBar: topBar,
        bottomNavigationBar: bottomBar,
        body: pBody);
  }
}
