import 'package:flutter/material.dart';

import 'package:sample_app/BaseConfig/config_globalvar.dart';

import 'package:rflutter_alert/rflutter_alert.dart';

/// 自訂警告提示框
///
/// ------------------------------------------------------------
/// 參數說明
///
/// title : 標題
///
/// message : 訊息
///
/// btnName : 按鈕名稱
///
/// barrierDismissible : 提示框是否可以在點擊空白處後，關閉視窗
///
/// onClick : 按下按鈕後調用的事件
Future<void> cusShowAlertDialog(String title, String message, String btnName,
    bool barrierDismissible, Function onClick) async {
  return showDialog<void>(
    context: GlobalVar.gNavState.currentContext!,
    barrierDismissible: barrierDismissible, // user must tap button!
    builder: (BuildContext context) {
      return AlertDialog(
        title: Text(title),
        content: SingleChildScrollView(
          child: ListBody(
            children: <Widget>[
              Text(message),
            ],
          ),
        ),
        actions: <Widget>[
          TextButton(
            child: Text(btnName),
            onPressed: () {
              Navigator.of(context).pop();
              onClick();
            },
          )
        ],
      );
    },
  );
}

/// 自訂確認提示框
///
/// ------------------------------------------------------------
/// 參數說明
///
/// title : 標題
///
/// message : 訊息
///
/// btnNames : 按鈕名稱 ex.["確認","取消"]
///
/// barrierDismissible : 提示框是否可以在點擊空白處後，關閉視窗
///
/// onClick : 按下兩個按鈕中的任一個後調用的事件，回傳點擊按鈕的索引值。 ex.[1,2]
Future<void> cusShowConfirmDialog(String title, String message,
    List<String> btnNames, bool barrierDismissible, Function onClick) async {
  return showDialog<void>(
    context: GlobalVar.gNavState.currentContext!,
    barrierDismissible: barrierDismissible, // user must tap button!
    builder: (BuildContext context) {
      return AlertDialog(
        title: Text(title),
        content: SingleChildScrollView(
          child: ListBody(
            children: <Widget>[
              Text(message),
            ],
          ),
        ),
        actions: <Widget>[
          TextButton(
            child: Text(btnNames[0]),
            onPressed: () {
              Navigator.of(context).pop();
              onClick(1);
            },
          ),
          TextButton(
            child: Text(btnNames[1]),
            onPressed: () {
              Navigator.of(context).pop();
              onClick(2);
            },
          ),
        ],
      );
    },
  );
}

/// 自訂按鈕列提示框
///
/// ------------------------------------------------------------
/// 參數說明
///
/// title : 標題
///
/// message : 訊息
///
/// btnName : 按鈕名稱 ex.["確認","取消"]
///
/// barrierDismissible : 提示框是否可以在點擊空白處後，關閉視窗
///
/// onClick : 按下兩個按鈕中的任一個後調用的事件，回傳點擊按鈕的索引值。 ex.[1,2]
Future<void> cusShowSimpleDialog(String title, bool barrierDismissible,
    List<Widget> simpleDialogChildren) async {
  await showDialog<void>(
      context: GlobalVar.gNavState.currentContext!,
      barrierDismissible: barrierDismissible, // user must tap button!
      builder: (BuildContext context) {
        return SimpleDialog(
          title: Text(title),
          children: simpleDialogChildren,
        );
      });
}

//rflutter_alert---------------------------------------------------------------

/// 自訂簡易提示框
///
/// ------------------------------------------------------------
/// 參數說明
///
/// type : 提示框類型
///
/// title : 標題
///
/// message : 訊息
void easyDialog(String type, String title, String message) {
  // Reusable alert style
  // ignore: prefer_const_constructors
  var alertStyle = AlertStyle(
    // animationType: AnimationType.fromTop, // 動畫位置
    // isCloseButton: false, // 是否顯示關閉按鈕
    isOverlayTapDismiss: false, // 是否允許點擊空白處關閉
    // descStyle: const TextStyle(fontWeight: FontWeight.bold), // message style
    // animationDuration: const Duration(milliseconds: 400), // 動畫時間
    // 外框 Radius
    // alertBorder: RoundedRectangleBorder(
    //   borderRadius: BorderRadius.circular(0.0),
    //   side: const BorderSide(
    //     color: Colors.grey,
    //   ),
    // ),
    // titleStyle: const TextStyle( // title style
    //   color: Colors.red,
    // ),
    // constraints: const BoxConstraints.expand(width: 300), // 提示框寬度，但會導致提示框置頂
    //First to chars "55" represents transparency of color
    // overlayColor: const Color(0x55000000), // 屏蔽螢幕的色彩度
    // alertElevation: 0,
    // alertAlignment: Alignment.topCenter // 提示框螢幕位置
  );

  Alert(
    context: GlobalVar.gNavState.currentContext!,
    style: alertStyle,
    type: switch (type) {
      "Success" => AlertType.success,
      "Warning" => AlertType.warning,
      "Error" => AlertType.error,
      "Info" => AlertType.info,
      String() => AlertType.none,
    },
    title: title,
    desc: message,
    buttons: [
      DialogButton(
          onPressed: () =>
              Navigator.of(GlobalVar.gNavState.currentContext!).pop(),
          width: 120,
          child: const Text(
            "確認",
            style: TextStyle(color: Colors.white, fontSize: 20),
          ))
    ],
  ).show();
}
