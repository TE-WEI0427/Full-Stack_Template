import 'package:flutter/material.dart';

import 'package:sample_app/BaseConfig/config_appbar.dart';
import 'package:sample_app/BaseConfig/config_materialapp.dart';
import 'package:sample_app/BaseConfig/config_printlog.dart';

import 'package:sample_app/Plugins/plugin_rflutter_alert.dart';

import 'package:sample_app/Widgets/cus_dialog.dart';
import 'package:sample_app/Widgets/cus_common.dart';

class PageDialog extends StatefulWidget {
  const PageDialog({Key? key}) : super(key: key);

  @override
  State<PageDialog> createState() => _PageDialogState();
}

class _PageDialogState extends State<PageDialog> {
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
    return MaterialApp(
        theme: ThemeData(
            colorScheme: ThemeDataConfig1.colorScheme,
            useMaterial3: ThemeDataConfig1.useMaterial3),
        home: Scaffold(
            appBar: AppBar(
              backgroundColor: AppBarConfig1.backgroundColor(context),
              leading: AppBarConfig1.wBackButton(context),
              title: const Text("Dialog sample"),
            ),
            body: SingleChildScrollView(
                child: Container(
                    margin: const EdgeInsets.all(8.0),
                    child: Center(
                      child: Column(
                        children: [
                          wCusShowAlertDialog(),
                          wCusShowConfirmDialog(),
                          wCusShowSimpleDialog(),
                          wHr(text: "rflutter_alert plugin Sample"),
                          wSampleWidget(context),
                          wHr(text: "implement rflutter_alert plugin"),
                          wBtnShowDialog("Success"),
                          wBtnShowDialog("Warning"),
                          wBtnShowDialog("Error"),
                        ],
                      ),
                    )))));
  }

  /// 警告提示框
  Widget wCusShowAlertDialog() {
    /// 回調方法，會接收一個索引值。
    void callBack() {
      devLog("wCusShowAlertDialog", "Alert checked");
    }

    // 按鈕
    return ElevatedButton(
      onPressed: () async {
        cusShowAlertDialog("提示", "測試訊息框", "確認", true, callBack);
      },
      child: const Text(
        'Show Alert Dialog',
        style: TextStyle(fontSize: 24),
      ),
    );
  }

  /// 確認提示框
  Widget wCusShowConfirmDialog() {
    /// 回調方法，會接收一個索引值。
    void callBack(int index) {
      devLog("wCusShowConfirmDialog", index.toString());
      switch (index) {
        case 1:
          devLog("wCusShowConfirmDialog", "確認");
          break;
        case 2:
          devLog("wCusShowConfirmDialog", "取消");
          break;
      }
    }

    // 按鈕
    return ElevatedButton(
      onPressed: () async {
        cusShowConfirmDialog("訊息", "測試訊息框", ["確認", "取消"], true, callBack);
      },
      child: const Text(
        'Show Confirm Dialog',
        style: TextStyle(fontSize: 24),
      ),
    );
  }

  /// 選項列表提示框
  Widget wCusShowSimpleDialog() {
    /// 回調方法，會接收一個索引值。
    void callBack(String str) {
      devLog("wCusShowSimpleDialog", str);
    }

    List<String> btnNameList = ["General", "Silver", "Gold"];

    /// Option widget
    SimpleDialogOption getOptions(String str) {
      return SimpleDialogOption(
        onPressed: () {
          Navigator.of(context).pop();
          callBack(str);
        },
        child: Text(str),
      );
    }

    // 提示框的 widget list
    List<Widget> getChildren() {
      List<Widget> rtnWidgets = [];
      for (String element in btnNameList) {
        rtnWidgets.add(getOptions(element));
      }

      return rtnWidgets;
    }

    // 按鈕
    return ElevatedButton(
      onPressed: () async {
        cusShowSimpleDialog("訊息", false, getChildren());
      },
      child: const Text(
        'Show Simple Dialog',
        style: TextStyle(fontSize: 24),
      ),
    );
  }

  //rflutter_alert------------------------------------------------------------

  /// 成功/警告/錯誤 提示框
  Widget wBtnShowDialog(String type) {
    return ElevatedButton(
      onPressed: () {
        devLog("wBtnShowDialog", type);
        switch (type) {
          case "Success":
            easyDialog("Success", "成功", "測試訊息框");
            break;
          case "Warning":
            easyDialog("Warning", "警告", "測試訊息框");
            break;
          case "Error":
            easyDialog("Error", "錯誤", "測試訊息框");
            break;
        }
      },
      child: Text(
        '$type Dialog',
        style: const TextStyle(fontSize: 24),
      ),
    );
  }
}
