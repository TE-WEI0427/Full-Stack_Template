import 'package:flutter/material.dart';
import 'package:image_picker/image_picker.dart';
import 'package:sample_app/BaseConfig/config_globalvar.dart';
import 'package:sample_app/BaseConfig/config_printlog.dart';

/// 拍照 或 相簿選擇照片 後 回傳圖片路徑
///
/// type =>
///
/// camera : 用相機拍照
///
/// gallery : 讀取相簿
Future<String> getPicturePath(String type) async {
  final ImagePicker _picker = ImagePicker();

  String imgPath = "";

  ImageSource imgSource;

  //ImageSource.gallery 用於讀取相簿
  //ImageSource.camera 即可使用相機拍照

  switch (type) {
    case "camera":
      imgSource = ImageSource.camera;
      break;
    case "gallery":
      imgSource = ImageSource.gallery;
      break;
    default:
      imgSource = ImageSource.camera;
      break;
  }

  XFile? image = await _picker.pickImage(source: imgSource);

  if (image != null) {
    imgPath = image.path;
  }

  return imgPath;
}

/// 跳出圖片上傳方式的提示框
///
/// callback 獲取回傳的圖片路徑字串資料
Future<void> showUploadImageOptionsDialog(Function callback) {
  Widget optionCamera = SimpleDialogOption(
    child: const Text("拍照"),
    onPressed: () async {
      devLog("showUploadImageOptionsDialog", "Camera");
      Navigator.of(GlobalVar.gNavState.currentContext!).pop();
      await getPicturePath("camera").then((value) => callback(value));
    },
  );

  Widget optionGallery = SimpleDialogOption(
    child: const Text("從相簿中選取"),
    onPressed: () async {
      devLog("showUploadImageOptionsDialog", "Gallery");
      Navigator.of(GlobalVar.gNavState.currentContext!).pop();
      await getPicturePath("gallery").then((value) => callback(value));
    },
  );

  SimpleDialog optionsDialog = SimpleDialog(
    title: const Text("選擇照片上傳方式"),
    children: <Widget>[optionCamera, optionGallery],
  );

  return showDialog(
    context: GlobalVar.gNavState.currentContext!,
    barrierDismissible: true, // 可由點擊空白處關閉此提示框
    builder: (BuildContext context) {
      return optionsDialog;
    },
  );
}
