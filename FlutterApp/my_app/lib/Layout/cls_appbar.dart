import 'package:flutter/material.dart';
import 'package:my_app/BaseConfig/config_appbar.dart';

class ClsAppBar1 extends AppBar {
  final BuildContext context;

  ClsAppBar1(this.context, {super.key})
      : super(
            backgroundColor: AppBarConfig1.backgroundColor(context),
            leading: Builder(
                builder: (context) => IconButton(
                    onPressed: () => Scaffold.of(context).openDrawer(),
                    icon: const Icon(
                      Icons.menu,
                      color: Colors.white,
                    ))),
            title: const Text("title my app"),
            centerTitle: true,
            actions: [
              IconButton(
                icon: const Icon(
                  Icons.home,
                  color: Colors.white,
                ),
                onPressed: () => debugPrint('home'),
              ),
              IconButton(
                icon: const Icon(
                  Icons.settings,
                  color: Colors.white,
                ),
                onPressed: () => debugPrint('settings'),
              )
            ]);
}
