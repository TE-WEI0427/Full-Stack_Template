import 'package:flutter/material.dart';
import 'package:sample_app/BaseConfig/config_appbar.dart';

/// 主頁 AppBar 樣式
///
/// 樣式_1
///
/// 掛在 Scaffold 的 appBar 參數上
class ClsAppBar1 extends AppBar {
  final BuildContext context;

  final String strTitle;

  final Function btnLeadingOnPressed;

  final Function btnOnPressed_1;
  final Function btnOnPressed_2;

  /// 建構子
  ///
  /// ---------------------------------------------------------------
  /// 參數說明
  ///
  /// context : 當前 context
  ///
  /// strTitle : 標題
  ///
  /// btnLeadingOnPressed : 按下 leading 後，調用的方法
  ///
  /// btnOnPressed_1 : 按下 右邊 第一個按鈕(從畫面左邊數) 後，調用的方法
  ///
  /// btnOnPressed_2 : 按下 右邊 第二個按鈕(從畫面左邊數) 後，調用的方法
  ClsAppBar1(this.context, this.strTitle, this.btnLeadingOnPressed,
      this.btnOnPressed_1, this.btnOnPressed_2,
      {super.key})
      : super(
            backgroundColor: AppBarConfig1.backgroundColor(context),
            leading: Builder(
                builder: (context) => IconButton(
                    onPressed: () =>
                        // Scaffold.of(context).openDrawer();
                        btnLeadingOnPressed(),
                    icon: const Icon(
                      Icons.menu,
                      color: Colors.white,
                    ))),
            title: Text(strTitle),
            centerTitle: true,
            actions: [
              IconButton(
                icon: const Icon(
                  Icons.home,
                  color: Colors.white,
                ),
                onPressed: () => btnOnPressed_1(),
              ),
              IconButton(
                icon: const Icon(
                  Icons.settings,
                  color: Colors.white,
                ),
                onPressed: () => btnOnPressed_2(),
              )
            ]);
}
